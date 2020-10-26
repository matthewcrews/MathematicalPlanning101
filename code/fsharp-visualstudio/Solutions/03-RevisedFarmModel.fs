module Solutions.RevisedFarmModel

open Flips
open Flips.Types

let solve () =

  // Define the set of animals you are working with
  let animals = ["cow"; "pig"; "chicken"]

  // Dictionary of Revenue indexed by Animal Type
  let revenue = ["cow", 100.0; "pig", 50.0; "chicken", 8.0] |> Map.ofList

  // Dictionary of Pasture required per animal indexed by Animal Type
  let pasture = ["cow",1.0; "pig",0.5; "chicken",0.0] |> Map.ofList

  // Dictionary of Feed required per animal indexed by Animal Type
  let feed = ["cow",0.0; "pig",1.0; "chicken",0.1] |> Map.ofList

  // Dictionary of the Labor required per animal indexed by Animal Type
  let labor = ["cow",9.5; "pig",8.0; "chicken",0.0] |> Map.ofList

  // Constants which determine the limits on our resources
  let totalPasture = 1000.0
  let totalFeed = 100.0
  let totalLabor = 7200.0


  // Create decision variables for all of the animals
  let animalDecs =
      DecisionBuilder "NumberOf" {
          for animal in animals -> Continuous (0.0, infinity)
      }
     |> Map.ofSeq


  // Create an objective expression
  let objectiveExpression = 
    [ for KeyValue(animal, decVar) in animalDecs -> 
      (revenue.[animal] * decVar)
    ] |> List.sum
    
  // Create the objective
  let objective = Objective.create "MaximizeRevenue" Maximize objectiveExpression

  // Add the constraint for the total amount of pasture available
  let pastureExpr = 
    [ for animal in animals -> 
      pasture.[animal] * animalDecs.[animal]
    ] |> List.sum

  let maxPastureConstraint = Constraint.create "MaxPasture" (pastureExpr <== totalPasture)

  // Add the constraint for the total amount of feed available
  let feedExpr = 
    [ for animal in animals -> 
      feed.[animal] * animalDecs.[animal]
    ] |> List.sum

  let maxFeedConstraint = Constraint.create "MaxFeed" (feedExpr <== totalFeed)

  // Add the constraint for the total amount of labor available
  let laborExpr = 
    [ for animal in animals -> 
      labor.[animal] * animalDecs.[animal]
    ] |> List.sum

  let maxLaborConstraint = Constraint.create "MaxLabor" (laborExpr <== totalLabor)


  // Create the model and add the constraints
  let model = 
      Model.create objective
      |> Model.addConstraint maxPastureConstraint
      |> Model.addConstraint maxFeedConstraint
      |> Model.addConstraint maxLaborConstraint


  // Create the solver settings
  let settings = {
          SolverType = SolverType.CBC
          MaxDuration = 10_000L
          WriteLPFile = None
      }

  // Solve the model and save the result
  let result = Solver.solve settings model

  printfn "--Results--"

  // Print the results of the solver
  match result with
      | Suboptimal msg -> printfn "Unable to solve. Error %s" msg
      | Optimal solution ->
          for (decision, value) in solution.DecisionResults |> Map.toSeq do
              let (DecisionName name) = decision.Name
              printfn "Decision: %s\tValue: %f" name value  