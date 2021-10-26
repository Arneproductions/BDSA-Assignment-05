using System;

namespace GildedRose.QualityAssessments
{
    public class MinMaxAssessment : IQualityAssessment
    {
        private readonly int _min;
        private readonly int _max;
        private readonly IQualityAssessment _qualityAssessment;

        public MinMaxAssessment(int min, int max, IQualityAssessment qualityAssessment = null)
        {
            _min = min;
            _max = max;
            _qualityAssessment = qualityAssessment;
        }

        public int AssessQuality(in int currentQuality, in int currentSellIn)
        {
            int quality = _qualityAssessment != null ? _qualityAssessment.AssessQuality(currentQuality, currentSellIn) : currentQuality;

            if(quality > _max)
                return _max;

            if(quality < _min)
                return _min;

            return quality;
        }
    }
}