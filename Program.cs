using System;
using RazorLight;
using System.Threading.Tasks;
using Markdig;

namespace til_hp_generator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string mkText = "# Discription\nI'm trying to make add using *RazorLight* *Markdig* *MicroBathFramework* for til.";
            var mk = Markdown.ToHtml(mkText);
            string template = "Hello this is my new article\n@Raw(@Model.mk)\nAnd that all! thank you!";
            var engine = new RazorLightEngineBuilder()
              .UseMemoryCachingProvider()
              .Build();
            string result = await engine.CompileRenderAsync("templateKey", template, new { mk = mk });
            Console.WriteLine(result);
        }
    }
}
