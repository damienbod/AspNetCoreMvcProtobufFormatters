using ProtoBuf;

namespace AspNetCoreProtobuf.Model
{
    [ProtoContract]
    public class ProtobufModelDto
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string StringValue { get; set; }
        [ProtoMember(4)]
        public int IntValue { get; set; }

    }
}
