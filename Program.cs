using house_of_reps;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.SetWindowSize(80, 54);
      var states = new States("2000_census.csv");
      Console.WriteLine("Loaded States");
      int targetReps = ComputeHouseSize(states.GetTotalPop());
      while (states.Reps < targetReps)
      {
        //WriteFullStatus(states);
        states.AddRep();
      }
      while( true )
      {
        WriteFullStatus(states);
        WriteElection(new Election("2004_electoral.csv").ComputeElectoralCollege(states));
        var val = Console.ReadKey(true).KeyChar;
        if (val == 'q') break;
      }
    }

    internal static int ComputeHouseSize( int pop )
    {
      int rep_max = 0;
      int target_representation = 30000;
      while( pop / target_representation >= rep_max )
      {
        target_representation += 10000;
        rep_max += 100;
      }
      Console.WriteLine(rep_max);
      Console.ReadKey(true);
      return (int)pop / target_representation;
    }

    internal static void WriteElection( Tuple<int, int> results )
    {
      Console.WriteLine("Dem: {0,4}, Rep: {1,4}", results.Item1, results.Item2);
    }

    internal static void WriteFullStatus( States states )
    {
      var avg = states.GetPRAvg();
      var std = states.StandardDeviation();
      Console.SetCursorPosition(0, 0);
      Console.WriteLine("After {0,4} reps, current average {1}, std dev: {2}", states.Reps, avg, std );
      Console.WriteLine("Current largest deviant {0}: {1}", states.LargestDeviant()?.Name ?? "None", states.LargestDeviation());

      var stateName = states.GetStatesSortedOnReversePop();

      foreach ( var (state, index) in stateName.Select((value, i) => (value, i)))
      {
        Console.WriteLine( "  {0,2}. {1}", index + 1, state.PrintStatus(avg, std));
      }
    }

    internal static SortedList<double, State> AddRep( SortedList<double, State> states )
    {
      State st = states.Values[0];
      states.RemoveAt(0);
      st.AddRep();
      states.Add(st.Pri, st);
      return states;
    }

    internal static float CalculateAverage( SortedList<double, State> states)
    {
      float avg = 0f;
      foreach ( var state in states )
      {
        avg += state.Value.PR;
      }
      return avg / 50.0f;
    }


  }
}