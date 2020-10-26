using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Practice
{
  public static class Extensions
  {
    public static LinearExpr Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, LinearExpr> selector)
    {
      var zeroExpr = new LinearExpr();
      return source.Aggregate(zeroExpr, (current, entry) => current + selector(entry));
    }
  }
}
