// <copyright file="State.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps
{
  using System;
  using System.Globalization;
  using System.Text;

  internal class State
  {
    private readonly int expected;
    private readonly List<Result> results;

    /// <summary>
    /// Initializes a new instance of the <see cref="State"/> class.
    /// </summary>
    /// <param name="name">Name of State.</param>
    /// <param name="pop">Population of State.</param>
    /// <param name="expected">Expected number of reps.</param>
    public State(string name, int pop, int expected)
    {
      this.Name = name;
      this.Pop = pop;
      this.Reps = 1;
      this.expected = expected;
      this.results = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="State"/> class.
    /// </summary>
    /// <param name="name">Name of State.</param>
    /// <param name="pop">Population of State.</param>
    public State(string name, int pop)
    {
      this.Name = name.ToUpper();
      this.Pop = pop;
      this.Reps = 1;
      this.expected = 0;
      this.results = [];
    }

    public string Name { get; }

    public int Pop { get; }

    public int Reps { get; private set; }

    public int PR => this.Pop / this.Reps;

    public double Pri => this.Pop / Math.Sqrt((this.Reps + 1) * this.Reps);

    public List<Result> Results => this.results;

    public int RepsAssigned => this.Results.Sum(x => x.Reps);

    public List<Result> GetResultsSortedOnReversePri()
    {
      return this.results.OrderBy(x => x.Pri).Reverse().ToList();
    }

    public void AddRep()
    {
      this.Reps++;
    }

    /* Prints _name */
    public string PrintStatus(float avg, double std)
    {
      StringBuilder sb = new StringBuilder();
      NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
      nfi.NumberDecimalDigits = 0;
      return sb.AppendFormat("{0,-15}", this.Name)
        .Append('\t')
        .AppendFormat("{0,-10:N}", this.PR.ToString("N", nfi))
        .AppendFormat("{0,11:N} ", ((int)(avg - this.PR)).ToString("N", nfi))
        .AppendFormat("{0,11:F4}", (avg - this.PR) / std)
        .AppendFormat("{0, 5}", this.Reps)
        .AppendFormat("{0, 6}", this.Reps == this.expected)
        .ToString();
    }
  }
}
