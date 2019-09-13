using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace AspNetCoreProtobuf.Formatters
{
    public class ProtobufOutputFormatter :  OutputFormatter
    {
        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public string ContentType { get; private set; }

        public static RuntimeTypeModel Model
        {
            get { return model.Value; }
        }

        public ProtobufOutputFormatter()
        {
            ContentType = "application/x-protobuf";
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));

            //SupportedEncodings.Add(Encoding.GetEncoding("utf-8"));
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = RuntimeTypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            //MemoryStream stream = new MemoryStream();
            //ProtoBuf.Serializer.Serialize<Table>(stream, new Table
            //{
            //    Name = "jim",
            //    Dimensions = "190x80x90",
            //    Description = "top of the range from Migro"
            //});

            //stream.Position = 0;
            //var sr = new StreamReader(stream);
            //var myStr = sr.ReadToEnd();

            Model.Serialize(response.Body, context.Object);
            return Task.FromResult(response);
        }
    }
}
