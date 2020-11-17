namespace CoffeePlanning

open Flips.SliceMap
open System.Collections.Generic

module Domain =
    
    type Location = Location of string

    type Plan = {
        SelectedWarehouses : array<string>
        SelectedRoasters : array<string>
        TotalCost : float
    }

    // A type for external use. Typically easily maps to JSON
    type Config = {
        Locations : array<string>
        WarehouseCosts : Dictionary<string, float>
        WarehouseCapacity : Dictionary<string, float>
        RoasterCosts : Dictionary<string, float>
        RoasterCapacity : Dictionary<string, float>
        MinWarehouseCapacity : float
        MinRoasterCapacity : float
    }

    type internal Parameters = {
        Locations : Location list
        WarehouseCosts : SMap<Location, float>
        WarehouseCapacity : SMap<Location, float>
        RoasterCosts : SMap<Location, float>
        RoasterCapacity : SMap<Location, float>
        MinWarehouseCapacity : float
        MinRoasterCapacity : float
    }

    module internal Parameters = 

        let ofConfig (c:Config) =
            {
                Locations = c.Locations |> Seq.map Location |> List.ofSeq
                WarehouseCosts = c.WarehouseCosts :> seq<_> |> Seq.map (|KeyValue|) |> Seq.map (fun (k, v) -> Location k, v) |> SMap
                WarehouseCapacity = c.WarehouseCapacity :> seq<_> |> Seq.map (|KeyValue|) |> Seq.map (fun (k, v) -> Location k, v) |> SMap
                RoasterCosts = c.RoasterCosts :> seq<_> |> Seq.map (|KeyValue|) |> Seq.map (fun (k, v) -> Location k, v) |> SMap
                RoasterCapacity = c.RoasterCapacity :> seq<_> |> Seq.map (|KeyValue|) |> Seq.map (fun (k, v) -> Location k, v) |> SMap
                MinWarehouseCapacity = c.MinWarehouseCapacity
                MinRoasterCapacity = c.MinRoasterCapacity
            }
