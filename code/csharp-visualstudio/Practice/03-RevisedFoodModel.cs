using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.LinearSolver;

namespace Practice
{
  public class RevisedFarmModel
  {
    public static void Solve()
    {
      var animals = new List<string>
      {
        "cow", "pig", "chicken"
      };

      var revenue = new Dictionary<string, double>()
      {
        { "cow", 100.0 }, {"pig", 50.0 }, {"chicken", 8.0 }
      };

      var pasture = new Dictionary<string, double>()
      {
        { "cow", 1.0 }, {"pig", 0.5 }, {"chicken", 0.0 }
      };

      var feed = new Dictionary<string, double>()
      {
        { "cow", 0.0 }, {"pig", 1.0 }, {"chicken", 0.1 }
      };

      var labor = new Dictionary<string, double>()
      {
        { "cow", 9.5 }, {"pig", 8.0 }, {"chicken", 0.0 }
      };

      var totalPasture = 1000.0;
      var totalFeed = 100.0;
      var totalLabor = 7200.0;

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
