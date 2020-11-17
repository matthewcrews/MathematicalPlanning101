module Solutions.CoffeeModelRevised

open Flips
open Flips.Types
open Flips.SliceMap

type Location = Location of string

let solve () =

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
        ] |> SMap
                   
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
        ] |> SMap
                       
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
        ] |> SMap
                     
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
        ] |> SMap
                     
    let minWarehouseCapacity = 30000.0
    let minRoastingCapacity = 30.0

    // Create variables to indicate whether to build a Roaster at a given location
    let roasterDecs = 
        DecisionBuilder "BuildRoaster" {
            for location in locations -> Boolean 
        } |> SMap

    // Create variables to indicate whether or not to build a Warehouse at a given location
    let warehouseDecs =
        DecisionBuilder "BuildWarehouse" {
            for location in locations -> Boolean
        } |> SMap

    // Create an expression for the cost of Warehouses
    let warehouseCostExpr = sum (warehouseCost .* warehouseDecs)

    // Create an expression for the cost of Roasters
    let roasterCostExpr = sum (roasterCost .* roasterDecs)

    // Create a Total Cost Expr by adding the Warehouse and Roasting expressions
    let totalCostExpr = warehouseCostExpr + roasterCostExpr

    // Create the Objective (minimize cost)
    let objective = Objective.create "MinimizeCost" Minimize totalCostExpr

    // Total Roasting capacity must be greater than 30 tons
    let roasterCapacityExpr = sum (roasterCapacity .* roasterDecs)

    let roasterCapacityConstraint = 
        Constraint.create "MinRoastingCapacity" (roasterCapacityExpr >== minRoastingCapacity)

    // Total Warehouse size must be greater than 30000 sq. ft.
    let warehouseCapacityExpr = sum (warehouseCapacity .* warehouseDecs)

    let warehouseCapacityConstraint = 
        Constraint.create "MinWarehouseSpace" (warehouseCapacityExpr >== minWarehouseCapacity)

    // Create a constraint for each Location which requires a
    // Warehouse wherever a Roaster is
    let warehouseAndRoasterConstraint =
        ConstraintBuilder "WarehouseAndRoasterCoexist" { 
            for location in locations ->
            roasterDecs.[location] <== warehouseDecs.[location]
        }

    // Create the model and add constraints
    let model = 
        Model.create objective
        |> Model.addConstraint roasterCapacityConstraint
        |> Model.addConstraint warehouseCapacityConstraint
        |> Model.addConstraints warehouseAndRoasterConstraint

    // Create the solver settings
    let settings = {
        SolverType = SolverType.CBC
        MaxDuration = 10_000L
        WriteLPFile = None
    }

    // Solve the model and save the results
    let result = Solver.solve settings model

    printfn "--Results--"
    // Print the results of the solver
    match result with
    | Optimal solution -> 
        printfn "Objective Value: %f" solution.ObjectiveResult
        for (decision, value) in solution.DecisionResults |> Map.toSeq do
            let (DecisionName name) = decision.Name
            printfn "Decision: %s\tValue: %f" name value
    | _ -> printfn "Unable to solve."