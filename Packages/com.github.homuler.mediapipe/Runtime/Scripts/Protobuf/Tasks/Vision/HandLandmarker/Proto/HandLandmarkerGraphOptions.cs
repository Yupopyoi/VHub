// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mediapipe/tasks/cc/vision/hand_landmarker/proto/hand_landmarker_graph_options.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Mediapipe.Tasks.Vision.HandLandmarker.Proto {

  /// <summary>Holder for reflection information generated from mediapipe/tasks/cc/vision/hand_landmarker/proto/hand_landmarker_graph_options.proto</summary>
  public static partial class HandLandmarkerGraphOptionsReflection {

    #region Descriptor
    /// <summary>File descriptor for mediapipe/tasks/cc/vision/hand_landmarker/proto/hand_landmarker_graph_options.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HandLandmarkerGraphOptionsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ClNtZWRpYXBpcGUvdGFza3MvY2MvdmlzaW9uL2hhbmRfbGFuZG1hcmtlci9w",
            "cm90by9oYW5kX2xhbmRtYXJrZXJfZ3JhcGhfb3B0aW9ucy5wcm90bxIsbWVk",
            "aWFwaXBlLnRhc2tzLnZpc2lvbi5oYW5kX2xhbmRtYXJrZXIucHJvdG8aJG1l",
            "ZGlhcGlwZS9mcmFtZXdvcmsvY2FsY3VsYXRvci5wcm90bxosbWVkaWFwaXBl",
            "L2ZyYW1ld29yay9jYWxjdWxhdG9yX29wdGlvbnMucHJvdG8aMG1lZGlhcGlw",
            "ZS90YXNrcy9jYy9jb3JlL3Byb3RvL2Jhc2Vfb3B0aW9ucy5wcm90bxpPbWVk",
            "aWFwaXBlL3Rhc2tzL2NjL3Zpc2lvbi9oYW5kX2RldGVjdG9yL3Byb3RvL2hh",
            "bmRfZGV0ZWN0b3JfZ3JhcGhfb3B0aW9ucy5wcm90bxpbbWVkaWFwaXBlL3Rh",
            "c2tzL2NjL3Zpc2lvbi9oYW5kX2xhbmRtYXJrZXIvcHJvdG8vaGFuZF9sYW5k",
            "bWFya3NfZGV0ZWN0b3JfZ3JhcGhfb3B0aW9ucy5wcm90byLlAwoaSGFuZExh",
            "bmRtYXJrZXJHcmFwaE9wdGlvbnMSPQoMYmFzZV9vcHRpb25zGAEgASgLMicu",
            "bWVkaWFwaXBlLnRhc2tzLmNvcmUucHJvdG8uQmFzZU9wdGlvbnMSaQobaGFu",
            "ZF9kZXRlY3Rvcl9ncmFwaF9vcHRpb25zGAIgASgLMkQubWVkaWFwaXBlLnRh",
            "c2tzLnZpc2lvbi5oYW5kX2RldGVjdG9yLnByb3RvLkhhbmREZXRlY3Rvckdy",
            "YXBoT3B0aW9ucxJ+CiVoYW5kX2xhbmRtYXJrc19kZXRlY3Rvcl9ncmFwaF9v",
            "cHRpb25zGAMgASgLMk8ubWVkaWFwaXBlLnRhc2tzLnZpc2lvbi5oYW5kX2xh",
            "bmRtYXJrZXIucHJvdG8uSGFuZExhbmRtYXJrc0RldGVjdG9yR3JhcGhPcHRp",
            "b25zEiQKF21pbl90cmFja2luZ19jb25maWRlbmNlGAQgASgCOgMwLjUydwoD",
            "ZXh0EhwubWVkaWFwaXBlLkNhbGN1bGF0b3JPcHRpb25zGPLi0dwBIAEoCzJI",
            "Lm1lZGlhcGlwZS50YXNrcy52aXNpb24uaGFuZF9sYW5kbWFya2VyLnByb3Rv",
            "LkhhbmRMYW5kbWFya2VyR3JhcGhPcHRpb25zQlkKNmNvbS5nb29nbGUubWVk",
            "aWFwaXBlLnRhc2tzLnZpc2lvbi5oYW5kbGFuZG1hcmtlci5wcm90b0IfSGFu",
            "ZExhbmRtYXJrZXJHcmFwaE9wdGlvbnNQcm90bw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Mediapipe.CalculatorReflection.Descriptor, global::Mediapipe.CalculatorOptionsReflection.Descriptor, global::Mediapipe.Tasks.Core.Proto.BaseOptionsReflection.Descriptor, global::Mediapipe.Tasks.Vision.HandDetector.Proto.HandDetectorGraphOptionsReflection.Descriptor, global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarksDetectorGraphOptionsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions), global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions.Parser, new[]{ "BaseOptions", "HandDetectorGraphOptions", "HandLandmarksDetectorGraphOptions", "MinTrackingConfidence" }, null, null, new pb::Extension[] { global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions.Extensions.Ext }, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class HandLandmarkerGraphOptions : pb::IMessage<HandLandmarkerGraphOptions>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<HandLandmarkerGraphOptions> _parser = new pb::MessageParser<HandLandmarkerGraphOptions>(() => new HandLandmarkerGraphOptions());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<HandLandmarkerGraphOptions> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptionsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HandLandmarkerGraphOptions() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HandLandmarkerGraphOptions(HandLandmarkerGraphOptions other) : this() {
      _hasBits0 = other._hasBits0;
      baseOptions_ = other.baseOptions_ != null ? other.baseOptions_.Clone() : null;
      handDetectorGraphOptions_ = other.handDetectorGraphOptions_ != null ? other.handDetectorGraphOptions_.Clone() : null;
      handLandmarksDetectorGraphOptions_ = other.handLandmarksDetectorGraphOptions_ != null ? other.handLandmarksDetectorGraphOptions_.Clone() : null;
      minTrackingConfidence_ = other.minTrackingConfidence_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HandLandmarkerGraphOptions Clone() {
      return new HandLandmarkerGraphOptions(this);
    }

    /// <summary>Field number for the "base_options" field.</summary>
    public const int BaseOptionsFieldNumber = 1;
    private global::Mediapipe.Tasks.Core.Proto.BaseOptions baseOptions_;
    /// <summary>
    /// Base options for configuring MediaPipe Tasks, such as specifying the model
    /// asset bundle file with metadata, accelerator options, etc.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Core.Proto.BaseOptions BaseOptions {
      get { return baseOptions_; }
      set {
        baseOptions_ = value;
      }
    }

    /// <summary>Field number for the "hand_detector_graph_options" field.</summary>
    public const int HandDetectorGraphOptionsFieldNumber = 2;
    private global::Mediapipe.Tasks.Vision.HandDetector.Proto.HandDetectorGraphOptions handDetectorGraphOptions_;
    /// <summary>
    /// Options for hand detector graph.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Vision.HandDetector.Proto.HandDetectorGraphOptions HandDetectorGraphOptions {
      get { return handDetectorGraphOptions_; }
      set {
        handDetectorGraphOptions_ = value;
      }
    }

    /// <summary>Field number for the "hand_landmarks_detector_graph_options" field.</summary>
    public const int HandLandmarksDetectorGraphOptionsFieldNumber = 3;
    private global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarksDetectorGraphOptions handLandmarksDetectorGraphOptions_;
    /// <summary>
    /// Options for hand landmarker subgraph.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarksDetectorGraphOptions HandLandmarksDetectorGraphOptions {
      get { return handLandmarksDetectorGraphOptions_; }
      set {
        handLandmarksDetectorGraphOptions_ = value;
      }
    }

    /// <summary>Field number for the "min_tracking_confidence" field.</summary>
    public const int MinTrackingConfidenceFieldNumber = 4;
    private readonly static float MinTrackingConfidenceDefaultValue = 0.5F;

    private float minTrackingConfidence_;
    /// <summary>
    /// Minimum confidence for hand landmarks tracking to be considered
    /// successfully.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float MinTrackingConfidence {
      get { if ((_hasBits0 & 1) != 0) { return minTrackingConfidence_; } else { return MinTrackingConfidenceDefaultValue; } }
      set {
        _hasBits0 |= 1;
        minTrackingConfidence_ = value;
      }
    }
    /// <summary>Gets whether the "min_tracking_confidence" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasMinTrackingConfidence {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "min_tracking_confidence" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearMinTrackingConfidence() {
      _hasBits0 &= ~1;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as HandLandmarkerGraphOptions);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(HandLandmarkerGraphOptions other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(BaseOptions, other.BaseOptions)) return false;
      if (!object.Equals(HandDetectorGraphOptions, other.HandDetectorGraphOptions)) return false;
      if (!object.Equals(HandLandmarksDetectorGraphOptions, other.HandLandmarksDetectorGraphOptions)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(MinTrackingConfidence, other.MinTrackingConfidence)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (baseOptions_ != null) hash ^= BaseOptions.GetHashCode();
      if (handDetectorGraphOptions_ != null) hash ^= HandDetectorGraphOptions.GetHashCode();
      if (handLandmarksDetectorGraphOptions_ != null) hash ^= HandLandmarksDetectorGraphOptions.GetHashCode();
      if (HasMinTrackingConfidence) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(MinTrackingConfidence);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (baseOptions_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(BaseOptions);
      }
      if (handDetectorGraphOptions_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(HandDetectorGraphOptions);
      }
      if (handLandmarksDetectorGraphOptions_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(HandLandmarksDetectorGraphOptions);
      }
      if (HasMinTrackingConfidence) {
        output.WriteRawTag(37);
        output.WriteFloat(MinTrackingConfidence);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (baseOptions_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(BaseOptions);
      }
      if (handDetectorGraphOptions_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(HandDetectorGraphOptions);
      }
      if (handLandmarksDetectorGraphOptions_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(HandLandmarksDetectorGraphOptions);
      }
      if (HasMinTrackingConfidence) {
        output.WriteRawTag(37);
        output.WriteFloat(MinTrackingConfidence);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (baseOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BaseOptions);
      }
      if (handDetectorGraphOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(HandDetectorGraphOptions);
      }
      if (handLandmarksDetectorGraphOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(HandLandmarksDetectorGraphOptions);
      }
      if (HasMinTrackingConfidence) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(HandLandmarkerGraphOptions other) {
      if (other == null) {
        return;
      }
      if (other.baseOptions_ != null) {
        if (baseOptions_ == null) {
          BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
        }
        BaseOptions.MergeFrom(other.BaseOptions);
      }
      if (other.handDetectorGraphOptions_ != null) {
        if (handDetectorGraphOptions_ == null) {
          HandDetectorGraphOptions = new global::Mediapipe.Tasks.Vision.HandDetector.Proto.HandDetectorGraphOptions();
        }
        HandDetectorGraphOptions.MergeFrom(other.HandDetectorGraphOptions);
      }
      if (other.handLandmarksDetectorGraphOptions_ != null) {
        if (handLandmarksDetectorGraphOptions_ == null) {
          HandLandmarksDetectorGraphOptions = new global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarksDetectorGraphOptions();
        }
        HandLandmarksDetectorGraphOptions.MergeFrom(other.HandLandmarksDetectorGraphOptions);
      }
      if (other.HasMinTrackingConfidence) {
        MinTrackingConfidence = other.MinTrackingConfidence;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (baseOptions_ == null) {
              BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
            }
            input.ReadMessage(BaseOptions);
            break;
          }
          case 18: {
            if (handDetectorGraphOptions_ == null) {
              HandDetectorGraphOptions = new global::Mediapipe.Tasks.Vision.HandDetector.Proto.HandDetectorGraphOptions();
            }
            input.ReadMessage(HandDetectorGraphOptions);
            break;
          }
          case 26: {
            if (handLandmarksDetectorGraphOptions_ == null) {
              HandLandmarksDetectorGraphOptions = new global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarksDetectorGraphOptions();
            }
            input.ReadMessage(HandLandmarksDetectorGraphOptions);
            break;
          }
          case 37: {
            MinTrackingConfidence = input.ReadFloat();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (baseOptions_ == null) {
              BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
            }
            input.ReadMessage(BaseOptions);
            break;
          }
          case 18: {
            if (handDetectorGraphOptions_ == null) {
              HandDetectorGraphOptions = new global::Mediapipe.Tasks.Vision.HandDetector.Proto.HandDetectorGraphOptions();
            }
            input.ReadMessage(HandDetectorGraphOptions);
            break;
          }
          case 26: {
            if (handLandmarksDetectorGraphOptions_ == null) {
              HandLandmarksDetectorGraphOptions = new global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarksDetectorGraphOptions();
            }
            input.ReadMessage(HandLandmarksDetectorGraphOptions);
            break;
          }
          case 37: {
            MinTrackingConfidence = input.ReadFloat();
            break;
          }
        }
      }
    }
    #endif

    #region Extensions
    /// <summary>Container for extensions for other messages declared in the HandLandmarkerGraphOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Extensions {
      public static readonly pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions> Ext =
        new pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions>(462713202, pb::FieldCodec.ForMessage(3701705618, global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions.Parser));
    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
