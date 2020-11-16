module Solutions.FarmModel

open Flips
open Flips.Types

let solve () =

  // Create decision variables for cows, pigs, and chickens
  let cows = Decision.createContinuous "NumberOfCows" 0.0 infinity
  let pigs = Decision.createContinuous "NumberOfPigs" 0.0 infinity
  let chickens = Decision.createContinuous "NumberOfChickens" 0.0 infinity

  // Create an objective expression
  let objectiveExpression = 100.0*cows + 50.0*pigs + 8.0*chickens
  // Create an objective
  let objective = Objective.create "MaximizeRevenue" Maximize objectiveExpression


  // Add constraint for amount of Pasture
  let maxPasture = Constraint.create "MaxPasture" (1.0*cows + 0.2*pigs <== 1000.0)
  // Add constraint for amount of Feed
  let maxFeed = Constraint.create "MaxFeed" (1.0*pigs + 0.1*chickens <== 100.0)
  // Add constraint for amount of Labor
  let maxLabor = Constraint.create "MaxLabor" (9.5*cows + 8.0*pigs <== 7200.0)

  // Create the model and add the constraints
  let model =
    Model.create objective
    |> Model.addConstraint maxPasture
    |> Model.addConstraint maxFeed
    |> Model.addConstraint maxLabor


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