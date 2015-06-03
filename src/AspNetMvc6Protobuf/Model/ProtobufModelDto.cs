using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProtoBuf;

namespace AspNetMvc6Protobuf.Model
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
