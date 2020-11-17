namespace CoffeePlanning.Tests

open System
open Xunit
open Flips
open Flips.Types
open Flips.SliceMap
open CoffeePlanning
open CoffeePlanning.Domain

module Tests =

    let internal locations = 
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

    let internal roasterCost = 
        [
            Location "Sellwood",     150000.0
            Location "Hawthorne",    100000.0
            Location "The Pearl",    250000.0
            Location "Eastmoreland", 120000.0
            Location "St. Johns",    130000.0
            Location "Alberta",      110000.0
            Location "Nob Hill",     135000.0
            Location "Belmont",      180000.0
        ] |> SMap
                   
    let internal roasterCapacity = 
        [
            Location "Sellwood",     12.0
            Location "Hawthorne",    18.0
            Location "The Pearl",    22.0
            Location "Eastmoreland", 13.0
            Location "St. Johns",    14.0
            Location "Alberta",      10.0
            Location "Nob Hill",     17.0
            Location "Belmont",      12.0
        ] |> SMap
                       
    let internal warehouseCost = 
        [
            Location "Sellwood",     80000.0
            Location "Hawthorne",    90000.0
            Location "The Pearl",    120000.0
            Location "Eastmoreland", 90000.0
            Location "St. Johns",    85000.0
            Location "Alberta",      70000.0
            Location "Nob Hill",     85000.0
            Location "Belmont",      90000.0
        ] |> SMap
                     
    let internal warehouseCapacity = 
        [
            Location "Sellwood",     8000.0
            Location "Hawthorne",    6000.0
            Location "The Pearl",    12000.0
            Location "Eastmoreland", 6000.0
            Location "St. Johns",    7000.0
            Location "Alberta",      9000.0
            Location "Nob Hill",     6000.0
            Location "Belmont",      9200.0
        ] |> SMap

    let internal parameters : Parameters = {
        Locations = locations
        WarehouseCapacity = warehouseCapacity
        WarehouseCosts = warehouseCost
        RoasterCapacity = roasterCapacity
        RoasterCosts = roasterCost
        MinRoasterCapacity = 30.0
        MinWarehouseCapacity = 30_000.0
    }

    let internal roasterDecisions =
        DecisionBuilder "BuildRoaster" {
            for location in parameters.Locations -> Boolean
        } |> SMap

    let internal warehouseDecisions =
        DecisionBuilder "BuildWarehouse" {
            for location in parameters.Locations -> Boolean
        } |> SMap

    let internal solverSettings = {
        SolverType = SolverType.CBC
        MaxDuration = 10_000L
        WriteLPFile = None
    }

    [<Fact>]
    let ``Base Model selects no locations`` () =
        
        // Arrange
        let baseModel = Model.createBase parameters roasterDecisions warehouseDecisions

        // Act
        let result = Solver.solve solverSettings baseModel

        // Assert
        match result with
        | Optimal solution ->
            for (decision, value) in Map.toSeq solution.DecisionResults do
                Assert.Equal(value, 0.0)
        | _ -> Assert.True(false, "Model failed to solve")

    [<Fact>]
    let ``MinWarehouseCapacity constraint ensures enough capacity`` () =
        
        // Arrange
        let model = 
            Model.createBase parameters roasterDecisions warehouseDecisions
            |> Model.addMinWarehouseCapacityConstraint parameters warehouseDecisions

        // Act
        let result = Solver.solve solverSettings model

        // Assert
        match result with
        | Optimal solution ->
            let totalWarehouseCapacity = 
                Solution.getValues solution warehouseDecisions
                |> Map.toSeq
                |> Seq.sumBy (fun (location, value) -> warehouseCapacity.[location] * value)

            Assert.True(totalWarehouseCapacity >= parameters.MinWarehouseCapacity, "Planned Warehouse Capacity insufficient")

        | _ -> Assert.True(false, "Model failed to solve")

    [<Fact>]
    let ``MinRoastingCapacity constraint ensures enough capacity`` () =
        
        // Arrange
        let model = 
            Model.createBase parameters roasterDecisions warehouseDecisions
            |> Model.addMinRoasterCapacityConstraint parameters roasterDecisions

        // Act
        let result = Solver.solve solverSettings model

        // Assert
        match result with
        | Optimal solution ->
            let totalRoastingCapacity = 
                Solution.getValues solution roasterDecisions
                |> Map.toSeq
                |> Seq.sumBy (fun (location, value) -> warehouseCapacity.[location] * value)

            Assert.True(totalRoastingCapacity >= parameters.MinRoasterCapacity, "Planned Roasting Capacity insufficient")

        | _ -> Assert.True(false, "Model failed to solve")

    [<Fact>]
    let ``Warehouse planned where there is a Roaster`` () =
        
        // Arrange
        let model = 
            Model.createBase parameters roasterDecisions warehouseDecisions
            |> Model.addMinRoasterCapacityConstraint parameters roasterDecisions
            |> Model.addWarehouseRequiredConstraints parameters warehouseDecisions roasterDecisions

        // Act
        let result = Solver.solve solverSettings model

        // Assert
        match result with
        | Optimal solution ->
            let roasterLocations = 
                Solution.getValues solution roasterDecisions
                |> Map.toSeq
                |> Seq.filter (fun (location, value) -> value >= 1.0)
                |> Seq.map fst

            let warehouseLocations =
                Solution.getValues solution warehouseDecisions
                |> Map.toSeq
                |> Seq.filter (fun (location, value) -> value >= 1.0)
                |> Seq.map fst
                |> Set.ofSeq

            for roasterLocation in roasterLocations do
                Assert.True(Set.contains roasterLocation warehouseLocations, "Roaster planned without Warehouse")
            
        | _ -> Assert.True(false, "Model failed to solve")
