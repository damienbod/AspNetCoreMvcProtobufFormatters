using ProtoBuf;

namespace WebAPiProtobuf.Models
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
        
	}
}