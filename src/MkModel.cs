
using System.IO;
using Markdig;

public class MkModel
{
    public string fileFullPath { get; private set; }
    public string fileName { get; private set; }
    public string convertHtml { get; private set; }

    /// <summary>
    /// markdown file model
    /// </summary>
    /// <param name="givenPath">mark down file path (relative/absolute)</param>
    public MkModel(string givenPath)
    {
        if (Path.IsPathRooted(givenPath))
        {
            fileFullPath = givenPath;
        }
        else
        {
            fileFullPath = Path.GetFullPath(givenPath);
        }
        fileName = Path.GetFileName(fileFullPath);
        convertHtml = ConvertToHtml(fileFullPath);
    }

    /// <summary>
    /// convert mark down to html
    /// </summary>
    /// <param name="path">mark down absolute path</param>
    /// <returns>html text</returns>
    public string ConvertToHtml(string path)
    {
        return Markdown.ToHtml(File.ReadAllText(path));
    }
}
