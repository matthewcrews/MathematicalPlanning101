module Practice.RevisedFarmModel

open Flips
open Flips.Types

let solve () =
  // Define the set of animals you are working with
  let animals = ["cows"; "pigs"; "chickens"]

  // Dictionary of Revenue indexed by Animal Type
  let revenue = ["cows", 100.0; "pigs", 50.0; "chickens", 8.0] |> Map.ofList

  // Dictionary of Pasture required per animal indexed by Animal Type
  let pasture = ["cows",1.0; "pigs",0.5; "chickens",0.0] |> Map.ofList

  // Dictionary of Feed required per animal indexed by Animal Type
  let feed = ["cows",0.0; "pigs",1.0; "chickens",0.1] |> Map.ofList

  // Dictionary of the Labor required per animal indexed by Animal Type
  let labor = ["cows",9.5; "pigs",8.0; "chickens",0.0] |> Map.ofList

  // Constants which determine the limits on our resources
  let total_pasture = 1000.0
  let total_feed = 100.0
  let total_labor = 7200.0


  // Create decision variables for all of the animals


  // Create an objective expression
    

  // Create the objective


  // Add the constraint for the total amount of pasture available


  // Add the constraint for the total amount of feed available


  // Add the constraint for the total amount of labor available


  // Create the model and add the constraints


  // Create the solver settings
  let settings = {
          SolverType = SolverType.CBC
          MaxDuration = 10_000L
          WriteLPFile = None
      }

  // Solve the model and save the result

  printfn "--Results--"

  // Print the results of the solver
  //match result with
  //    | Suboptimal msg -> printfn "Unable to solve. Error %s" msg
  //    | Optimal solution ->
  //        for (decision, value) in solution.DecisionResults |> Map.toSeq do
  //            let (DecisionName name) = decision.Name
  //            printfn "Decision: %s\tValue: %f" name value  