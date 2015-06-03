using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.Framework.Internal;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;


namespace AspNetMvc6Protobuf.Formatters
{
    public class ProtobufInputFormatter : InputFormatter
    {
        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public static RuntimeTypeModel Model
        {
            get { return model.Value; }
        }

        public override Task<object> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.ActionContext.HttpContext.Request;
            MediaTypeHeaderValue requestContentType = null;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);


            object result = Model.Deserialize(context.ActionContext.HttpContext.Request.Body, null, type);

            return Task.FromResult(result);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }


        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }
    }
}
