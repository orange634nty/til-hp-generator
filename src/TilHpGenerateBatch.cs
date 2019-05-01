using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using RazorLight;
using MicroBatchFramework;

public class TilHpGenerateBatch : BatchBase
{
    public async Task Build(
        [Option("-i", "input dir")]string inputDir,
        [Option("-o", "output dir")]string outputDir,
        [Option("-t", "template dir")]string tplDir
    )
    {
        var thGenerator = new TilHpGenerator();
        await thGenerator.Build(inputDir, outputDir, tplDir);
        // var mkModel = new MkModel("./sampleMk/blog.mk");
        // var engine = new RazorLightEngineBuilder()
        //     .UseFilesystemProject(Directory.GetCurrentDirectory())
        //     .UseMemoryCachingProvider()
        //     .Build();
        // string result = await engine.CompileRenderAsync("tpl/index.cshtml", new { Title = "test title", ArticleText = mkModel.convertHtml });
        // Console.WriteLine(result);
    }
}
