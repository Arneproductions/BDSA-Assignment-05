﻿using GildedRose;
using System;
using System.Collections.Generic;
using Xunit;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        public static TheoryData<Item, int> QualityUpdateTestData => new TheoryData<Item, int> {
            { new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 }, 19 },
            { new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 }, 1 },
            { new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 }, 6 },
            { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 }, 80 },
            { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 }, 21 },
            { new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }, 5 }
        };

        private Program _p;
        private Item _vest;
        private Item _brie;
        private Item _elixir;
        private Item _sulfuras;
        private Item _backstagePass;
        private Item _conjured;

        public TestAssemblyTests()
        {
            _vest = new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 };
            _brie = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };
            _elixir = new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 };
            _sulfuras = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };
            _backstagePass = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };
            _conjured = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };

            _p = new Program();
            _p.Items = new List<Item>() { _vest, _brie, _elixir, _sulfuras, _backstagePass, _conjured };
        }

        [Theory]
        [MemberData(nameof(QualityUpdateTestData))]
        public void Quality_Is_Correct_After_1_Day(Item item, int expectedQuality)
        {
            _p.Items = new List<Item>() { item };
            _p.UpdateQuality();

            Assert.Equal(expectedQuality, item.Quality);
        }

        [Fact]
        public void Quality_Degrades_Twice_As_Fast_Past_Sell_By_Date()
        {
            int prevQuality = _vest.Quality;

            while (_vest.SellIn > 0)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality - 1, _vest.Quality);
                prevQuality = _vest.Quality;
            }

            while (_vest.Quality > 0)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality - 2, _vest.Quality);
                prevQuality = _vest.Quality;
            }
        }

        [Fact]
        public void Quality_Cannot_Be_Negative()
        {            
            for (int i = 0; i < 50; i++)
            {
                _p.UpdateQuality();

                foreach (var item in _p.Items)
                {
                    Assert.False(item.Quality < 0);
                }
            }
        }

        [Fact]
        public void Quality_Cannot_Be_Above_50_Unless_Legendary()
        {
            for (int i = 0; i < 50; i++)
            {
                _p.UpdateQuality();

                foreach (var item in _p.Items)
                {
                    if (item != _sulfuras)
                    {
                        Assert.False(item.Quality > 50);
                    }
                }
            }
        }

        [Fact]
        public void Sulfuras_Never_Has_To_Be_Sold_Or_Decreases_In_Value()
        {
            int startSellIn = _sulfuras.SellIn;
            int startQuality = _sulfuras.Quality;

            for (int i = 0; i < 50; i++)
            {
                _p.UpdateQuality();

                Assert.Equal(startSellIn, _sulfuras.SellIn);
                Assert.Equal(startQuality, _sulfuras.Quality);
            }
        }

        [Fact]
        public void Brie_Increases_In_Quality()
        {
            int prevQuality = _brie.Quality;

            // Brie increases in quality the older it gets
            while (_brie.SellIn > 0)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality + 1, _brie.Quality);
                prevQuality = _brie.Quality;
            }

            // Once the sell by date has passed, the quality of brie increases twice as fast
            while (_brie.Quality < 50)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality + 2, _brie.Quality);
                prevQuality = _brie.Quality;
            }
        }

        [Fact]
        public void Backstage_Passes_Increases_In_Quality_Until_The_Concert()
        {
            int prevQuality = _backstagePass.Quality;

            // The quality of backstage passes increases by 1 when there's more than 10 days left
            while (_backstagePass.SellIn > 10)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality + 1, _backstagePass.Quality);
                prevQuality = _backstagePass.Quality;
            }

            // The quality increases by 2 when there's between 6-10 days left
            while (_backstagePass.SellIn > 5)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality + 2, _backstagePass.Quality);
                prevQuality = _backstagePass.Quality;
            }

            // The quality increases by 3 when there's less than 5 days left
            while (_backstagePass.SellIn > 0)
            {
                _p.UpdateQuality();
                Assert.Equal(prevQuality + 3, _backstagePass.Quality);
                prevQuality = _backstagePass.Quality;
            }

            // The quality drops to 0 after the concert
            _p.UpdateQuality();
            Assert.Equal(0, _backstagePass.Quality);
        }
    }
}