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
    /// necessary file extension to generate HP
    /// </summary>
    /// <value>extension array</value>
    private static readonly string[] necessaryFileExtension = {".jpg", "jpeg", ".png", "gif"};

    /// <summary>
    /// build html pages
    /// </summary>
    /// <param name="inputDir">directory which markdown file exist</param>
    /// <param name="outputDir">directory which html file generate</param>
    /// <param name="tplDir" >directory which cshtml file exist</param>
    public async Task Build(string inputDir, string outputDir, string tplDir)
    {
        // generate html file from markdown
        var engine = new RazorLightEngineBuilder()
            .UseFilesystemProject(Directory.GetCurrentDirectory())
            .UseMemoryCachingProvider()
            .Build();
        IEnumerable<MkModel> mkModels = GetMkModelsFromDir(inputDir);
        foreach (var mkModel in mkModels)
        {
            await mkModel.CreateOrOverWrite(engine, Path.Combine(tplDir, "article.cshtml"), outputDir);
        }

        await GenerateTopPageHtml(engine, mkModels, Path.Combine(tplDir, "index.cshtml"), outputDir);


        // copy images
        IEnumerable<string> imgFilePaths = GetImageFileFromDir(inputDir);
        foreach (var imgFilePath in imgFilePaths)
        {
            Console.WriteLine($"[Copy] : {imgFilePath}");
            // create img copy directory
            string imgDir = Path.GetDirectoryName(Path.Combine(outputDir, imgFilePath));
            if (!Directory.Exists(imgDir))
            {
                Directory.CreateDirectory(imgDir);
            }

            // copy file
            File.Copy(Path.Combine(inputDir, imgFilePath), Path.Combine(outputDir, imgFilePath), true);
        }
    }

    /// <summary>
    /// generate top page of html
    /// </summary>
    /// <param name="engine"></param>
    /// <param name="mkModels"></param>
    /// <param name="cshtmlPath"></param>
    /// <param name="outputDir"></param>
    private async Task GenerateTopPageHtml(RazorLightEngine engine, IEnumerable<MkModel> mkModels, string cshtmlPath, string outputDir)
    {
        // generate HP top page
        string result = await engine.CompileRenderAsync(cshtmlPath, new { mkModels = mkModels });
        string htmlFilePath = Path.Combine(outputDir, "index.html");
        Console.WriteLine($"[Generate] : {htmlFilePath}");

        // over write or create file
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

    /// <summary>
    /// get all necessary file from directory
    /// Where : if file extension is necessary to create HP
    /// Select : convert to relative path
    /// </summary>
    /// <param name="dirPath">target directory full path</param>
    /// <returns></returns>
    private IEnumerable<string> GetImageFileFromDir(string dirPath)
    {
        return Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories)
            .Where((filePath, index) => necessaryFileExtension.Contains(Path.GetExtension(filePath)))
            .Select((filePath, index) => {
                Uri filePathUri = new Uri(filePath);
                Uri basePathUri = new Uri(dirPath);
                return basePathUri.MakeRelativeUri(filePathUri).ToString();
            });
    }

    /// <summary>
    /// get all markdow file from target directory
    /// </summary>
    /// <param name="dirPath">taerget directory</param>
    private IEnumerable<MkModel> GetMkModelsFromDir(string dirPath)
    {
        return Directory.EnumerateFiles(dirPath, "*.md", SearchOption.AllDirectories)
            .Where((filePath, index) => !filePath.Contains("README"))
            .Select((filePath, index) => new MkModel(filePath, dirPath));
    }
}
