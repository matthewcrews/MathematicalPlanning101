using System;

namespace ConsoleRunner
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Optimization 101!");
      Console.WriteLine();

      Practice.FoodCart.Solve();
      //Practice.FarmModel.Solve();
      //Practice.RevisedFarmModel.Solve();
      //Practice.CoffePlanning.Solve();
      //Practice.PowerPlanning.Solve();

      Console.WriteLine("Press Enter to close...");
      Console.ReadLine();
    }
  }
}
