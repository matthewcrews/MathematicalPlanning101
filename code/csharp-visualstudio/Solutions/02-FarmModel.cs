using System;
using Google.OrTools.LinearSolver;

namespace Solutions
{
  public class FarmModel
  {
    public static void Solve()
    {
      // Create an instance of the Solver class
      var solver = Solver.CreateSolver("FarmModel", "GLOP_LINEAR_PROGRAMMING");

      // Create variables for each type of Anaimal
      var cows = solver.MakeNumVar(0.0, Double.MaxValue, "Cows");
      var pigs = solver.MakeNumVar(0.0, Double.MaxValue, "Pigs");
      var chickens = solver.MakeNumVar(0.0, Double.MaxValue, "Chickens");

      // Create expression for Revenue
      var objExpr = 100.0 * cows + 50.0 * pigs + 8.0 * chickens;
      // Add the expression to the solver
      solver.Maximize(objExpr);

      // Create constraint for Pasture
      var maxPasture = 1.0 * cows + 0.2 * pigs <= 1000.0;
      solver.Add(maxPasture);

      // Create constraint for the Feed
      var maxFeed = 1.0 * pigs + 0.1 * chickens <= 100.0;
      solver.Add(maxFeed);

      // Create constraint for the Labor
      var maxLabor = 9.5 * cows + 8.0 * pigs <= 7200.0;
      solver.Add(maxLabor);

      // Solve the problem
      var resultStatus = solver.Solve();

      // Evaluate the solve status
      if (resultStatus != Solver.ResultStatus.OPTIMAL)
      {
        Console.WriteLine("The problem does not have an optimal solution!");
        return;
      }

      // Print the results
      Console.WriteLine("Solution:");
      Console.WriteLine("Objective value = " + solver.Objective().Value());
      foreach (var variable in solver.variables())
      {
        Console.WriteLine($"{variable.Name()} = {variable.SolutionValue()}");
      }
    }
  }
}
