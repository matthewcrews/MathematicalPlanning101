module Practice.PowerPlanning

open Flips
open Flips.Types
open Flips.SliceMap

type City = City of string
type Plant = Plant of string

let solve () =

    // Setup sets
    let cities = 
        [
            City "C1"
            City "C2"
            City "C3"
            City "C4"
            City "C5"
            City "C6"
            City "C7"
            City "C8"
            City "C9"
        ]

    let powerPlants = 
        [
            Plant "P1"
            Plant "P2"
            Plant "P3"
            Plant "P4"
            Plant "P5"
            Plant "P6"
        ]

    let connections = 
        [
            (City "C1", Plant "P1"); (City "C1", Plant "P3"); (City "C1", Plant "P5");
            (City "C2", Plant "P1"); (City "C2", Plant "P2"); (City "C2", Plant "P4");
            (City "C3", Plant "P2"); (City "C3", Plant "P3"); (City "C3", Plant "P4");
            (City "C4", Plant "P2"); (City "C4", Plant "P4"); (City "C4", Plant "P6");
            (City "C5", Plant "P2"); (City "C5", Plant "P5"); (City "C5", Plant "P6");
            (City "C6", Plant "P3"); (City "C6", Plant "P4"); (City "C6", Plant "P6");
            (City "C7", Plant "P1"); (City "C7", Plant "P3"); (City "C7", Plant "P6");
            (City "C8", Plant "P2"); (City "C8", Plant "P3"); (City "C8", Plant "P4");
            (City "C9", Plant "P3"); (City "C9", Plant "P5"); (City "C9", Plant "P6")
        ]

    // Setup parameters
    let maxPowerGeneration = 
        [ 
            Plant "P1", 100.0
            Plant "P2", 150.0
            Plant "P3", 250.0
            Plant "P4", 125.0
            Plant "P5", 175.0
            Plant "P6", 165.0
        ] |> SMap

    let startupCost = 
        [
            Plant "P1", 50.0
            Plant "P2", 80.0
            Plant "P3", 90.0
            Plant "P4", 60.0
            Plant "P5", 60.0
            Plant "P6", 70.0
        ] |> SMap

    let powerCost = 
        [
            Plant "P1", 2.0
            Plant "P2", 1.5
            Plant "P3", 1.2
            Plant "P4", 1.8
            Plant "P5", 0.8
            Plant "P6", 1.1
        ] |> SMap

    let powerRequired = 
        [
            City "C1", 25.0
            City "C2", 35.0
            City "C3", 30.0
            City "C4", 125.0
            City "C5", 40.0
            City "C6", 35.0
            City "C7", 50.0
            City "C8", 45.0
            City "C9", 38.0
        ] |> SMap

    // Create decision variables to turn plant on


    // Create decisions for how much power to generate


    // Create decision for how much power to send from a PowerPlant
    // to a City


    // Create expression for cost of turning Power Plants on


    // Create expression for the cost of power generation


    // Create an objective function


    // Create the Objective


    // Add Power Capacity Constraints


    // Add Power Balance Constraints


    // Add Cities Powered Constraints

    // Create the model and add constraints



    // Create the solver settings
    let settings = {
        SolverType = SolverType.CBC
        MaxDuration = 10_000L
        WriteLPFile = None
    }

    // Solve the model and save the result


    printfn "--Results--"
    // Print the results
    //match result with
    //| Optimal solution -> 
    //    printfn "Objective Value: %f" solution.ObjectiveResult
    //    for (decision, value) in solution.DecisionResults |> Map.toSeq do
    //        let (DecisionName name) = decision.Name
    //        printfn "Decision: %s\tValue: %f" name value
    //| _ -> printfn "Unable to solve."