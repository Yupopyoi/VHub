// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mediapipe/calculators/image/mask_overlay_calculator.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Mediapipe {

  /// <summary>Holder for reflection information generated from mediapipe/calculators/image/mask_overlay_calculator.proto</summary>
  public static partial class MaskOverlayCalculatorReflection {

    #region Descriptor
    /// <summary>File descriptor for mediapipe/calculators/image/mask_overlay_calculator.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MaskOverlayCalculatorReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjltZWRpYXBpcGUvY2FsY3VsYXRvcnMvaW1hZ2UvbWFza19vdmVybGF5X2Nh",
            "bGN1bGF0b3IucHJvdG8SCW1lZGlhcGlwZRokbWVkaWFwaXBlL2ZyYW1ld29y",
            "ay9jYWxjdWxhdG9yLnByb3RvIvUBChxNYXNrT3ZlcmxheUNhbGN1bGF0b3JP",
            "cHRpb25zEk4KDG1hc2tfY2hhbm5lbBgBIAEoDjIzLm1lZGlhcGlwZS5NYXNr",
            "T3ZlcmxheUNhbGN1bGF0b3JPcHRpb25zLk1hc2tDaGFubmVsOgNSRUQiLgoL",
            "TWFza0NoYW5uZWwSCwoHVU5LTk9XThAAEgcKA1JFRBABEgkKBUFMUEhBEAIy",
            "VQoDZXh0EhwubWVkaWFwaXBlLkNhbGN1bGF0b3JPcHRpb25zGILgnHggASgL",
            "MicubWVkaWFwaXBlLk1hc2tPdmVybGF5Q2FsY3VsYXRvck9wdGlvbnM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Mediapipe.CalculatorReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Mediapipe.MaskOverlayCalculatorOptions), global::Mediapipe.MaskOverlayCalculatorOptions.Parser, new[]{ "MaskChannel" }, null, new[]{ typeof(global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel) }, new pb::Extension[] { global::Mediapipe.MaskOverlayCalculatorOptions.Extensions.Ext }, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class MaskOverlayCalculatorOptions : pb::IMessage<MaskOverlayCalculatorOptions>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<MaskOverlayCalculatorOptions> _parser = new pb::MessageParser<MaskOverlayCalculatorOptions>(() => new MaskOverlayCalculatorOptions());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<MaskOverlayCalculatorOptions> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mediapipe.MaskOverlayCalculatorReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public MaskOverlayCalculatorOptions() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public MaskOverlayCalculatorOptions(MaskOverlayCalculatorOptions other) : this() {
      _hasBits0 = other._hasBits0;
      maskChannel_ = other.maskChannel_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public MaskOverlayCalculatorOptions Clone() {
      return new MaskOverlayCalculatorOptions(this);
    }

    /// <summary>Field number for the "mask_channel" field.</summary>
    public const int MaskChannelFieldNumber = 1;
    private readonly static global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel MaskChannelDefaultValue = global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel.Red;

    private global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel maskChannel_;
    /// <summary>
    /// Selects which channel of the MASK input to use for masking.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel MaskChannel {
      get { if ((_hasBits0 & 1) != 0) { return maskChannel_; } else { return MaskChannelDefaultValue; } }
      set {
        _hasBits0 |= 1;
        maskChannel_ = value;
      }
    }
    /// <summary>Gets whether the "mask_channel" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasMaskChannel {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "mask_channel" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearMaskChannel() {
      _hasBits0 &= ~1;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as MaskOverlayCalculatorOptions);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(MaskOverlayCalculatorOptions other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MaskChannel != other.MaskChannel) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasMaskChannel) hash ^= MaskChannel.GetHashCode();
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
      if (HasMaskChannel) {
        output.WriteRawTag(8);
        output.WriteEnum((int) MaskChannel);
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
      if (HasMaskChannel) {
        output.WriteRawTag(8);
        output.WriteEnum((int) MaskChannel);
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
      if (HasMaskChannel) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) MaskChannel);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(MaskOverlayCalculatorOptions other) {
      if (other == null) {
        return;
      }
      if (other.HasMaskChannel) {
        MaskChannel = other.MaskChannel;
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
          case 8: {
            MaskChannel = (global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel) input.ReadEnum();
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
          case 8: {
            MaskChannel = (global::Mediapipe.MaskOverlayCalculatorOptions.Types.MaskChannel) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the MaskOverlayCalculatorOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      public enum MaskChannel {
        [pbr::OriginalName("UNKNOWN")] Unknown = 0,
        [pbr::OriginalName("RED")] Red = 1,
        [pbr::OriginalName("ALPHA")] Alpha = 2,
      }

    }
    #endregion

    #region Extensions
    /// <summary>Container for extensions for other messages declared in the MaskOverlayCalculatorOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Extensions {
      public static readonly pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.MaskOverlayCalculatorOptions> Ext =
        new pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.MaskOverlayCalculatorOptions>(252129282, pb::FieldCodec.ForMessage(2017034258, global::Mediapipe.MaskOverlayCalculatorOptions.Parser));
    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code