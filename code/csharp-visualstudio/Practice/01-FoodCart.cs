using System;
using Google.OrTools.LinearSolver;


namespace Practice
{
  public class FoodCart
  {
    public static void Solve()
    {
      var solver = Solver.CreateSolver("FoodCart", "GLOP_LINEAR_PROGRAMMING");

      var numberOfHamburgers = solver.MakeNumVar(0.0, Double.MaxValue, "X1");
      var numberOfTacos = solver.MakeNumVar(0.0, Double.MaxValue, "X2");

      var objExpr = 1.0 * numberOfHamburgers + 1.5 * numberOfTacos;
      solver.Maximize(objExpr);

      var maxHamburgers = numberOfHamburgers <= 300.0;
      solver.Add(maxHamburgers);

      var maxTacos = numberOfTacos <= 200.0;
      solver.Add(maxTacos);

      var maxWeight = 1.5 * numberOfTacos + 1.0 * numberOfHamburgers <= 450.0;
      solver.Add(maxWeight);

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
