namespace CoffeePlanning

open Flips
open Flips.Types
open Flips.SliceMap
open CoffeePlanning.Domain

module Model =

    let internal createBase 
        (config:Config)
        (roasterDecision:SMap<Location, Decision>)
        (warehouseDecisions: SMap<Location, Decision>) =
        
        let costExpression = sum (config.RoasterCosts .* roasterDecision) + sum (config.WarehouseCosts .* warehouseDecisions)
        let objective = Objective.create "MinimizeCost" Minimize costExpression
        Model.create objective

    let internal addMinWarehouseCapacityConstraint 
        (config:Config)
        (warehouseDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseConstraint =
            Constraint.create "MinWarehouseCapacity" (sum (config.WarehouseCapacity .* warehouseDecisions) >== config.MinWarehouseCapacity)

        Model.addConstraint warehouseConstraint model

    let internal addMinRoasterCapacityConstraint 
        (config:Config)
        (roasterDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseConstraint =
            Constraint.create "MinWarehouseCapacity" (sum (config.RoasterCapacity .* roasterDecisions) >== config.MinRoasterCapacity)

        Model.addConstraint warehouseConstraint model

    let internal addWarehouseRequiredConstraints 
        (config:Config)
        (warehouseDecisions: SMap<Location, Decision>)
        (roasterDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseRequiredConstraints =
            ConstraintBuilder "WarehouseAndRoasterCoexist" { 
                for location in config.Locations ->
                roasterDecisions.[location] <== warehouseDecisions.[location]
            }

        Model.addConstraints warehouseRequiredConstraints model

    let internal buildModel (config:Config) (warehouseDecisions: SMap<Location, Decision>) (roasterDecisions: SMap<Location, Decision>) =

        createBase config roasterDecisions warehouseDecisions
        |> addMinRoasterCapacityConstraint config roasterDecisions
        |> addMinWarehouseCapacityConstraint config warehouseDecisions
        |> addWarehouseRequiredConstraints config roasterDecisions warehouseDecisions

