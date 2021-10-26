﻿using GildedRose;
using GildedRose.QualityAssessments;
using System.Collections.Generic;
using Xunit;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        public static TheoryData<Item, int> QualityUpdateTestData => new TheoryData<Item, int> {
            { new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, -1))) }, 19 },
            { new Item { Name = "Aged Brie", SellIn = 2, Quality = 0, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, 1), (0, 2))) }, 1 },
            { new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, -1))) }, 6 },
            { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80, QualityAssesser = new MinMaxAssessment(80, 80) }, 80 },
            { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((50, 1), (5, 3), (10, 2))) }, 21 },
            { new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, -1))) }, 5 }
        };

        private Item _vest;
        private Item _brie;
        private Item _elixir;
        private Item _sulfuras;
        private Item _backstagePass;
        private Item _conjured;

        public TestAssemblyTests()
        {
            _vest = new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, -1))) };
            _brie = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, 1), (0, 2))) };
            _elixir = new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, -1))) };
            _sulfuras = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80, QualityAssesser = new MinMaxAssessment(80, 80)};
            _backstagePass = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((50, 1), (5, 3), (10, 2))) };
            _conjured = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6, QualityAssesser = new MinMaxAssessment(0, 50, new IncreaseAssessment((int.MaxValue, -1))) };
        }

        [Theory]
        [MemberData(nameof(QualityUpdateTestData))]
        public void UpdateQuality_QualityIsCorrect(Item item, int expectedQuality)
        {
            Program p = new Program();
            p.Items = new List<Item>() { item };
            p.UpdateQuality();

            Assert.Equal(expectedQuality, item.Quality);
        }

        [Fact]
        public void UpdateQuality_DoesNotSetNegativeQuality()
        {
            Program p = new Program();
            p.Items = new List<Item>() { _vest, _elixir };
            
            for (int i = 0; i < 20; i++)
            {
                p.UpdateQuality();
            }

            foreach (var item in p.Items)
            {
                Assert.Equal(0, item.Quality);
            }
        }

        [Fact]
        public void UpdateQuality_DoesNotSetQualityAbove50()
        {
            Program p = new Program();
            p.Items = new List<Item>() { _brie, _backstagePass };

            for (int i = 0; i < 50; i++)
            {
                p.UpdateQuality();
            }

            foreach (var item in p.Items)
            {
                Assert.False(item.Quality > 50);
            }
        }

        [Fact]
        public void UpdateQuality_IncreasesBrieQuality()
        {
            Program p = new Program();
            p.Items = new List<Item>() { _brie };

            int prevQuality = _brie.Quality;

            // Brie increases in quality the older it gets
            while (_brie.SellIn > 0)
            {
                p.UpdateQuality();
                Assert.Equal(prevQuality + 1, _brie.Quality);
                prevQuality = _brie.Quality;
            }

            // Once the sell by date has passed, the quality of brie increases twice as fast
            for (int i = 0; i < 5; i++)
            {
                p.UpdateQuality();
                Assert.Equal(prevQuality + 2, _brie.Quality);
                prevQuality = _brie.Quality;
            }
        }

        [Fact]
        public void Sulfuras_Never_Has_To_Be_Sold_Or_Decrease_In_Value()
        {
            //Given
            Program p = new Program();
            _sulfuras.SellIn = 10;
            _sulfuras.Quality = 80;
            p.Items = new List<Item>() { _sulfuras };

            //When
            for (int i = 0; i < 8; i++)
            {
                p.UpdateQuality();
            }
            
            //Then
            foreach (var item in p.Items)
            {
                Assert.Equal(80, item.Quality);
            }
        }
    }
}