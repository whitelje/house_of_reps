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
    private readonly SortedList<string, bool> elections = new(50);

    public Election(string file, int year, States states)
    {
      using TextFieldParser tfp = new(file);
      tfp.SetDelimiters(",");
      while (!tfp.EndOfData)
      {
        string stateName;
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

/* "year","state","state_po","state_fips","state_cen","state_ic","office","candidate","party_detailed","writein","candidatevotes","totalvotes","version","notes","party_simplified" */
      }
    }

    public Tuple<int, int> ComputeElectoralCollege(States states)
    {
      int dem = 0;
      int rep = 0;
      foreach (State st in states)
      {
        if (this.elections[st.Name])
        {
          dem += st.Reps + 2;
        }
        else
        {
          rep += st.Reps + 2;
        }
      }

      return new Tuple<int, int>(dem, rep);
    }
  }
}
