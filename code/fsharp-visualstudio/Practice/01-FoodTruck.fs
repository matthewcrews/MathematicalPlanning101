module Practice.FoodTruck

open Flips
open Flips.Types

let solve () =

    // Create a decision variable for the number of burgers below

    // Create a decision variable for the number of tacos below

    // Create an objective expression here

    // Create an objective here

    // Create a constraint for the max number of Burgers

    // Create a constraint for the max number of Tacos

    // Create a constraint for the total weight


    // Create the model below


    let settings = {
        SolverType = SolverType.CBC
        MaxDuration = 10_000L
        WriteLPFile = None
    }

    // Store result of solved model below


    printfn "--Result--"

    // Print the results of the solver below
    //match result with
    //| Optimal solution -> 
    //    printfn "Objective Value: %f" solution.ObjectiveResult
    //    for (decision, value) in solution.DecisionResults |> Map.toSeq do
    //        let (DecisionName name) = decision.Name
    //        printfn "Decision: %s\tValue: %f" name value
    //| _ -> printfn "Unable to solve."