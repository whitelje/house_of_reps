// <copyright file="Program.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps // Note: actual namespace depends on the project name.
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      Console.SetWindowSize(80, 54);
      var states = new States("2000_census.csv");
      Console.WriteLine("Loaded States");

      while (true)
      {
        states.WriteFullStatus();
        WriteElection(new Election("2004_electoral.csv", 2004, states).ComputeElectoralCollege(states));
        var val = Console.ReadKey(true).KeyChar;
        if (val == 'q')
        {
            break;
        }
      }
    }

    internal static void WriteElection(Tuple<int, int> results)
    {
      Console.WriteLine("Dem: {0,4}, Rep: {1,4}", results.Item1, results.Item2);
    }
  }
}