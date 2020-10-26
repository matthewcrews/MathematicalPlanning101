using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Practice
{
  public class PowerPlanning
  {
    public static void Solve()
    {
      var cities = new List<string>()
      {
        "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9"
      };

      var powerPlants = new List<string>()
      {
        "P1", "P2", "P3", "P4", "P5", "P6"
      };

      var connections = new HashSet<(string, string)>
      {
        ("C1", "P1"), ("C1", "P3"), ("C1", "P5"),
        ("C2", "P1"), ("C2", "P2"), ("C2", "P4"),
        ("C3", "P2"), ("C3", "P3"), ("C3", "P4"),
        ("C4", "P2"), ("C4", "P4"), ("C4", "P6"),
        ("C5", "P2"), ("C5", "P5"), ("C5", "P6"),
        ("C6", "P3"), ("C6", "P4"), ("C6", "P6"),
        ("C7", "P1"), ("C7", "P3"), ("C7", "P6"),
        ("C8", "P2"), ("C8", "P3"), ("C8", "P4"),
        ("C9", "P3"), ("C9", "P5"), ("C9", "P6")
      };

      var maxPowerGeneration = new Dictionary<string, double>()
      {
         { "P1", 100.0 },
         { "P2", 150.0 },
         { "P3", 250.0 },
         { "P4", 125.0 },
         { "P5", 175.0 },
         { "P6", 165.0 }
      };

      var startupCost = new Dictionary<string, double>()
      {
        { "P1", 50.0 },
        { "P2", 80.0 },
        { "P3", 90.0 },
        { "P4", 60.0 },
        { "P5", 60.0 },
        { "P6", 70.0 }
      };

      var powerCost = new Dictionary<string, double>()
      {
        { "P1", 2.0 },
        { "P2", 1.5 },
        { "P3", 1.2 },
        { "P4", 1.8 },
        { "P5", 0.8 },
        { "P6", 1.1 }
      };

      var powerRequired = new Dictionary<string, double>()
      {
        { "C1",  25.0 },
        { "C2",  35.0 },
        { "C3",  30.0 },
        { "C4", 125.0 },
        { "C5",  40.0 },
        { "C6",  35.0 },
        { "C7",  50.0 },
        { "C8",  45.0 },
        { "C9",  38.0 }
      };


      // Create an instance of the Solver class
      var solver = Solver.CreateSolver("CoffeePlanning", "CBC_MIXED_INTEGER_PROGRAMMING");

      // Create decision variables to turn plant on
     

      // Create variables for the amount of power generation
      

      // Create variable for how much power to send from Power Plant
      // to City
      

      // Create Startup Cost expression
      

      // Create Power Generation Cost expression
      

      // Create Total Cost expression
      

      // Set the objective to minimize the total cost
      

      // Add constraints to limit max power generation
      

      // Add Power Balance Constraints
      

      // Add Cities powered Constraints
      

      //// Solve the problem
      //var resultStatus = solver.Solve();

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
