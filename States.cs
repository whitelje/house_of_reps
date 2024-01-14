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

    /* "year","state","state_po","state_fips","state_cen","state_ic","office","candidate","party_detailed","writein","candidatevotes","totalvotes","version","notes","party_simplified" */

    internal States(string file)
    {
      this.states = new List<State>(50);
      using (var stream = new StreamReader(file))
      {
        while (!stream.EndOfStream)
        {
          var line = stream.ReadLine() ?? throw new EndOfStreamException();
          var values = line.Split(',');
          int result = 0;
          bool parsed = false;

          if (values.Length > 2)
          {
            parsed = int.TryParse(values[2], out result);
          }

          State state = parsed ? new State(values[0], int.Parse(values[1]), result) : new State(values[0], int.Parse(values[1]));
          this.states.Add(state);
        }
      }

      this.Reps = 50;
    }

    public int Reps { get; private set; }

    public State? this[string i] => this.states.Find(state => state.Name == i);

    public void AddRep()
    {
      this.GetStatesSortedOnReversePri()[0].AddRep();
      this.Reps++;
    }

    public float GetPRAvg()
    {
      return this.states.Sum(x => x.PR) / this.states.Count;
    }

    public int GetTotalPop()
    {
      return this.states.Sum(x => x.Pop);
    }

    public List<State> GetStatesSortedOnName()
    {
      return this.states.OrderBy(x => x.Name).ToList();
    }

    public List<State> GetStatesSortedOnReverseName()
    {
      List<State> states = this.states.OrderBy(x => x.Name).ToList();
      states.Reverse();
      return states;
    }

    public List<State> GetStatesSortedOnPop()
    {
      return this.states.OrderBy(x => x.Pop).ToList();
    }

    public List<State> GetStatesSortedOnReversePop()
    {
      List<State> states = this.states.OrderBy(x => x.Pop).ToList();
      states.Reverse();
      return states;
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
  }
}
