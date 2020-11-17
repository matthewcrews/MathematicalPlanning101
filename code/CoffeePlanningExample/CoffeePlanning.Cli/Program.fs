open System
open Newtonsoft.Json
open CoffeePlanning

[<EntryPoint>]
let main argv =
    Console.Title <- "C# Coffee Planning Example"
    printfn "C# Coffee Planning Example"

    let configFile = "PlanConfig.json"
    let config = JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText configFile)
    let result = Solve.findPlan config 10_000L

    let resultJson = JsonConvert.SerializeObject(result, Formatting.Indented)
    printfn "================Result================"
    printfn "%s" resultJson

    0 // return an integer exit code
