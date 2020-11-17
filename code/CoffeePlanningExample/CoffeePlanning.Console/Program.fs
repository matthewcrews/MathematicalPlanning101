// Learn more about F# at http://fsharp.org

open System
open CoffeePlanning.Domain


let locations = 
    [
        Location "Sellwood"
        Location "Hawthorne"
        Location "The Pearl"
        Location "Eastmoreland"
        Location "St. Johns"
        Location "Alberta"
        Location "Nob Hill"
        Location "Belmont"
    ]

let roasterCost = 
    [
        Location "Sellwood",     150000.0
        Location "Hawthorne",    100000.0
        Location "The Pearl",    250000.0
        Location "Eastmoreland", 120000.0
        Location "St. Johns",    130000.0
        Location "Alberta",      110000.0
        Location "Nob Hill",     135000.0
        Location "Belmont",      180000.0
    ] |> dict
               
let roasterCapacity = 
    [
        Location "Sellwood",     12.0
        Location "Hawthorne",    18.0
        Location "The Pearl",    22.0
        Location "Eastmoreland", 13.0
        Location "St. Johns",    14.0
        Location "Alberta",      10.0
        Location "Nob Hill",     17.0
        Location "Belmont",      12.0
    ] |> dict
                   
let warehouseCost = 
    [
        Location "Sellwood",     80000.0
        Location "Hawthorne",    90000.0
        Location "The Pearl",    120000.0
        Location "Eastmoreland", 90000.0
        Location "St. Johns",    85000.0
        Location "Alberta",      70000.0
        Location "Nob Hill",     85000.0
        Location "Belmont",      90000.0
    ] |> dict
                 
let warehouseCapacity = 
    [
        Location "Sellwood",     8000.0
        Location "Hawthorne",    6000.0
        Location "The Pearl",    12000.0
        Location "Eastmoreland", 6000.0
        Location "St. Johns",    7000.0
        Location "Alberta",      9000.0
        Location "Nob Hill",     6000.0
        Location "Belmont",      9200.0
    ] |> dict
                 
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

    let plan = CoffeePlanning.Plan.findPlan config 10_000L
    
    printfn "Our Plan..."
    printfn "%A" plan
    
    0 // return an integer exit code
