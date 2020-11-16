module Solutions.CoffeeModel

open Flips
open Flips.Types

let solve () =

  let locations = ["Sellwood"; "Hawthorne"; "The Pearl"; "Eastmoreland"; "St. Johns"; "Alberta"; "Nob Hill"; "Belmont"]

  let roasterCost = ["Sellwood", 150000.0; "Hawthorne", 100000.0; "The Pearl", 250000.0; "Eastmoreland", 120000.0; "St. Johns", 130000.0;
                     "Alberta", 110000.0; "Nob Hill", 135000.0; "Belmont", 180000.0] |> Map.ofSeq
                   
  let roasterCapacity = ["Sellwood", 12.0; "Hawthorne", 18.0; "The Pearl", 22.0; "Eastmoreland", 13.0;
                         "St. Johns", 14.0; "Alberta", 10.0; "Nob Hill", 17.0; "Belmont", 12.0] |> Map.ofSeq
                       
  let warehouseCost = ["Sellwood", 80000.0; "Hawthorne", 90000.0; "The Pearl", 120000.0; "Eastmoreland", 90000.0;
                       "St. Johns", 85000.0; "Alberta", 70000.0; "Nob Hill", 85000.0; "Belmont", 90000.0] |> Map.ofSeq
                     
  let warehouseSize = ["Sellwood", 8000.0; "Hawthorne", 6000.0; "The Pearl", 12000.0; "Eastmoreland", 6000.0;
                       "St. Johns", 7000.0; "Alberta", 9000.0; "Nob Hill", 6000.0; "Belmont", 9200.0] |> Map.ofSeq
                     
  let minWarehouseCapacity = 30000.0
  let minRoastingCapacity = 30.0

  // Create variables to indicate whether to build a Roaster at a given location
  let roasterDecs = 
      DecisionBuilder "BuildRoaster" {
          for location in locations -> Boolean 
      } |> Map.ofSeq

  // Create variables to indicate whether or not to build a Warehouse at a given location
  let warehouseDecs =
      DecisionBuilder "BuildWarehouse" {
          for location in locations -> Boolean
      } |> Map.ofSeq

  // Create an expression for the cost of Warehouses
  let warehouseCostExpr = 
    [for location in locations -> 
      warehouseCost.[location] * warehouseDecs.[location]
    ] |> List.sum

  // Create an expression for the cost of Roasters
  let roasterCostExpr = 
    [for location in locations -> 
      roasterCost.[location] * roasterDecs.[location]
    ] |> List.sum

  // Create a Total Cost Expr by adding the Warehouse and Roasting expressions
  let totalCostExpr = warehouseCostExpr + roasterCostExpr

  // Create the Objective (minimize cost)
  let objective = Objective.create "MinimizeCost" Minimize totalCostExpr

  // Total Roasting capacity must be greater than 30 tons
  let roasterCapacityExpr = 
    [for location in locations -> 
      roasterCapacity.[location] * roasterDecs.[location]
    ] |> List.sum

  let roasterCapacityConstraint = 
    Constraint.create "MinRoastingCapacity" (roasterCapacityExpr >== minRoastingCapacity)

  // Total Warehouse size must be greater than 30000 sq. ft.
  let warehouseCapacityExpr = 
    [for location in locations -> 
      warehouseSize.[location] * roasterDecs.[location]
    ] |> List.sum

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