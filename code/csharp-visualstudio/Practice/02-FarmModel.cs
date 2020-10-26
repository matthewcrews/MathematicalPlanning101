using System;
using Google.OrTools.LinearSolver;

namespace Practice
{
  public class FarmModel
  {
    public static void Solve()
    {
      // Create an instance of the Solver class
      var solver = Solver.CreateSolver("FarmModel", "GLOP_LINEAR_PROGRAMMING");

      // Create variables for each type of Anaimal

      // Create expression for Revenue
      
      // Add the expression to the solver
      

      // Create constraint for Pasture
      

      // Create constraint for the Feed
      

      // Create constraint for the Labor
      

      // Solve the problem
      

      //// Evaluate the solve status
      //if (resultStatus != Solver.ResultStatus.OPTIMAL)
      //{
      //  Console.WriteLine("The problem does not have an optimal solution!");
      //  return;
      //}

      //// Print the results
      //Console.WriteLine("Solution:");
      //Console.WriteLine("Objective value = " + solver.Objective().Value());
      //foreach (var variable in solver.variables())
      //{
      //  Console.WriteLine($"{variable.Name()} = {variable.SolutionValue()}");
      //}
    }
  }
}
