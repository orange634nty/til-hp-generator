using System;
using RazorLight;
using System.Threading.Tasks;

namespace til_hp_generator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string template = "Hello @Model.Name, welcome to RazorEngine!";
            var engine = new RazorLightEngineBuilder()
              .UseMemoryCachingProvider()
              .Build();
            string result = await engine.CompileRenderAsync("templateKey", template, new { Name = "World" });
            Console.WriteLine(result);
        }
    }
}
