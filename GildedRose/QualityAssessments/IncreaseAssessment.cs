using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.QualityAssessments
{
    public class IncreaseAssessment : IQualityAssessment
    {
        private readonly IEnumerable<(int quality, int increaseAmount)> _increaseAmountRules;

        public IncreaseAssessment(params (int quality, int increaseAmount)[] increaseAmountRules)
        {
            _increaseAmountRules = increaseAmountRules.OrderBy(x => x.quality);
        }

        public int AssessQuality(in int currentQuality, in int currentSellIn)
        {
            foreach (var rule in _increaseAmountRules)
                if(rule.quality > currentQuality)
                    return currentQuality + rule.increaseAmount;

            return currentQuality;
        }
    }
}