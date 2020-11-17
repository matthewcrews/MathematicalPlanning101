namespace CoffeePlanning

open Flips
open Flips.Types
open Flips.SliceMap
open CoffeePlanning.Domain
open CoffeePlanning.Model


module Solve =
        
    /// A function for taking a solution and creating a Plan type to return
    let internal buildPlan 
        (warehouseDecisions: SMap<Location, Decision>)
        (roasterDecisions: SMap<Location, Decision>)
        (solution:Solution) =

        let selectedWarehouses =
            Solution.getValues solution warehouseDecisions
            |> Map.toSeq
            |> Seq.filter (fun (_, decisionValue) -> decisionValue >= 1.0)
            |> Seq.map (fun (Location location, _) -> location)
            |> Array.ofSeq

        let selectedRoasters =
            Solution.getValues solution roasterDecisions
            |> Map.toSeq
            |> Seq.filter (fun (_, decisionValue) -> decisionValue >= 1.0)
            |> Seq.map (fun (Location location, _) -> location)
            |> Array.ofSeq

        let totalCost = solution.ObjectiveResult

        {
            SelectedRoasters = selectedRoasters
            SelectedWarehouses = selectedWarehouses
            TotalCost = totalCost
        }

    /// The one function that is exposed for external use
    let findPlan 
        (config:Config)
        (maxDuration: int64) =

        let parameters = Parameters.ofConfig config

        let roasterDecisions =
            DecisionBuilder "BuildRoaster" {
                for location in parameters.Locations -> Boolean
            } |> SMap

        let warehouseDecisions =
            DecisionBuilder "BuildWarehouse" {
                for location in parameters.Locations -> Boolean
            } |> SMap

        let model = buildModel parameters warehouseDecisions roasterDecisions

        let settings = {
            SolverType = SolverType.CBC
            MaxDuration = maxDuration
            WriteLPFile = None
        }

        let result = Solver.solve settings model

        match result with
        | Optimal solution -> buildPlan warehouseDecisions roasterDecisions solution
        | _ -> failwith "Unable to solve model"


