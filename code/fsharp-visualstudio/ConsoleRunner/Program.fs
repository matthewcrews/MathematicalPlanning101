// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Practice


[<EntryPoint>]
let main argv =


    FoodTruck.solve ()
    //FarmModel.solve ()
    //RevisedFarmModel.solve ()
    //CoffeeModel.solve ()
    //PowerPlanning.solve ()

    printfn "Hit Enter to exit..."
    let r = Console.ReadLine()
    0 // return an integer exit code