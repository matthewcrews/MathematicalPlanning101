namespace CoffeePlanning

open Flips.SliceMap
open System.Collections.Generic

module Domain =
    
    type Location = Location of string

    type Plan = {
        SelectedWarehouses : array<Location>
        SelectedRoasters : array<Location>
        TotalCost : float
    }

    type Config = {
        Locations : array<Location>
        WarehouseCosts : Dictionary<Location, float>
        WarehouseCapacity : Dictionary<Location, float>
        RoasterCosts : Dictionary<Location, float>
        RoasterCapacity : Dictionary<Location, float>
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
                Locations = c.Locations |> List.ofArray
                WarehouseCosts = c.WarehouseCosts :> seq<_> |> Seq.map (|KeyValue|) |> SMap
                WarehouseCapacity = c.WarehouseCapacity :> seq<_> |> Seq.map (|KeyValue|) |> SMap
                RoasterCosts = c.RoasterCosts :> seq<_> |> Seq.map (|KeyValue|) |> SMap
                RoasterCapacity = c.RoasterCapacity :> seq<_> |> Seq.map (|KeyValue|) |> SMap
                MinWarehouseCapacity = c.MinWarehouseCapacity
                MinRoasterCapacity = c.MinRoasterCapacity
            }
