// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: bcst_scene.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace Pb
{

    [global::ProtoBuf.ProtoContract(Name = @"Bcst_UnitIntoView")]
    public partial class BcstUnitIntoView : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"role")]
        public Entity Role { get; set; }

    }

    [global::ProtoBuf.ProtoContract(Name = @"Bcst_UnitOutofView")]
    public partial class BcstUnitOutofView : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint roleId { get; set; }

    }

    [global::ProtoBuf.ProtoContract(Name = @"Bcst_UnitMove")]
    public partial class BcstUnitMove : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint roleId { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public int pointIndex { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"points")]
        public global::System.Collections.Generic.List<Vector3> Points { get; } = new global::System.Collections.Generic.List<Vector3>();

    }

    [global::ProtoBuf.ProtoContract(Name = @"Bcst_UnitUpdatePosition")]
    public partial class BcstUnitUpdatePosition : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint roleId { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"point")]
        public Vector3 Point { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion