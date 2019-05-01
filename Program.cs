using System;
using System.IO;
using RazorLight;
using System.Threading.Tasks;
using Markdig;
using MicroBatchFramework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace til_hp_generator
{
    /// <summary>
    /// Generate Hp from Til repo
    /// </summary>
    class TilHpGenerator : BatchBase
    {
        /// <summary>
        /// Build
        /// </summary>
        public async Task Build()
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

    /// <summary>
    /// Entry Point Class
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <params name="args">Command Line args</params>
        static async Task Main(string[] args)
        {
            await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<TilHpGenerator>(args);
        }
    }
}
