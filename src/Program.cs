using System;
using System.Threading.Tasks;
using MicroBatchFramework;

public class Program
{
    /// <summary>
    /// Entry Point
    /// </summary>
    /// <param name="args">Command Line Args</param>
    static async Task Main(string[] args)
    {
        await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<TilHpGenerator>(args);
    }
}
