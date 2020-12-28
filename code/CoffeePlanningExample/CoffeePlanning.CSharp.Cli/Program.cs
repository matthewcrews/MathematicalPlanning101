using System;
using Newtonsoft.Json;
using CoffeePlanning;

namespace CoffeePlanning.CSharp.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "C# Coffee Planning Example";
            Console.WriteLine("C# Coffee Planning Example");

            var configFile = "PlanConfig.json";
            var config = JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(configFile));
            var result = Solve.findPlan(config, 10000L);

            var resultJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.WriteLine("================Result================");
            Console.WriteLine(resultJson);
            Console.WriteLine("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
