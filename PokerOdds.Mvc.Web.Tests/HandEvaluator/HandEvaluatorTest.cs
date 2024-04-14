using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using HoldemHand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PokerOdds.Mvc.Web;
using PokerOdds.Mvc.Web.Controllers;

namespace PokerOdds.Mvc.Web.Tests.HandEvaluator
{
    [TestClass]
    public class HandEvaluatorTest
    {
        [TestMethod]
        public void TestHandEvaluatorWithRepeatedValues()
        {
            // Arrange
            var hand = "";
            //    new Card[]
            //{
            //new Card(Rank.Ace, Suit.Hearts),
            //new Card(Rank.Ace, Suit.Diamonds),
            //new Card(Rank.Queen, Suit.Spades),
            //new Card(Rank.Jack, Suit.Clubs),
            //new Card(Rank.Ten, Suit.Hearts)
            //};

            // Act
            var handRank = Hand.Evaluate(hand);

            // Assert
            Assert.Fail();
        }
    }
}
