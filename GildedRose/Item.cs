using System;

namespace GildedRose
{
    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }

        public IQualityAssessment QualityAssesser { get; init; }

        public void UpdateQuality()
        {
            if(QualityAssesser == null)
                throw new NullReferenceException("QualityAssesser is not set!");

            Quality = QualityAssesser.AssessQuality(Quality, SellIn);
            SellIn--;
        }
    }
}