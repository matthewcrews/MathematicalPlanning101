module Solutions.FoodTruck

open Flips
open Flips.Types

let solve () =

  // Create a decision variable for the number of burgers below
  let numberOfHamburgers = Decision.createContinuous "NumberOfHamburgers" 0.0 infinity

  // Create a decision variable for the number of tacos below
  let numberOfTacos = Decision.createContinuous "NumberOfTacos" 0.0 infinity

  // Create an objective expression here
  let objectiveExpression = 2.0 * numberOfHamburgers + 1.0 * numberOfTacos

  // Create an objective here
  let objective = 
    Objective.create "MaximizeRevenue" Maximize objectiveExpression

  // Create a constraint for the max number of Burgers
  let maxBurgers = 
    Constraint.create "MaxBurgers" (numberOfHamburgers <== 300.0)

  // Create a constraint for the max number of Tacos
  let maxTacos = 
    Constraint.create "MaxTacos" (numberOfTacos <== 200.0)

  // Create a constraint for the total weight
  let maxWeight = 
    Constraint.create "MaxWeight" ((1.5*numberOfTacos+1.0*numberOfHamburgers) <== 450.0)


  // Create the model below
  let model =
      Model.create objective
      |> Model.addConstraint maxBurgers
      |> Model.addConstraint maxTacos
      |> Model.addConstraint maxWeight


  let settings = {
    SolverType = SolverType.CBC
    MaxDuration = 10_000L
    WriteLPFile = None
  }

  // Store result of solved model below
  let result = Solver.solve settings model


  printfn "--Result--"

  // Print the results of the solver below
  match result with
  | Optimal solution ->
      printfn "Objective Value: %f" solution.ObjectiveResult

      for (decision, value) in solution.DecisionResults |> Map.toSeq do
          let (DecisionName name) = decision.Name
          printfn "Decision: %s\tValue: %f" name value
  | _ -> printfn "Unable to solve."