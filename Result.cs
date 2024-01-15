// <copyright file="Result.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps
{
  internal class Result
  {
    private readonly string party;

    public Result(string party, double reps)
    {
      this.party = party;
      this.Reps = reps;
    }

    public Result(string party)
    {
      this.party = party;
      this.Reps = 0;
    }

    public double Reps { get; set; }

    public string Party
    {
      get
      {
        return this.party;
      }
    }
  }
}