using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.QualityAssessments
{
    public class IncreaseAssessment : IQualityAssessment
    {
        private readonly IEnumerable<(int sellIn, int increaseAmount)> _increaseAmountRules;

        public IncreaseAssessment(params (int sellIn, int increaseAmount)[] increaseAmountRules)
        {
            _increaseAmountRules = increaseAmountRules.OrderBy(x => x.sellIn);
        }

        public int AssessQuality(in int currentQuality, in int currentSellIn)
        {
            foreach (var rule in _increaseAmountRules)
                if(rule.sellIn >= currentSellIn)
                    return currentQuality + rule.increaseAmount;

            return currentQuality;
        }
    }
}