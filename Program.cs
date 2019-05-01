using System;
using System.IO;
using RazorLight;
using System.Threading.Tasks;
using Markdig;

namespace til_hp_generator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string mkText = File.ReadAllText(Directory.GetCurrentDirectory() + "/sampleMk/blog.mk");
            var articleText = Markdown.ToHtml(mkText);
            var engine = new RazorLightEngineBuilder()
              .UseFilesystemProject(Directory.GetCurrentDirectory())
              .UseMemoryCachingProvider()
              .Build();
            string result = await engine.CompileRenderAsync("tpl/index.cshtml", new { Title = "test title", ArticleText = articleText });
            Console.WriteLine(result);
        }
    }
}
