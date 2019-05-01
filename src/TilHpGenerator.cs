using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using RazorLight;
using MicroBatchFramework;

public class TilHpGenerator
{
    /// <summary>
    /// Build
    /// </summary>
    public async Task Build(string inputDir, string outputDir, string tplDir)
    {
        var engine = new RazorLightEngineBuilder()
            .UseFilesystemProject(Directory.GetCurrentDirectory())
            .UseMemoryCachingProvider()
            .Build();
        IEnumerable<MkModel> mkModels = GetMkModelsFromDir(inputDir);
        foreach (var mkModel in mkModels)
        {
            string result = await engine.CompileRenderAsync(Path.Combine(tplDir, "index.cshtml"), new { Title = mkModel.fileName, ArticleText = mkModel.convertHtml });
            string htmlFilePath = mkModel.HtmlFilePath(outputDir);
            Console.WriteLine(htmlFilePath);
            if (File.Exists(htmlFilePath))
            {
                File.WriteAllText(htmlFilePath, result);
            }
            else
            {
                using (FileStream fs = File.Create(htmlFilePath))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(result);
                }
            }
        }
    }

    private IEnumerable<MkModel> GetMkModelsFromDir(string dirPath)
    {
        return Directory.EnumerateFiles(dirPath, "*.md", SearchOption.AllDirectories)
            .Select((filePath, index) => new MkModel(filePath, dirPath));
    }
}