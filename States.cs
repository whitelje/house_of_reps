// <copyright file="States.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps
{
  using System.Collections;

  internal class States : IEnumerable<State>
  {
    private List<State> states;

    internal States(string file)
    {
      this.states = new List<State>(50);
      using (var stream = new StreamReader(file))
      {
        while (!stream.EndOfStream)
        {
          var line = stream.ReadLine() ?? throw new EndOfStreamException();
          var values = line.Split(',');

          State state = new State(values[0], int.Parse(values[1]));
          this.states.Add(state);
        }
      }

      this.CalculateHouse();
    }

    private int Reps => this.states.Sum(x => x.Reps);

    public State? this[string i] => this.states.Find(state => state.Name == i);

    private void AddRep()
    {
      this.GetStatesSortedOnReversePri()[0].AddRep();
    }

    private float GetPRAvg()
    {
      return (float)this.states.Sum(x => x.PR) / this.states.Count;
    }

    private int GetTotalPop()
    {
      return this.states.Sum(x => x.Pop);
    }

    public List<State> GetStatesSortedOnName()
    {
      return this.states.OrderBy(x => x.Name).ToList();
    }

    public List<State> GetStatesSortedOnReverseName()
    {
      return this.states.OrderBy(x => x.Name).Reverse().ToList();
    }

    public List<State> GetStatesSortedOnPop()
    {
      return this.states.OrderBy(x => x.Pop).ToList();
    }

    public List<State> GetStatesSortedOnReversePop()
    {
      return this.states.OrderBy(x => x.Pop).Reverse().ToList();
    }

    public double StandardDeviation()
    {
      double avg = this.states.Average(v => v.PR);
      return Math.Sqrt(this.states.Average(v => Math.Pow(v.PR - avg, 2)));
    }

    public State? LargestDeviant()
    {
      double std = this.StandardDeviation();
      var avg = this.GetPRAvg();
      return this.states.MaxBy(v => Math.Abs((avg - v.PR) / std));
    }

    public double LargestDeviation()
    {
      double std = this.StandardDeviation();
      var avg = this.GetPRAvg();
      return this.states.Max(v => Math.Abs((avg - v.PR) / std));
    }

    public List<State> GetStatesSortedOnPri()
    {
      return this.states.OrderBy(x => x.Pri).ToList();
    }

    public List<State> GetStatesSortedOnReversePri()
    {
      List<State> states = this.states.OrderBy(x => x.Pri).ToList();
      states.Reverse();
      return states;
    }

    public IEnumerator<State> GetEnumerator()
    {
      return ((IEnumerable<State>)this.states).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)this.states).GetEnumerator();
    }

    public void CalculateHouse()
    {
      var requiredReps = this.RequiredReps();
      while (this.Reps < requiredReps)
      {
        this.AddRep();
      }
    }

    public void WriteFullStatus()
    {
      var avg = this.GetPRAvg();
      var std = this.StandardDeviation();

      // Console.SetCursorPosition(0, 0);
      Console.WriteLine("After {0,4} reps, current average {1}, std dev: {2}", this.Reps, avg, std);
      Console.WriteLine("Current largest deviant {0}: {1}", this.LargestDeviant()?.Name ?? "None", this.LargestDeviation());

      var stateName = this.GetStatesSortedOnReversePop();

      foreach (var (state, index) in stateName.Select((value, i) => (value, i)))
      {
        Console.WriteLine("  {0,2}. {1}", index + 1, state.PrintStatus(avg, std));
      }
    }

    public void WriteConciseStatus()
    {
      var avg = this.GetPRAvg();
      var std = this.StandardDeviation();
      Console.WriteLine("After {0,4} reps, current average {1}, std dev: {2}", this.Reps, avg, std);
      Console.WriteLine("Current largest deviant {0}: {1}", this.LargestDeviant()?.Name ?? "None", this.LargestDeviation());
    }

    private int RequiredReps()
    {
      int repMax = 0;
      int targetRepresentation = 30000;
      while (this.GetTotalPop() / targetRepresentation >= repMax)
      {
        targetRepresentation += 10000;
        repMax += 100;
      }

      Console.WriteLine("RepMax:  " + repMax);
      Console.WriteLine("ReqReps: " + ((int)this.GetTotalPop() / targetRepresentation));
      return (int)this.GetTotalPop() / targetRepresentation;
    }
  }
}
