using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.QualityAssessments
{
    public class IncreaseAssessment : IQualityAssessment
    {
        private readonly IQualityAssessment _qualityAssessment;
        private readonly IEnumerable<(int sellIn, int increaseAmount)> _increaseAmountRules;

        public IncreaseAssessment(IQualityAssessment qualityAssessment, params (int sellIn, int increaseAmount)[] increaseAmountRules)
        {
            _qualityAssessment = qualityAssessment;
            _increaseAmountRules = increaseAmountRules.OrderBy(x => x.sellIn);
        }

        public IncreaseAssessment(params (int sellIn, int increaseAmount)[] increaseAmountRules)
            : this(null, increaseAmountRules)
        {
            
        }

        public int AssessQuality(in int currentQuality, in int currentSellIn)
        {
            int quality = _qualityAssessment != null ? _qualityAssessment.AssessQuality(currentQuality, currentSellIn) : currentQuality;

            foreach (var rule in _increaseAmountRules)
                if(rule.sellIn >= currentSellIn)
                    return quality + rule.increaseAmount;

            return quality;
        }
    }
}