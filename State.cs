using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace house_of_reps
{
  internal class State
  {
    private readonly String _name;
    private readonly int _pop;
    private int _reps;
    private int _expected;

    public State(string name, int pop, int expected)
    {
      this._name = name;
      this._pop = pop;
      this._reps = 1;
      this._expected = expected;
    }
    public State(string name, int pop )
    {
      this._name = name;
      this._pop = pop;
      this._reps = 1;
      this._expected = 0;
    }
    public String Name { get { return _name; } }
    public int Pop { get { return _pop; } }
    public int Reps { get { return _reps; } }
    public int PR { get { return _pop / _reps; } }
    public double Pri {  get { return _pop / Math.Sqrt((Reps + 1) * (Reps)); } }

    public void AddRep()
    {
      _reps++;
    }

    /* Prints _name */
    public string PrintStatus(float avg, double std)
    {
      StringBuilder sb = new StringBuilder();
      NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
      nfi.NumberDecimalDigits = 0;
      return sb.AppendFormat("{0,-15}", Name)
        .Append('\t')
        .AppendFormat("{0,-10:N}", PR.ToString("N",nfi))
        .AppendFormat("{0,11:N} ", ((int)(avg - PR)).ToString("N",nfi))
        .AppendFormat("{0,11:F4}", (avg - PR) / std)
        .AppendFormat("{0, 5}", Reps)
        .AppendFormat("{0, 6}", _reps == _expected)
        .ToString();
    }
  }
}
