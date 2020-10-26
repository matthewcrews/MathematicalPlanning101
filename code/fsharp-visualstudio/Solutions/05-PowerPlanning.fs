module Solutions.PowerPlanning

open Flips
open Flips.Types

let solve () =

  // Setup sets
  let cities = ["C1"; "C2"; "C3"; "C4"; "C5"; "C6"; "C7"; "C8"; "C9"]

  let powerPlants = ["P1"; "P2"; "P3"; "P4"; "P5"; "P6"]

  let connections = [
      ("C1", "P1"); ("C1", "P3"); ("C1", "P5");
      ("C2", "P1"); ("C2", "P2"); ("C2","P4");
      ("C3", "P2"); ("C3", "P3"); ("C3","P4");
      ("C4", "P2"); ("C4", "P4"); ("C4","P6");
      ("C5", "P2"); ("C5", "P5"); ("C5","P6");
      ("C6", "P3"); ("C6", "P4"); ("C6","P6");
      ("C7", "P1"); ("C7", "P3"); ("C7","P6");
      ("C8", "P2"); ("C8", "P3"); ("C8","P4");
      ("C9", "P3"); ("C9", "P5"); ("C9","P6")
  ]
    
  // Setup parameters
  let maxPowerGeneration = ["P1", 100.0; "P2", 150.0; "P3", 250.0; "P4", 125.0; "P5", 175.0; "P6", 165.0] |> Map.ofSeq

  let startupCost = ["P1", 50.0; "P2", 80.0; "P3", 90.0; "P4", 60.0; "P5", 60.0; "P6", 70.0] |> Map.ofSeq

  let powerCost = ["P1", 2.0; "P2", 1.5; "P3", 1.2; "P4", 1.8; "P5", 0.8; "P6", 1.1] |> Map.ofSeq

  let powerRequired = ["C1", 25.0; "C2", 35.0; "C3", 30.0; "C4", 125.0; "C5", 40.0; "C6", 35.0; "C7", 50.0; "C8", 45.0; "C9", 38.0] |> Map.ofSeq

  // Create decision variables to turn plant on
  let runPowerPlant = 
      DecisionBuilder "RunPlant" {
          for plant in powerPlants -> Boolean
      } |> Map.ofSeq

  // Create decisions for how much power to generate
  let powerGeneration = 
      DecisionBuilder "PowerGeneration" {
          for plant in powerPlants -> Continuous (0.0, infinity)
      } |> Map.ofSeq

  // Create decision for how much power to send from a PowerPlant
  // to a City
  let powerSent =
      DecisionBuilder "PowerSent" {
          for connection in connections -> Continuous (0.0, infinity)
      } |> Map.ofSeq

  // Create expression for cost of turning Power Plants on
  let startupCostExpr = 
    [for city, plant in connections -> 
      startupCost.[plant] * runPowerPlant.[plant]
    ] |> List.sum

  // Create expression for the cost of power generation
  let powerCostExpr = 
    [for city, plant in connections -> 
      powerGeneration.[plant] * powerCost.[plant]
    ] |> List.sum

  // Create an objective function
  let totalCostExpr = startupCostExpr + powerCostExpr

  // Create the Objective
  let objective = Objective.create "MinimizeCost" Minimize totalCostExpr

  // Add Power Capacity Constraints
  let powerCapConstraint = ConstraintBuilder "PowerCapacity" {
      for plant in powerPlants ->
          powerGeneration.[plant] <== maxPowerGeneration.[plant] * runPowerPlant.[plant]
  }

  // Add Power Balance Constraints
  let powerBalanceConstraint = ConstraintBuilder "PowerSent" {
      for plant in powerPlants -> 
          // Gets all connections out of a the given plant
          let connectionsForPlant =  connections |> List.filter (fun (_, powerPlant) -> powerPlant = plant)
        
          // Sums up the power sent through the connections
          let totalPowerSent = 
              connectionsForPlant
              |> List.sumBy (fun conn -> LinearExpression.OfDecision powerSent.[conn]) // Same result comes from 1.0* powerSent.[conn]
          totalPowerSent == powerGeneration.[plant]
  }

  // Add Cities Powered Constraints
  let citiesPoweredConstraint = ConstraintBuilder "PowerRequired" {
      for city in cities ->
          let connectionsForCity = connections |> List.filter (fun (c,_) -> c = city)
          let totalPowerRequired =
              connectionsForCity
              |> List.sumBy (fun conn -> LinearExpression.OfDecision powerSent.[conn]) // does the same thing as map and sum, but in one function
          totalPowerRequired == powerRequired.[city]
  }

  // Create the model and add constraints
  let model = 
      Model.create objective
      |> Model.addConstraints powerCapConstraint
      |> Model.addConstraints powerBalanceConstraint
      |> Model.addConstraints citiesPoweredConstraint


  // Create the solver settings
  let settings = {
      SolverType = SolverType.CBC
      MaxDuration = 10_000L
      WriteLPFile = None
  }

  // Solve the model and save the result
  let result = Solver.solve settings model

  printfn "--Results--"
  // Print the results
  match result with
  | Suboptimal msg -> printfn"Unable to solve. Error: %s" msg
  | Optimal solution ->
      printfn "Objective Result: %f" solution.ObjectiveResult
      for decision, value in solution.DecisionResults |> Map.toSeq do
          let (DecisionName name) = decision.Name
          printfn "Decision: %s\tValue: %f" name value