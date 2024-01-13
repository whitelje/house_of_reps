using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace house_of_reps
{
  internal class Election
  {
    readonly SortedList<string, bool> _elections = new(50);
    public Election( string file, int year )
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
        if (parseYear != year) continue;
        if (!int.TryParse(fields[10], out int votes))
        {
          throw new FormatException("Couldn't parse votes: " + fields[10]);
        }

      }
    }

    public Tuple<int, int> ComputeElectoralCollege(States states)
    {
      int dem = 0;
      int rep = 0;
      foreach ( State st in states )
      {
        if (_elections[st.Name])
        {
          dem += st.Reps + 2;
        } else
        {
          rep += st.Reps + 2;
        }
      }

      return new Tuple<int, int>(dem, rep);
    }
  }
}
