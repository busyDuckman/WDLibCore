/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: I have made a better version of this in the Java port, need update this.

namespace WDToolbox.Maths.Statistics
{
    /// <summary>
    /// Note, accumulated errors are going to be an issue 
    /// </summary>
    public sealed class DescriptiveStatisticsOnTheFly
    {
        public long Count { get; protected set; }
        public double Min { get; protected set; }
        public double Max { get; protected set; }
        public double StandardDeviation { get; protected set; }
        public double Mean { get; protected set; }
        public double Sum { get; protected set; }
        public double SumSqares { get; protected set; }
        public long PositiveCount { get; protected set; }
        public long NegativeCount { get; protected set; }
        public long ZeroCount { get; protected set; }
        public string Name { get; set; }

        public DescriptiveStatisticsOnTheFly(string name)
        {
            Count = 0;
            StandardDeviation = 0;
            Min = double.MaxValue;
            Max = double.MinValue;
            Name = name;
        }

        public void Next(double value)
        {
            Count++;
            Min = Math.Min(Min, value);
            Max = Math.Max(Max, value);
            Sum += value;
            Mean = Sum / Count;
            SumSqares += (Count * Count);
            
            //TODO: I think this should allow for a continiously updated standard dfeviation, need to test
            StandardDeviation = Math.Sqrt(((Count * SumSqares) - (Sum * Sum))) / Count;

            if (value > 0)
            {
                PositiveCount++;
            }
            else if(value < 0)
            {
                NegativeCount++;
            }
            else
            {
                ZeroCount++;
            }
        }

        public static double getStandardDeviation(List<double> doubleList)
        {
            double average = doubleList.Average();
            double sumOfDerivation = 0;
            doubleList.ForEach(v => sumOfDerivation += Math.Pow(v - average, 2));
            double sumOfDerivationAverage = sumOfDerivation / doubleList.Count;
            return Math.Sqrt(sumOfDerivationAverage);
        }
    }
}
