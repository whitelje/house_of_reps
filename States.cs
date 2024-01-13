using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace house_of_reps
{
  internal class States : IEnumerable<State>
  {
    List<State> _states { get; }
    int _reps { get; set; }
    public int Reps { get { return _reps; } }

    /* "year","state","state_po","state_fips","state_cen","state_ic","office","candidate","party_detailed","writein","candidatevotes","totalvotes","version","notes","party_simplified" */

    internal States(String file)
    {
      _states = new List<State>(50);
      using (var stream = new StreamReader(file))
      {
        while (!stream.EndOfStream)
        {
          var line = stream.ReadLine();
          var values = line?.Split(',');
          int result = 0;
          bool parsed = false;

          if( values.Length > 2 )
          {
            parsed = int.TryParse(values[2], out result);
          }
          
          State state;

          if( parsed )
          {
            state = new State(values[0], int.Parse(values[1]), result);

          } else
          {
            state = new State(values[0], int.Parse(values[1]));
          }

          _states.Add(state);
        }
      }
      _reps = 50;
    }

    public void AddRep()
    {
      GetStatesSortedOnReversePri()[0].AddRep();
      _reps++;
    }

    public float GetPRAvg()
    {
      return _states.Sum(x => x.PR) / _states.Count;
    }
    public int GetTotalPop()
    {
      return _states.Sum(x => x.Pop);
    }
    public List<State> GetStatesSortedOnName()
    {
      return _states.OrderBy(x => x.Name).ToList();
    }
    public List<State> GetStatesSortedOnReverseName()
    {
      List<State> states = _states.OrderBy(x => x.Name).ToList();
      states.Reverse();
      return states;
    }

    public List<State> GetStatesSortedOnPop()
    {
      return _states.OrderBy(x => x.Pop).ToList();
    }
    public List<State> GetStatesSortedOnReversePop()
    {
      List<State> states = _states.OrderBy(x => x.Pop).ToList();
      states.Reverse();
      return states;
    }

    public double StandardDeviation()
    {
      double avg = _states.Average( v => v.PR );
      return Math.Sqrt(_states.Average(v => Math.Pow(v.PR - avg, 2)));
    }

    public State? LargestDeviant()
    {
      double std = StandardDeviation();
      var avg = GetPRAvg();
      return _states.MaxBy(v => Math.Abs((avg - v.PR) / std));
    }

    public double LargestDeviation()
    {
      double std = StandardDeviation();
      var avg = GetPRAvg();
      return _states.Max(v => Math.Abs((avg - v.PR) / std));
    }

    public List<State> GetStatesSortedOnPri()
    {
      return _states.OrderBy(x => x.Pri).ToList();
    }
    public List<State> GetStatesSortedOnReversePri()
    {
      List<State> states = _states.OrderBy(x => x.Pri).ToList();
      states.Reverse();
      return states;
    }

    public IEnumerator<State> GetEnumerator()
    {
      return ((IEnumerable<State>)_states).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)_states).GetEnumerator();
    }
  }

}
