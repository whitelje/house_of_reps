// <copyright file="Result.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps
{
  internal class Result
  {
    private readonly string party;

    public Result(string party, int votes)
    {
      this.party = party;
      this.Votes = votes;
      this.Reps = 1;
    }

    public Result(string party)
    {
      this.party = party;
      this.Reps = 0;
    }

    public int Votes { get; set; }

    public int Reps { get; set; }

    public double Pri => this.Votes / Math.Sqrt((this.Reps + 1) * this.Reps);

    public string Party
    {
      get
      {
        return this.party;
      }
    }

    public void AddRep()
    {
      this.Reps++;
    }
  }
}