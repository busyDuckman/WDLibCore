/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */


//TODO: I think this came from some academic work I did a while back. In any case it needs to be properly documented and
//checked to see If I still want it.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WDToolbox.Maths.Statistics;

namespace WDToolbox.Maths.Heuristics
{
    public class AdaptiveThresholdingSlidingWindow : DescriptiveStatisticsSlidingWindow
    {
        public enum Comparison { greaterThan, lessThan };

        public enum AdaptiveMetric {
            Average, //Median, 
            Smallest, Largest
        };

        public enum ParamatisedAdaptiveMetric
        {
            StandardDeviationsAboveMean,
            StandardDeviationsBelowMean,
            UperPercentiles,
            LowerPercentiles,
            StandardDeviationsAwayFromMean
        };

        public AdaptiveThresholdingSlidingWindow(int windowSize) : base(windowSize)
        {
        }

        public bool isValue(double value, Comparison comparison, AdaptiveMetric metric)
        {
            double threshold = calculateMetric(metric);
            return doComparison(value, comparison, threshold);
        }

        public bool isValue(double value, Comparison comparison, ParamatisedAdaptiveMetric metric, double n)
        {
            double threshold = calculateMetric(metric, n);
            switch (metric)
            {
                case ParamatisedAdaptiveMetric.StandardDeviationsAwayFromMean:
                    //make same distance above mean, if below
                    value = Mean + System.Math.Abs(value-Mean);
                    break;
            }
            return doComparison(value, comparison, threshold);
        }

        private static bool doComparison(double value, Comparison comparison, double threshold)
        {
            switch (comparison)
            {
                case Comparison.greaterThan:
                    return value > threshold;
                case Comparison.lessThan:
                    return value < threshold;
                default:
                    throw new InvalidOperationException("unknown comparison");
            }
        }

        public double calculateMetric(AdaptiveMetric metric)
        {
            switch (metric)
            {
                case AdaptiveMetric.Average:
                    return Mean;
                //case AdaptiveMetric.Median:
                //    break;
                case AdaptiveMetric.Smallest:
                    return Min;
                case AdaptiveMetric.Largest:
                    return Max;
                default:
                    throw new InvalidOperationException("unknown metric");
            }
        }

        public double calculateMetric(ParamatisedAdaptiveMetric metric, double n)
        {
            switch (metric)
            {
                case ParamatisedAdaptiveMetric.StandardDeviationsAwayFromMean:
                    return Mean + System.Math.Abs(StdDev * n);
                case ParamatisedAdaptiveMetric.StandardDeviationsAboveMean:
                    return Mean + (StdDev * n);
                case ParamatisedAdaptiveMetric.StandardDeviationsBelowMean:
                    return Mean - (StdDev * n);
                case ParamatisedAdaptiveMetric.UperPercentiles:
                    throw new NotImplementedException(); //todo
                case ParamatisedAdaptiveMetric.LowerPercentiles:
                    throw new NotImplementedException(); //todo
                default:
                    throw new InvalidOperationException("unknown metric");
            }
        }

    }
}
