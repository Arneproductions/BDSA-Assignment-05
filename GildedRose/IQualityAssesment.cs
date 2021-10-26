using System;

namespace GildedRose
{
    /// <summary>
    /// Defines operations to assess the quality of an item
    /// </summary>
    public interface IQualityAssessment
    {
        /// <summary>
        /// Assess quality of an item based on the current sellIn number and quality
        /// </summary>
        /// <param name="currentQuality">The current quality number</param>
        /// <param name="currentSellIn">The current sellIn</param>
        /// <returns>Returns the quality that the assesment thinks it should be</returns>
        int AssessQuality(in int currentQuality, in int currentSellIn);
    }
}