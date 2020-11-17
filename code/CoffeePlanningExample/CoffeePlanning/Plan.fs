namespace CoffeePlanning

open Flips
open Flips.Types
open Flips.SliceMap
open CoffeePlanning.Domain
open CoffeePlanning.Model


module Plan =
        
        let internal buildResult 
            (warehouseDecisions: SMap<Location, Decision>)
            (roasterDecisions: SMap<Location, Decision>)
            (solution:Solution) =

            let selectedWarehouses =
                Solution.getValues solution warehouseDecisions
                |> Seq.filter (fun kvp -> kvp.Value >= 1.0)
                |> Seq.map (fun kvp -> kvp.Key)
                |> Array.ofSeq

            let selectedRoasters =
                Solution.getValues solution roasterDecisions
                |> Seq.filter (fun kvp -> kvp.Value >= 1.0)
                |> Seq.map (fun kvp -> kvp.Key)
                |> Array.ofSeq

            let totalCost = solution.ObjectiveResult

            {
                SelectedRoasters = selectedRoasters
                SelectedWarehouses = selectedWarehouses
                TotalCost = totalCost
            }

        let findPlan 
            (config:Config)
            (maxDuration: int64) =

            let roasterDecisions =
                DecisionBuilder "BuildRoaster" {
                    for location in config.Locations -> Boolean
                } |> SMap

            let warehouseDecisions =
                DecisionBuilder "BuildWarehouse" {
                    for location in config.Locations -> Boolean
                } |> SMap

            let model = buildModel config warehouseDecisions roasterDecisions

            let settings = {
                SolverType = SolverType.CBC
                MaxDuration = maxDuration
                WriteLPFile = None
            }

            let result = Solver.solve settings model

            match result with
            | Optimal solution -> buildResult warehouseDecisions roasterDecisions solution
            | _ -> failwith "Unable to solve model"


