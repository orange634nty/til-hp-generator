using System;
using System.IO;
using Markdig;

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
        fileName = Path.GetFileName(fileFullPath);
        convertHtml = ConvertToHtml(fileFullPath);
    }

    public string HtmlFilePath(string outDir)
    {
        string htmlFileName = $"{Path.GetFileNameWithoutExtension(fileFullPath)}.html";
        string htmlFileDir = Path.GetDirectoryName(relativePath);
        return Path.Combine(outDir, Path.Combine(htmlFileDir, htmlFileName));
    }

    private string GetRelativePath(string filePath, string basePath)
    {
        Uri filePathUri = new Uri(filePath);
        Uri basePathUri = new Uri(basePath);
        return basePathUri.MakeRelativeUri(filePathUri).ToString();

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
