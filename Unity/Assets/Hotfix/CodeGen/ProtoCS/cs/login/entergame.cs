// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: entergame.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace pb
{

    [global::ProtoBuf.ProtoContract(Name = @"C2S_EnterGame")]
    public partial class C2SEnterGame : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"roleid")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Roleid { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract(Name = @"S2C_EnterGame")]
    public partial class S2CEnterGame : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"error")]
        public ErrCode Error { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"self")]
        public Entity Self { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"entitys")]
        public global::System.Collections.Generic.List<Entity> Entitys { get; } = new global::System.Collections.Generic.List<Entity>();

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion