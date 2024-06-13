// <copyright file="Program.cs" company="Jake Whiteley">
// Copyright (c) Jake Whiteley. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HouseOfReps // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Console.SetWindowSize(80, 54);
            List<CensusElectionYears> ceyears =
            [
                new CensusElectionYears(1970, [1976, 1980]), new CensusElectionYears(1980, [1984, 1988]),
                new CensusElectionYears(1990, [1992, 1996, 2000]), new CensusElectionYears(2000, [2004, 2008]),
                new CensusElectionYears(2010, [2012, 2016, 2020])
            ];

            foreach (var ceyear in ceyears)
            {
                foreach (var electionYear in ceyear.electionYears)
                {
                    string censusFileName = ceyear.censusYear + "_census.csv";
                    var states = new States(censusFileName);
                    // states.WriteFullStatus();
                    Console.WriteLine("======== " + electionYear + " ========");
                    states.WriteConciseStatus();
                    Console.WriteLine("Electoral College");
                    WriteElection(new Election("1976-2020-president_nodc.csv", electionYear, states)
                        .ComputeElectoralCollege(states));
                    Console.WriteLine();
                    Console.WriteLine("House of Reps");
                    WriteElection(
                        new Election("1976-2020-president_nodc.csv", electionYear, states).ComputeHouseOfReps(states));
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }

        internal struct CensusElectionYears
        {
            internal CensusElectionYears(int censusYear, int[] eY)
            {
                this.censusYear = censusYear;
                this.electionYears = eY.ToList();
            }

            internal int censusYear { get; }

            internal List<int> electionYears { get; }
        }

        internal static void WriteElection(Dictionary<string, int> results)
        {
            int totalReps = 0;
            foreach (KeyValuePair<string, int> res in results)
            {
                Console.WriteLine("{0}: {1}", res.Key, res.Value);
                totalReps += res.Value;
            }

            Console.WriteLine("Total Reps: {0}", totalReps);
        }
    }
}