// <copyright file="Election.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps
{
  using System;
  using System.Collections.Generic;
  using Microsoft.VisualBasic.FileIO;

  internal class Election
  {
    public Election(string file, int year, States states)
    {
      using TextFieldParser tfp = new(file);
      tfp.SetDelimiters(",");
      while (!tfp.EndOfData)
      {
        var fields = tfp.ReadFields();
        if (fields.Length < 15)
        {
          throw new FormatException("Not enough fields in line");
        }

        if (!int.TryParse(fields[0], out int parseYear))
        {
          throw new FormatException("Couldn't parse year: " + fields[0]);
        }

        if (parseYear != year)
        {
          continue;
        }

        if (!int.TryParse(fields[10], out int votes))
        {
          throw new FormatException("Couldn't parse votes: " + fields[10]);
        }

        if (!int.TryParse(fields[11], out int totalVotes))
        {
          throw new FormatException("Couldn't parse votes: " + fields[11]);
        }

/*    0  ,    1  ,    2     ,   3        ,    4      ,   5      ,   6    ,   7       ,      8         ,    9    ,     10         ,   11       ,   12    ,  13   ,   14    */
/* "year","state","state_po","state_fips","state_cen","state_ic","office","candidate","party_detailed","writein","candidatevotes","totalvotes","version","notes","party_simplified" */
        State? state = states[fields[1]];
        if (state == null)
        {
          throw new InvalidDataException("State name doesn't match any existing states");
        }

        Result? res = state.Results.Find(res => res.Party == fields[14]);
        if (res == null)
        {
          state.Results.Add(new Result(fields[14], (double)votes / (double)totalVotes));
        }
        else
        {
          res.Reps += votes / totalVotes;
        }
      }
    }

    public Dictionary<string, int> ComputeElectoralCollege(States states)
    {
      var electoralCollege = new Dictionary<string, int>();
      foreach (State st in states)
      {
        var result = st.Results.OrderBy(x => x.Reps).Reverse().ToList()[0];
        if (electoralCollege.ContainsKey(result.Party))
        {
          electoralCollege[result.Party] += st.Reps;
        }
        else
        {
          electoralCollege.Add(result.Party, st.Reps);
        }
      }

      return electoralCollege;
    }

    public Dictionary<string, int> ComputeHouseOfReps(States states)
    {
      var electoralCollege = new Dictionary<string, int>();
      foreach (State st in states)
      {
        foreach (Result res in st.Results)
        {
          if (electoralCollege.ContainsKey(res.Party))
          {
            electoralCollege[res.Party] += (int)(res.Reps * st.Reps);
          }
          else
          {
            electoralCollege.Add(res.Party, (int)(res.Reps * st.Reps));
          }
        }
      }

      return electoralCollege;
    }
  }
}
