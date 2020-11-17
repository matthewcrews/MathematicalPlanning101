namespace CoffeePlanning

open Flips
open Flips.Types
open Flips.SliceMap
open CoffeePlanning.Domain

module internal Model =

    /// Create the base Model with just the Objective function
    let createBase 
        (p: Parameters)
        (roasterDecision:SMap<Location, Decision>)
        (warehouseDecisions: SMap<Location, Decision>) =
        
        let costExpression = sum (p.RoasterCosts .* roasterDecision) + sum (p.WarehouseCosts .* warehouseDecisions)
        let objective = Objective.create "MinimizeCost" Minimize costExpression
        Model.create objective

    /// Add constraint for Minimum Warehouse Capacity
    let addMinWarehouseCapacityConstraint 
        (p: Parameters)
        (warehouseDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseConstraint =
            Constraint.create "MinWarehouseCapacity" (sum (p.WarehouseCapacity .* warehouseDecisions) >== p.MinWarehouseCapacity)

        Model.addConstraint warehouseConstraint model

    /// Add constraint for Minimum Roaster Capacity
    let addMinRoasterCapacityConstraint 
        (p: Parameters)
        (roasterDecisions: SMap<Location, Decision>)
        (model: Model.Model) =

        let warehouseConstraint =
            Constraint.create "MinWarehouseCapacity" (sum (p.RoasterCapacity .* roasterDecisions) >== p.MinRoasterCapacity)

        Model.addConstraint warehouseConstraint model

    /// Add Warehouse required wherever there is a Roaster constraints
    let addWarehouseRequiredConstraints 
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

    /// Build the full model
    let buildModel (p: Parameters) (warehouseDecisions: SMap<Location, Decision>) (roasterDecisions: SMap<Location, Decision>) =

        createBase p roasterDecisions warehouseDecisions
        |> addMinRoasterCapacityConstraint p roasterDecisions
        |> addMinWarehouseCapacityConstraint p warehouseDecisions
        |> addWarehouseRequiredConstraints p roasterDecisions warehouseDecisions

