using System;

namespace GildedRose
{
    public class MinMaxAssessment : IQualityAssessment
    {
        private readonly int _min;
        private readonly int _max;

        public MinMaxAssessment(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public int AssessQuality(in int currentQuality, in int currentSellIn)
        {
            if(currentQuality > _max)
                return _max;

            if(currentQuality < _min)
                return _min;

            return currentQuality;
        }
    }
}