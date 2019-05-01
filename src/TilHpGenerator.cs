using System;
using System.IO;
using System.Threading.Tasks;
using Markdig;
using RazorLight;
using MicroBatchFramework;

public class TilHpGenerator : BatchBase
{
    /// <summary>
    /// Build
    /// </summary>
    public async Task Build()
    {
        var mkModel = new MkModel("./sampleMk/blog.mk");
        var engine = new RazorLightEngineBuilder()
            .UseFilesystemProject(Directory.GetCurrentDirectory())
            .UseMemoryCachingProvider()
            .Build();
        string result = await engine.CompileRenderAsync("tpl/index.cshtml", new { Title = "test title", ArticleText = mkModel.convertHtml });
        Console.WriteLine(result);
    }
}
