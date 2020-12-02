﻿namespace Microsoft.LocalForwarder.Library
{
    using ApplicationInsights.Channel;
    using ApplicationInsights.DataContracts;
    using ApplicationInsights.Extensibility.Implementation;
    using Common;
    using Google.Protobuf.Collections;
    using Inputs.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    static class AiTelemetryConverter
    {
        private static readonly ContextTagKeys TagKeys = new ContextTagKeys();
        private const string DefaultSdkVersion = "lf_unspecified:0.0.0";
        
        public static EventTelemetry ConvertEventToSdkApi(Telemetry inputTelemetry)
        {
            var result = new EventTelemetry();

            result.Name = inputTelemetry.Event.Name;
            result.Properties.PopulateFromProtobuf(inputTelemetry.Event.Properties);
            result.Metrics.PopulateFromProtobuf(inputTelemetry.Event.Measurements);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            AiTelemetryConverter.CopySamplingFields(inputTelemetry, result);

            return result;
        }

        public static TraceTelemetry ConvertTraceToSdkApi(Telemetry inputTelemetry)
        {
            var result = new TraceTelemetry();

            result.Message = inputTelemetry.Message.Message_;

            result.SeverityLevel = AiTelemetryConverter.ConvertSeverityLevel(inputTelemetry.Message.SeverityLevel);

            result.Properties.PopulateFromProtobuf(inputTelemetry.Message.Properties);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            AiTelemetryConverter.CopySamplingFields(inputTelemetry, result);

            return result;
        }

        public static MetricTelemetry ConvertMetricToSdkApi(Telemetry inputTelemetry)
        {
            // only one metric in the list is currently supported by Application Insights storage. If multiple data points were sent only the first one will be used
            if (!inputTelemetry.Metric.Metrics.Any())
            {
                throw new ArgumentException("Metrics list can't be empty");
            }

            DataPoint firstMetric = inputTelemetry.Metric.Metrics[0];

            MetricTelemetry result;

            switch (firstMetric.Kind)
            {
                case DataPointType.Measurement:
                    result = new MetricTelemetry(firstMetric.Name, firstMetric.Value);
                    break;
                case DataPointType.Aggregation:
                    firstMetric.Count = firstMetric.Count ?? new Google.Protobuf.WellKnownTypes.Int32Value() { Value = 1 };
                    firstMetric.Min = firstMetric.Min ?? new Google.Protobuf.WellKnownTypes.DoubleValue() { Value = firstMetric.Value };
                    firstMetric.Max = firstMetric.Max ?? new Google.Protobuf.WellKnownTypes.DoubleValue() { Value = firstMetric.Value };
                    firstMetric.StdDev = firstMetric.StdDev ?? new Google.Protobuf.WellKnownTypes.DoubleValue() { Value = 0 };

                    result = new MetricTelemetry(firstMetric.Name, firstMetric.Count.Value, firstMetric.Value, firstMetric.Min.Value, firstMetric.Max.Value, firstMetric.StdDev.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(FormattableString.Invariant($"Unknown aggregation Kind: {firstMetric.Kind}"));
            }

            result.MetricNamespace = firstMetric.Ns;

            result.Properties.PopulateFromProtobuf(inputTelemetry.Metric.Properties);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            
            return result;
        }

        public static ExceptionTelemetry ConvertExceptionToSdkApi(Telemetry inputTelemetry)
        {
            //!!! make sure we're onboarded onto a release version of AI SDK
            var item = inputTelemetry.Exception;

            var result = new ExceptionTelemetry(
                item.Exceptions.Select(ed => new ExceptionDetailsInfo(ed.Id, ed.OuterId, ed.TypeName, ed.Message, ed.HasFullStack?.Value ?? true, ed.Stack,
                    ed.ParsedStack.Select(f => new Microsoft.ApplicationInsights.DataContracts.StackFrame(f.Assembly, f.FileName, f.Level, f.Line, f.Method)))),
                AiTelemetryConverter.ConvertSeverityLevel(item.SeverityLevel), item.ProblemId, item.Properties, item.Measurements);

            
            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            AiTelemetryConverter.CopySamplingFields(inputTelemetry, result);

            return result;
        }

        public static DependencyTelemetry ConvertDependencyToSdkApi(Telemetry inputTelemetry)
        {
            var result = new DependencyTelemetry();

            Dependency item = inputTelemetry.Dependency;

            result.Type = item.Type;
            result.Target = item.Target;
            result.Name = item.Name;
            result.Data = item.Data;

            if (DateTimeOffset.TryParseExact(inputTelemetry.DateTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            {
                result.Timestamp = timestamp;
            }

            result.Duration = item.Duration?.ToTimeSpan() ?? TimeSpan.Zero;
            result.ResultCode = item.ResultCode;
            result.Success = item.Success?.Value ?? true;
            result.Id = item.Id;
            
            result.Properties.PopulateFromProtobuf(item.Properties);
            result.Metrics.PopulateFromProtobuf(item.Measurements);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            AiTelemetryConverter.CopySamplingFields(inputTelemetry, result);

            return result;
        }

        public static AvailabilityTelemetry ConvertAvailabilityToSdkApi(Telemetry inputTelemetry)
        {
            var result = new AvailabilityTelemetry();

            Availability item = inputTelemetry.Availability;

            result.Id = item.Id;
            result.Name = item.Name;

            if (DateTimeOffset.TryParseExact(inputTelemetry.DateTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            {
                result.Timestamp = timestamp;
            }

            result.Duration = item.Duration?.ToTimeSpan() ?? TimeSpan.Zero;
            result.RunLocation = item.RunLocation;
            result.Success = item.Success;
            result.Message = item.Message;
            
            result.Properties.PopulateFromProtobuf(item.Properties);
            result.Metrics.PopulateFromProtobuf(item.Measurements);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            
            return result;
        }

        public static PageViewTelemetry ConvertPageViewToSdkApi(Telemetry inputTelemetry)
        {
            var result = new PageViewTelemetry();

            PageView item = inputTelemetry.PageView;

            item.Event = item.Event ?? new Event();

            result.Id = item.Id;

            if (Uri.TryCreate(item.Url, UriKind.RelativeOrAbsolute, out var uri))
            {
                result.Url = uri;
            }

            result.Duration = item.Duration?.ToTimeSpan() ?? TimeSpan.Zero;
            
            result.Name = item.Event.Name;

            result.Properties.PopulateFromProtobuf(item.Event?.Properties);
            result.Metrics.PopulateFromProtobuf(item.Event?.Measurements);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            AiTelemetryConverter.CopySamplingFields(inputTelemetry, result);

            return result;
        }

        public static RequestTelemetry ConvertRequestToSdkApi(Telemetry inputTelemetry)
        {
            var result = new RequestTelemetry();

            Request item = inputTelemetry.Request;

            result.Id = item.Id;
            result.Name = item.Name;

            if (DateTimeOffset.TryParseExact(inputTelemetry.DateTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            {
                result.Timestamp = timestamp;
            }

            result.Duration = item.Duration?.ToTimeSpan() ?? TimeSpan.Zero;
            result.ResponseCode = item.ResponseCode;
            result.Success = item.Success?.Value;
            result.Source = item.Source;

            if (Uri.TryCreate(item.Url, UriKind.RelativeOrAbsolute, out Uri url))
            {
                result.Url = url;
            }

            result.Properties.PopulateFromProtobuf(item.Properties);
            result.Metrics.PopulateFromProtobuf(item.Measurements);

            AiTelemetryConverter.CopyCommonFields(inputTelemetry, result);
            AiTelemetryConverter.CopySamplingFields(inputTelemetry, result);

            return result;
        }

        private static void CopyCommonFields(Telemetry inputTelemetry, ITelemetry telemetry)
        {
            telemetry.Sequence = inputTelemetry.SequenceNumber;
            if (DateTimeOffset.TryParseExact(inputTelemetry.DateTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            {
                telemetry.Timestamp = timestamp;
            }
            telemetry.Context.InstrumentationKey = inputTelemetry.InstrumentationKey;

            AiTelemetryConverter.PopulateContext(inputTelemetry, telemetry);
        }

        private static void CopySamplingFields(Telemetry inputTelemetry, ISupportSampling telemetry)
        {
            telemetry.SamplingPercentage = null; // inputTelemetry.SamplingRate?.Value; always sample (if configured), regardless of whether it was sampled previousely on the client
        }

        private static void PopulateContext(Telemetry telemetryItem, ITelemetry telemetry)
        {
            foreach (var tag in telemetryItem.Tags)
            {
                if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.ApplicationVersion, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Component.Version = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.CloudRole, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Cloud.RoleName = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.CloudRoleInstance, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Cloud.RoleInstance = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.DeviceId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Device.Id = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.DeviceLocale, StringComparison.InvariantCultureIgnoreCase))
                {
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.DeviceModel, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Device.Model = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.DeviceOEMName, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Device.OemName = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.DeviceOSVersion, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Device.OperatingSystem = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.DeviceType, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Device.Type = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.InternalAgentVersion, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.GetInternalContext().AgentVersion = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.InternalNodeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.GetInternalContext().NodeName = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.InternalSdkVersion, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.GetInternalContext().SdkVersion = string.Concat("lf_", tag.Value);
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.LocationIp, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Location.Ip = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.OperationCorrelationVector, StringComparison.InvariantCultureIgnoreCase))
                {
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.OperationId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Operation.Id = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.OperationName, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Operation.Name = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.OperationParentId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Operation.ParentId = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.OperationSyntheticSource, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Operation.SyntheticSource = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.SessionId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Session.Id = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.SessionIsFirst, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.Session.IsFirst = false;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.UserAccountId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.User.AccountId = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.UserAgent, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.User.UserAgent = tag.Value; //TODO: Deprecated???
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.UserAuthUserId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.User.AuthenticatedUserId = tag.Value;
                }
                else if (string.Equals(tag.Key, AiTelemetryConverter.TagKeys.UserId, StringComparison.InvariantCultureIgnoreCase))
                {
                    telemetry.Context.User.Id = tag.Value;
                }
                else
                {
                    // unknown tag, log and ignore
                    Diagnostics.LogTrace(FormattableString.Invariant($"Unknown tag. Ignoring. {tag.Key}"));
                }
            }

            if (string.IsNullOrEmpty(telemetry.Context.GetInternalContext().SdkVersion))
            {
                telemetry.Context.GetInternalContext().SdkVersion = DefaultSdkVersion;
            }
        }

        private static void PopulateFromProtobuf<TKey, TValue>(this IDictionary<TKey, TValue> destination, MapField<TKey, TValue> source)
        {
            foreach (var keyValuePair in source ?? new MapField<TKey, TValue>())
            {
                destination.Add(keyValuePair);
            }
        }

        private static Microsoft.ApplicationInsights.DataContracts.SeverityLevel? ConvertSeverityLevel(Inputs.Contracts.SeverityLevel inputSeverityLevel)
        {
            switch (inputSeverityLevel)
            {
                case Inputs.Contracts.SeverityLevel.Verbose:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Verbose;
                case Inputs.Contracts.SeverityLevel.Information:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information;
                case Inputs.Contracts.SeverityLevel.Warning:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning;
                case Inputs.Contracts.SeverityLevel.Error:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error;
                case Inputs.Contracts.SeverityLevel.Critical:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Critical;
                default:
                    return null;
            }
        }
    }
}
