module Solutions.PowerPlanning

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
    let runPowerPlant = 
        DecisionBuilder "RunPlant" {
            for plant in powerPlants -> Boolean
        } |> SMap

    // Create decisions for how much power to generate
    let powerGeneration = 
        DecisionBuilder "PowerGeneration" {
            for plant in powerPlants -> Continuous (0.0, infinity)
        } |> SMap

    // Create decision for how much power to send from a PowerPlant
    // to a City
    let powerSent =
        DecisionBuilder "PowerSent" {
            for connection in connections -> Continuous (0.0, infinity)
        } |> SMap2

    // Create expression for cost of turning Power Plants on
    let startupCostExpr = sum (startupCost .* runPowerPlant)

    // Create expression for the cost of power generation
    let powerCostExpr = sum (powerGeneration .* powerCost)

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
            sum powerSent.[All, plant] == powerGeneration.[plant]
    }

    // Add Cities Powered Constraints
    let citiesPoweredConstraint = ConstraintBuilder "PowerRequired" {
        for city in cities ->
            sum powerSent.[city, All] == powerRequired.[city]
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
    | Optimal solution ->
        printfn "Objective Result: %f" solution.ObjectiveResult
        for decision, value in solution.DecisionResults |> Map.toSeq do
            let (DecisionName name) = decision.Name
            printfn "Decision: %s\tValue: %f" name value
    | _ -> printfn"Unable to solve."