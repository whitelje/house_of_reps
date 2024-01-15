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
      // Console.SetWindowSize(80, 54);
      var states = new States("2000_census.csv");
      Console.WriteLine("Loaded States");

      // states.WriteFullStatus();
      states.WriteConciseStatus();
      Console.WriteLine("Electoral College");
      Console.WriteLine();
      WriteElection(new Election("1976-2020-president_nodc.csv", 2000, states).ComputeElectoralCollege(states));
      Console.WriteLine();
      Console.WriteLine("House of Reps");
      Console.WriteLine();
      WriteElection(new Election("1976-2020-president_nodc.csv", 2000, states).ComputeHouseOfReps(states));
    }

    internal static void WriteElection(Dictionary<string, int> results)
    {
      int totalReps = 0;
      foreach (KeyValuePair<string, int> res in results)
      {
        Console.WriteLine("{0}: {1}", res.Key, res.Value);
        totalReps += res.Value;
      }

      Console.WriteLine("Total Reps: {0}", totalReps);
    }
  }
}