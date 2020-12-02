namespace Microsoft.LocalForwarder.LibraryTest.Library
{
    using ApplicationInsights.DataContracts;
    using LocalForwarder.Library;
    using Opencensus.Proto.Agent.Common.V1;
    using Opencensus.Proto.Agent.Trace.V1;
    using Opencensus.Proto.Trace.V1;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HostTests
    {
        [TestMethod]
        public async Task HostTests_RunsLibrary()
        {
            // ARRANGE
            var telemetryClient = Common.SetupStubTelemetryClient(out var sentItems);

            int portAI = Common.GetPort();
            int portOC = Common.GetPort();

            var config = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<LocalForwarderConfiguration>
  <Inputs>
    <ApplicationInsightsInput Enabled=""true"">
      <Host>0.0.0.0</Host>
      <Port>{portAI}</Port>
    </ApplicationInsightsInput>
    <OpenCensusInput Enabled=""true"">
      <Host>0.0.0.0</Host>
      <Port>{portOC}</Port>
    </OpenCensusInput>
  </Inputs>
  <OpenCensusToApplicationInsights>
    <InstrumentationKey>ikey1</InstrumentationKey>
  </OpenCensusToApplicationInsights>
  <ApplicationInsights>
    <LiveMetricsStreamInstrumentationKey>ikey1</LiveMetricsStreamInstrumentationKey>
    <LiveMetricsStreamAuthenticationApiKey></LiveMetricsStreamAuthenticationApiKey>
    <AdaptiveSampling Enabled=""false"">
      <MaxEventsPerSecond>%APPINSIGHTS_ADAPTIVESAMPLINGEVENTSLIMIT%</MaxEventsPerSecond>
      <!--Telemetry items other than events are counted together-->
      <MaxOtherItemsPerSecond>%APPINSIGHTS_ADAPTIVESAMPLINGNONEVENTSLIMIT%</MaxOtherItemsPerSecond>
    </AdaptiveSampling>
  </ApplicationInsights>
</LocalForwarderConfiguration>
";

            // ACT
            Host host = new Host(telemetryClient);
            host.Run(config, TimeSpan.FromSeconds(5));

            Thread.Sleep(TimeSpan.FromMilliseconds(250));

            var telemetryBatch = new ExportTraceServiceRequest();
            telemetryBatch.Spans.Add(new Span { Name = new TruncatableString { Value = "Span1" }, Kind = Span.Types.SpanKind.Server });
            telemetryBatch.Spans.Add(new Span { Name = new TruncatableString { Value = "Span2" }, Kind = Span.Types.SpanKind.Client });
            telemetryBatch.Node = new Node { ServiceInfo = new ServiceInfo { Name = "tests" } };

            var configRequest = new CurrentLibraryConfig
            {
                Config = new TraceConfig
                {
                    RateLimitingSampler = new RateLimitingSampler { Qps = 1 }
                }
            };

            var writer = new GrpcWriter(false, portOC);
            await writer.Write(telemetryBatch).ConfigureAwait(false);
            await writer.Write(configRequest).ConfigureAwait(false);

            Common.AssertIsTrueEventually(() => sentItems.Count == 3);

            // ASSERT
            Assert.AreEqual("tests", (sentItems.Skip(0).First() as EventTelemetry).Context.Cloud.RoleName);
            Assert.AreEqual("Span1", (sentItems.Skip(1).First() as RequestTelemetry).Name);
            Assert.AreEqual("Span2", (sentItems.Skip(2).First() as DependencyTelemetry).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task HostTests_StopsLibrary()
        {
            // ARRANGE
            var telemetryClient = Common.SetupStubTelemetryClient(out var sentItems);

            int portAI = Common.GetPort();
            int portOC = Common.GetPort();

            var config = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<LocalForwarderConfiguration>
  <Inputs>
    <ApplicationInsightsInput Enabled=""true"">
      <Host>0.0.0.0</Host>
      <Port>{portAI}</Port>
    </ApplicationInsightsInput>
    <OpenCensusInput Enabled=""true"">
      <Host>0.0.0.0</Host>
      <Port>{portOC}</Port>
    </OpenCensusInput>
  </Inputs>
  <OpenCensusToApplicationInsights>
    <InstrumentationKey>ikey1</InstrumentationKey>
  </OpenCensusToApplicationInsights>
</LocalForwarderConfiguration>
";

            Host host = new Host(telemetryClient);
            host.Run(config, TimeSpan.FromSeconds(5));
            Thread.Sleep(TimeSpan.FromMilliseconds(250));

            // ACT
            host.Stop();
            Thread.Sleep(TimeSpan.FromMilliseconds(250));

            var telemetryBatch = new ExportTraceServiceRequest();
            telemetryBatch.Spans.Add(new Span() { Name = new TruncatableString() { Value = "Span1" }, Kind = Span.Types.SpanKind.Server });
            telemetryBatch.Spans.Add(new Span() { Name = new TruncatableString() { Value = "Span2" }, Kind = Span.Types.SpanKind.Client });

            // ASSERT
            var writer = new GrpcWriter(false, portOC);
            await writer.Write(telemetryBatch).ConfigureAwait(false);

            Assert.Fail();
        }

        [TestMethod]
        public async Task HostTests_RestartsLibraryIfStoppedUnexpectedly()
        {
            // ARRANGE
            var telemetryClient = Common.SetupStubTelemetryClient(out var sentItems);

            int portAI = Common.GetPort();
            int portOC = Common.GetPort();

            var config = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<LocalForwarderConfiguration>
  <Inputs>
    <ApplicationInsightsInput Enabled=""true"">
      <Host>0.0.0.0</Host>
      <Port>{portAI}</Port>
    </ApplicationInsightsInput>
    <OpenCensusInput Enabled=""true"">
      <Host>0.0.0.0</Host>
      <Port>{portOC}</Port>
    </OpenCensusInput>
  </Inputs>
  <OpenCensusToApplicationInsights>
    <InstrumentationKey>ikey1</InstrumentationKey>
  </OpenCensusToApplicationInsights>
  <ApplicationInsights>
    <LiveMetricsStreamInstrumentationKey>ikey1</LiveMetricsStreamInstrumentationKey>
    <LiveMetricsStreamAuthenticationApiKey></LiveMetricsStreamAuthenticationApiKey>
    <AdaptiveSampling Enabled=""false"">
      <MaxEventsPerSecond>%APPINSIGHTS_ADAPTIVESAMPLINGEVENTSLIMIT%</MaxEventsPerSecond>
      <!--Telemetry items other than events are counted together-->
      <MaxOtherItemsPerSecond>%APPINSIGHTS_ADAPTIVESAMPLINGNONEVENTSLIMIT%</MaxOtherItemsPerSecond>
    </AdaptiveSampling>
  </ApplicationInsights>
</LocalForwarderConfiguration>
";

            Host host = new Host(telemetryClient);
            host.Run(config, TimeSpan.FromSeconds(1));

            FieldInfo libraryFieldInfo = host.GetType().GetField("library", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            Common.AssertIsTrueEventually(() => libraryFieldInfo.GetValue(host) != null);

            // ACT
            // stop the existing library (as if something went wrong)
            var library = libraryFieldInfo.GetValue(host) as Library;
            library.Stop();

            Common.AssertIsTrueEventually(() => !library.IsRunning);

            // ASSERT
            // wait for a new library
            Common.AssertIsTrueEventually(() => libraryFieldInfo.GetValue(host) != null);
            Common.AssertIsTrueEventually(() => (libraryFieldInfo.GetValue(host) as Library).IsRunning);

            // verify the new library works
            var telemetryBatch = new ExportTraceServiceRequest();
            telemetryBatch.Spans.Add(new Span() { Name = new TruncatableString() { Value = "Span1" }, Kind = Span.Types.SpanKind.Server });
            telemetryBatch.Spans.Add(new Span() { Name = new TruncatableString() { Value = "Span2" }, Kind = Span.Types.SpanKind.Client });

            var writer = new GrpcWriter(false, portOC);
            await writer.Write(telemetryBatch).ConfigureAwait(false);

            Common.AssertIsTrueEventually(() => sentItems.Count == 2);
            
            Assert.AreEqual("Span1", (sentItems.Skip(0).First() as RequestTelemetry).Name);
            Assert.AreEqual("Span2", (sentItems.Skip(1).First() as DependencyTelemetry).Name);
        }
    }
}