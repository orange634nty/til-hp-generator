using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using RazorLight;
using MicroBatchFramework;

public class TilHpGenerateBatch : BatchBase
{
    /// <summary>
    /// build task
    /// </summary>
    /// <param name="inputDir">option -i input dir</param>
    /// <param name="outputDir">option -o output dir</param>
    /// <param name="tplDir">option -t template dir</param>
    public async Task Build(
        [Option("-i", "input dir")]string inputDir,
        [Option("-o", "output dir")]string outputDir,
        [Option("-t", "template dir")]string tplDir
    )
    {
        var thGenerator = new TilHpGenerator();
        await thGenerator.Build(inputDir, outputDir, tplDir);
    }
}
