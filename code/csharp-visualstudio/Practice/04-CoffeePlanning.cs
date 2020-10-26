using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practice
{
  public class CoffePlanning
  {
    public static void Solve()
    {
      var locations = new List<string>()
      {
        "Sellwood", "Hawthorne", "The Pearl", "Eastmoreland", "St. Johns", "Alberta", "Nob Hill", "Belmont"
      };

      var roasterCost = new Dictionary<string, double>() {
        {"Sellwood", 150000.0},
        {"Hawthorne", 100000.0},
        {"The Pearl", 250000.0},
        {"Eastmoreland", 120000.0},
        {"St. Johns", 130000.0},
        {"Alberta", 110000.0},
        {"Nob Hill", 135000.0},
        {"Belmont", 180000.0}
      };

      var roasterCapacity = new Dictionary<string, Double>()
      {
        {"Sellwood", 12.0     },
        {"Hawthorne", 18.0    },
        {"The Pearl", 22.0    },
        {"Eastmoreland", 13.0 },
        {"St. Johns", 14.0    },
        {"Alberta", 10.0      },
        {"Nob Hill", 17.0     },
        {"Belmont", 12.0     }
      };

      var warehouseCost = new Dictionary<string, Double>()
      {
        {"Sellwood", 80000.0     },
        {"Hawthorne", 90000.0    },
        {"The Pearl", 120000.0   },
        {"Eastmoreland", 90000.0 },
        {"St. Johns", 85000.0    },
        {"Alberta", 70000.0      },
        {"Nob Hill", 85000.0     },
        {"Belmont", 90000.0 }
      };

      var warehouseCapacity = new Dictionary<string, Double>()
      {
        {"Sellwood", 8000.0     },
        {"Hawthorne", 6000.0    },
        {"The Pearl", 12000.0   },
        {"Eastmoreland", 6000.0 },
        {"St. Johns", 7000.0    },
        {"Alberta", 9000.0      },
        {"Nob Hill", 6000.0     },
        {"Belmont", 9200.0 }
      };

      var minWarehouseCapacity = 30000.0;
      var minRoastingCapacity = 30.0;

      // Create an instance of the Solver class
      var solver = Solver.CreateSolver("CoffeePlanning", "CBC_MIXED_INTEGER_PROGRAMMING");

      // Create Dictionary of Roaster variables



      // Create Dictionary of Warehouse variables


      // Create Roaster cost expression
      

      // Create Warehouse cost expression
      

      // Create total cost expression
      

      // Set the objective to Minimize the Total Cost
      

      // Create Roaster capacity expression
      

      // Add constraint that Roaster Capacity > Min Roaster Capacity
      

      // Create Warehouse Capacity expression
      

      // Add constraint that Total Warehouse > Min Warehouse Capacity
      

      // For each Location, create a constraint that where there is a Roaster
      // there must also be a Warehouse
      

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
