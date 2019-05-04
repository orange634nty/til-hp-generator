using System;
using System.IO;
using System.Threading.Tasks;
using Markdig;
using RazorLight;

public class MkModel
{
    public string fileFullPath { get; private set; }
    public string relativePath { get; private set; }
    public string fileName { get; private set; }
    public string convertHtml { get; private set; }

    /// <summary>
    /// markdown file model
    /// </summary>
    /// <param name="_fileFullPath">mark down file full path</param>
    /// <param name="basePath">base path</param>
    public MkModel(string _fileFullPath, string basePath)
    {
        fileFullPath = _fileFullPath;
        relativePath = GetRelativePath(fileFullPath, basePath);
        fileName = Path.GetFileNameWithoutExtension(fileFullPath);
        convertHtml = ConvertToHtml(fileFullPath);
    }

    /// <summary>
    /// create or over write html file
    /// </summary>
    /// <param name="engine">RazorLightEngin Instance</param>
    /// <param name="cshtmlPath">cshtml template file</param>
    /// <param name="outputDir">out put directory</param>
    public async Task CreateOrOverWrite(RazorLightEngine engine, string cshtmlPath, string outputDir)
    {
        string result = await engine.CompileRenderAsync(cshtmlPath, new { title = fileName, articleText = convertHtml });
        string htmlFilePath = HtmlFilePath(outputDir);
        Console.WriteLine($"[Generate] : {htmlFilePath}");

        // create directory if not exist
        string htmlFileDir = Path.GetDirectoryName(htmlFilePath);
        if (!Directory.Exists(htmlFileDir))
        {
            Directory.CreateDirectory(htmlFileDir);
        }

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
    /// get relative html file
    /// </summary>
    /// <returns> relative html path</returns>
    public string RelativeHtmlFilePath()
    {
        string htmlFileName = $"{fileName}.html";
        string htmlFileDir = Path.GetDirectoryName(relativePath);
        return Path.Combine(htmlFileDir, htmlFileName);
    }

    /// <summary>
    /// get relative path
    /// </summary>
    /// <param name="filePath">file path</param>
    /// <param name="basePath">base path</param>
    /// <returns>relative path</returns>
    private string GetRelativePath(string filePath, string basePath)
    {
        Uri filePathUri = new Uri(filePath);
        Uri basePathUri = new Uri(basePath);
        return basePathUri.MakeRelativeUri(filePathUri).ToString();
    }

    /// <summary>
    /// get html path
    /// </summary>
    /// <param name="outDir">out put dir</param>
    /// <returns>html file path</returns>
    private string HtmlFilePath(string outDir)
    {
        return Path.Combine(outDir, RelativeHtmlFilePath());
    }

    /// <summary>
    /// convert mark down to html
    /// </summary>
    /// <param name="path">mark down absolute path</param>
    /// <returns>html text</returns>
    private string ConvertToHtml(string path)
    {
        return Markdown.ToHtml(File.ReadAllText(path));
    }
}
