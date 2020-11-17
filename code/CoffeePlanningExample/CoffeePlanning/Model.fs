namespace CoffeePlanning

open Flips
open Flips.Types
open Flips.SliceMap
open CoffeePlanning.Domain

module Model =

    let internal createBase 
        (p: Parameters)
        (roasterDecision:SMap<Location, Decision>)
        (warehouseDecisions: SMap<Location, Decision>) =
        
        let costExpression = sum (p.RoasterCosts .* roasterDecision) + sum (p.WarehouseCosts .* warehouseDecisions)
        let objective = Objective.create "MinimizeCost" Minimize costExpression
        Model.create objective

    let internal addMinWarehouseCapacityConstraint 
        (p: Parameters)
        (warehouseDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseConstraint =
            Constraint.create "MinWarehouseCapacity" (sum (p.WarehouseCapacity .* warehouseDecisions) >== p.MinWarehouseCapacity)

        Model.addConstraint warehouseConstraint model

    let internal addMinRoasterCapacityConstraint 
        (p: Parameters)
        (roasterDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseConstraint =
            Constraint.create "MinWarehouseCapacity" (sum (p.RoasterCapacity .* roasterDecisions) >== p.MinRoasterCapacity)

        Model.addConstraint warehouseConstraint model

    let internal addWarehouseRequiredConstraints 
        (p: Parameters)
        (warehouseDecisions: SMap<Location, Decision>)
        (roasterDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseRequiredConstraints =
            ConstraintBuilder "WarehouseAndRoasterCoexist" { 
                for location in p.Locations ->
                roasterDecisions.[location] <== warehouseDecisions.[location]
            }

        Model.addConstraints warehouseRequiredConstraints model

    let internal buildModel (p: Parameters) (warehouseDecisions: SMap<Location, Decision>) (roasterDecisions: SMap<Location, Decision>) =

        createBase p roasterDecisions warehouseDecisions
        |> addMinRoasterCapacityConstraint p roasterDecisions
        |> addMinWarehouseCapacityConstraint p warehouseDecisions
        |> addWarehouseRequiredConstraints p roasterDecisions warehouseDecisions

