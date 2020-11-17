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
            var config = JsonConvert.DeserializeObject<Domain.Config>(System.IO.File.ReadAllText(configFile));
            var result = Plan.findPlan(config, 10000L);

            var resultJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.WriteLine(resultJson);
        }
    }
}
