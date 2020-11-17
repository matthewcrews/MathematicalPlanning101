namespace CoffeePlanning

open Flips.SliceMap

module Domain =
    
    type Location = Location of string

    type Config = {
        Locations : List<Location>
        WarehouseCosts : SMap<Location, float>
        WarehouseCapacity : SMap<Location, float>
        RoasterCosts : SMap<Location, float>
        RoasterCapacity : SMap<Location, float>
        MinWarehouseCapacity : float
        MinRoasterCapacity : float
    }

    type Plan = {
        SelectedWarehouses : array<Location>
        SelectedRoasters : array<Location>
        TotalCost : float
    }
