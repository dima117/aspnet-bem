using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.NodeServices;

namespace EmptyWeb
{
    public class BemhtmlMiddleware
    {
        public const string BEMJSON_CONTENT_TYPE = "application/bemjson";
        public const string HTML_CONTENT_TYPE = "text/html;charset=UTF-8";

        private readonly RequestDelegate next;
        private readonly INodeServices node;

        public BemhtmlMiddleware(RequestDelegate next, INodeServices node)
        {
            this.next = next;
            this.node = node;
        }

        public async Task Invoke(HttpContext context)
        {
            var existingBody = context.Response.Body;

            using (var newBody = new MemoryStream())
            {
                context.Response.Body = newBody;
                await next.Invoke(context);

                if (context.Response.ContentType == BEMJSON_CONTENT_TYPE)
                {
                    context.Response.Body = existingBody;
                    newBody.Seek(0, SeekOrigin.Begin);
                    await ApplyTemplates(context, newBody);
                }
            }
        }

        private async Task ApplyTemplates(HttpContext context, Stream newBody)
        {
            var bemjson = await ReadStream(newBody);
            var html = await node.InvokeAsync<string>("./index", bemjson);

            context.Response.ContentType = HTML_CONTENT_TYPE;
            await context.Response.WriteAsync(html);
        }

        private async Task<string> ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}