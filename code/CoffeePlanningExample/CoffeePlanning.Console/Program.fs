// Learn more about F# at http://fsharp.org

open System
open CoffeePlanning
open System.Collections.Generic


let locations = 
    [|
        "Sellwood"
        "Hawthorne"
        "The Pearl"
        "Eastmoreland"
        "St. Johns"
        "Alberta"
        "Nob Hill"
        "Belmont"
    |]

let roasterCost = 
    [
        "Sellwood",     150000.0
        "Hawthorne",    100000.0
        "The Pearl",    250000.0
        "Eastmoreland", 120000.0
        "St. Johns",    130000.0
        "Alberta",      110000.0
        "Nob Hill",     135000.0
        "Belmont",      180000.0
    ]
    |> List.map KeyValuePair
    |> Dictionary
               
let roasterCapacity = 
    [
        "Sellwood",     12.0
        "Hawthorne",    18.0
        "The Pearl",    22.0
        "Eastmoreland", 13.0
        "St. Johns",    14.0
        "Alberta",      10.0
        "Nob Hill",     17.0
        "Belmont",      12.0
    ]
    |> List.map KeyValuePair
    |> Dictionary
                   
let warehouseCost = 
    [
        "Sellwood",     80000.0
        "Hawthorne",    90000.0
        "The Pearl",    120000.0
        "Eastmoreland", 90000.0
        "St. Johns",    85000.0
        "Alberta",      70000.0
        "Nob Hill",     85000.0
        "Belmont",      90000.0
    ] 
    |> List.map KeyValuePair
    |> Dictionary
                 
let warehouseCapacity = 
    [
        "Sellwood",     8000.0
        "Hawthorne",    6000.0
        "The Pearl",    12000.0
        "Eastmoreland", 6000.0
        "St. Johns",    7000.0
        "Alberta",      9000.0
        "Nob Hill",     6000.0
        "Belmont",      9200.0
    ]
    |> List.map KeyValuePair
    |> Dictionary
                 
let minWarehouseCapacity = 30000.0
let minRoastingCapacity = 30.0

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let config = {
        Locations = locations
        WarehouseCapacity = warehouseCapacity
        WarehouseCosts = warehouseCost
        RoasterCapacity = roasterCapacity
        RoasterCosts = roasterCost
        MinWarehouseCapacity = minWarehouseCapacity
        MinRoasterCapacity = minRoastingCapacity
    }

    let plan = CoffeePlanning.Solve.findPlan config 10_000L
    
    printfn "Our Plan..."
    printfn "%A" plan

    printfn "Press any key to close..."
    let _ = Console.ReadKey()
    0 // return an integer exit code
