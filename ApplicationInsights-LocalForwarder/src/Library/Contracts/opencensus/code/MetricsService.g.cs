// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: agent/metrics/v1/metrics_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Opencensus.Proto.Agent.Metrics.V1 {

  /// <summary>Holder for reflection information generated from agent/metrics/v1/metrics_service.proto</summary>
  public static partial class MetricsServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for agent/metrics/v1/metrics_service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MetricsServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiZhZ2VudC9tZXRyaWNzL3YxL21ldHJpY3Nfc2VydmljZS5wcm90bxIhb3Bl",
            "bmNlbnN1cy5wcm90by5hZ2VudC5tZXRyaWNzLnYxGi1vcGVuY2Vuc3VzL3By",
            "b3RvL2FnZW50L2NvbW1vbi92MS9jb21tb24ucHJvdG8aKW9wZW5jZW5zdXMv",
            "cHJvdG8vbWV0cmljcy92MS9tZXRyaWNzLnByb3RvIokBChtFeHBvcnRNZXRy",
            "aWNzU2VydmljZVJlcXVlc3QSNAoEbm9kZRgBIAEoCzImLm9wZW5jZW5zdXMu",
            "cHJvdG8uYWdlbnQuY29tbW9uLnYxLk5vZGUSNAoHbWV0cmljcxgCIAMoCzIj",
            "Lm9wZW5jZW5zdXMucHJvdG8ubWV0cmljcy52MS5NZXRyaWMiHgocRXhwb3J0",
            "TWV0cmljc1NlcnZpY2VSZXNwb25zZTKiAQoOTWV0cmljc1NlcnZpY2USjwEK",
            "BkV4cG9ydBI+Lm9wZW5jZW5zdXMucHJvdG8uYWdlbnQubWV0cmljcy52MS5F",
            "eHBvcnRNZXRyaWNzU2VydmljZVJlcXVlc3QaPy5vcGVuY2Vuc3VzLnByb3Rv",
            "LmFnZW50Lm1ldHJpY3MudjEuRXhwb3J0TWV0cmljc1NlcnZpY2VSZXNwb25z",
            "ZSIAKAEwAUKJAQokaW8ub3BlbmNlbnN1cy5wcm90by5hZ2VudC5tZXRyaWNz",
            "LnYxQhNNZXRyaWNzU2VydmljZVByb3RvUAFaSmdpdGh1Yi5jb20vY2Vuc3Vz",
            "LWluc3RydW1lbnRhdGlvbi9vcGVuY2Vuc3VzLXByb3RvL2dlbi1nby9hZ2Vu",
            "dC9tZXRyaWNzL3YxYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Opencensus.Proto.Agent.Common.V1.CommonReflection.Descriptor, global::Opencensus.Proto.Metrics.V1.MetricsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Opencensus.Proto.Agent.Metrics.V1.ExportMetricsServiceRequest), global::Opencensus.Proto.Agent.Metrics.V1.ExportMetricsServiceRequest.Parser, new[]{ "Node", "Metrics" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Opencensus.Proto.Agent.Metrics.V1.ExportMetricsServiceResponse), global::Opencensus.Proto.Agent.Metrics.V1.ExportMetricsServiceResponse.Parser, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ExportMetricsServiceRequest : pb::IMessage<ExportMetricsServiceRequest> {
    private static readonly pb::MessageParser<ExportMetricsServiceRequest> _parser = new pb::MessageParser<ExportMetricsServiceRequest>(() => new ExportMetricsServiceRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ExportMetricsServiceRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Opencensus.Proto.Agent.Metrics.V1.MetricsServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExportMetricsServiceRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExportMetricsServiceRequest(ExportMetricsServiceRequest other) : this() {
      node_ = other.node_ != null ? other.node_.Clone() : null;
      metrics_ = other.metrics_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExportMetricsServiceRequest Clone() {
      return new ExportMetricsServiceRequest(this);
    }

    /// <summary>Field number for the "node" field.</summary>
    public const int NodeFieldNumber = 1;
    private global::Opencensus.Proto.Agent.Common.V1.Node node_;
    /// <summary>
    /// This is required only in the first message on the stream or if the
    /// previous sent ExportMetricsServiceRequest message has a different Node (e.g.
    /// when the same RPC is used to send Metrics from multiple Applications).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Opencensus.Proto.Agent.Common.V1.Node Node {
      get { return node_; }
      set {
        node_ = value;
      }
    }

    /// <summary>Field number for the "metrics" field.</summary>
    public const int MetricsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Opencensus.Proto.Metrics.V1.Metric> _repeated_metrics_codec
        = pb::FieldCodec.ForMessage(18, global::Opencensus.Proto.Metrics.V1.Metric.Parser);
    private readonly pbc::RepeatedField<global::Opencensus.Proto.Metrics.V1.Metric> metrics_ = new pbc::RepeatedField<global::Opencensus.Proto.Metrics.V1.Metric>();
    /// <summary>
    /// A list of metrics that belong to the last received Node.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Opencensus.Proto.Metrics.V1.Metric> Metrics {
      get { return metrics_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ExportMetricsServiceRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ExportMetricsServiceRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Node, other.Node)) return false;
      if(!metrics_.Equals(other.metrics_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (node_ != null) hash ^= Node.GetHashCode();
      hash ^= metrics_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (node_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Node);
      }
      metrics_.WriteTo(output, _repeated_metrics_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (node_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Node);
      }
      size += metrics_.CalculateSize(_repeated_metrics_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ExportMetricsServiceRequest other) {
      if (other == null) {
        return;
      }
      if (other.node_ != null) {
        if (node_ == null) {
          node_ = new global::Opencensus.Proto.Agent.Common.V1.Node();
        }
        Node.MergeFrom(other.Node);
      }
      metrics_.Add(other.metrics_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (node_ == null) {
              node_ = new global::Opencensus.Proto.Agent.Common.V1.Node();
            }
            input.ReadMessage(node_);
            break;
          }
          case 18: {
            metrics_.AddEntriesFrom(input, _repeated_metrics_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class ExportMetricsServiceResponse : pb::IMessage<ExportMetricsServiceResponse> {
    private static readonly pb::MessageParser<ExportMetricsServiceResponse> _parser = new pb::MessageParser<ExportMetricsServiceResponse>(() => new ExportMetricsServiceResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ExportMetricsServiceResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Opencensus.Proto.Agent.Metrics.V1.MetricsServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExportMetricsServiceResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExportMetricsServiceResponse(ExportMetricsServiceResponse other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExportMetricsServiceResponse Clone() {
      return new ExportMetricsServiceResponse(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ExportMetricsServiceResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ExportMetricsServiceResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ExportMetricsServiceResponse other) {
      if (other == null) {
        return;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
