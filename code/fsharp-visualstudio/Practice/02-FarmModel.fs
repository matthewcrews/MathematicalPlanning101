module Practice.FarmModel

open Flips
open Flips.Types

let solve () =

    // Create decision variables for cows, pigs, and chickens

    // Create an objective expression

    // Create an objective


    // Add constraint for amount of Pasture

    // Add constraint for amount of Feed

    // Add constraint for amount of Labor


    // Create the model and add the constraints


    // Create the solver settings
    let settings = {
        SolverType = SolverType.CBC
        MaxDuration = 10000L
        WriteLPFile = None
    }

    // Solve the model and save the results

    printfn "--Results--"

    // Print the results of the solver
    //match result with
    //| Optimal solution -> 
    //    printfn "Objective Value: %f" solution.ObjectiveResult
    //    for (decision, value) in solution.DecisionResults |> Map.toSeq do
    //        let (DecisionName name) = decision.Name
    //        printfn "Decision: %s\tValue: %f" name value
    //| _ -> printfn "Unable to solve."