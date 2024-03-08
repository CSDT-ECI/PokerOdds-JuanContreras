﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HoldemHand;
using PokerOdds.Mvc.Web.Models.TexasHoldem;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace PokerOdds.Mvc.Web.Controllers
{
    public class TexasHoldemController : ApiController
    {
        int stopWatchSeconds = 10;
        int cardsNumber = 7;
        long winner = 1.0;
        // GET api/TexasHoldem?pocket=as ks&board=Ts Qs 2d
        [OutputCache]
        public TexasHoldemOdds Get(string pocket, string board)
        {
            if (pocket == null)
                throw new ArgumentNullException("pocket");

            if (board == null || board.Equals("undefined", StringComparison.InvariantCultureIgnoreCase))
                board = string.Empty;

            ulong playerMask = Hand.ParseHand(pocket); // Player Pocket Cards
            ulong partialBoard = Hand.ParseHand(board);   // Partial Board

            // Calculate values for each hand type
            double[] playerWins = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] opponentWins = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            // Count of total hands examined.
            long count = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Iterate through all possible opponent hands
            foreach (ulong opponentMask in Hand.Hands(0UL, partialBoard | playerMask, 2).AsParallel())
            {
                // Iterate through all possible boards
                foreach (ulong boardMask in Hand.Hands(partialBoard, opponentMask | playerMask, 5).AsParallel())
                {
                    // Create a hand value for each player
                    uint playerHandValue = Hand.Evaluate(boardMask | playerMask, cardsNumber);
                    uint opponentHandValue = Hand.Evaluate(boardMask | opponentMask, cardsNumber);

                    // Calculate Winners
                    if (playerHandValue > opponentHandValue)
                    {
                        // Player Win
                        playerWins[Hand.HandType(playerHandValue)] += winner;
                    }
                    else if (playerHandValue < opponentHandValue)
                    {
                        // Opponent Win
                        opponentWins[Hand.HandType(opponentHandValue)] += winner;
                    }
                    else if (playerHandValue == opponentHandValue)
                    {
                        // Give half credit for ties.
                        playerWins[Hand.HandType(playerHandValue)] += 0.5;
                        opponentWins[Hand.HandType(opponentHandValue)] += 0.5;
                    }
                    count++;

                    if (stopWatch.Elapsed > TimeSpan.FromSeconds(stopWatchSeconds))
                        break;
                }

                if (stopWatch.Elapsed > TimeSpan.FromSeconds(stopWatchSeconds))
                    break;
            }

            stopWatch.Stop();

            var outcomes = new List<PokerOutcome>();

            for (var index = 0; index < playerWins.Length; index++)
            {
                outcomes.Add(new PokerOutcome
                {
                    HandType = (Hand.HandTypes)index,
                    HandTypeName = ((Hand.HandTypes)index).ToString(),
                    WinPercentage = playerWins[index] / ((double)count) * 100.0
                });
            }

            var results = new TexasHoldemOdds
            {
                Pocket = pocket,
                Board = board,
                Outcomes = outcomes.ToArray(),
                OverallWinSplitPercentage = outcomes.Sum(o => o.WinPercentage),
                CalculationTimeMS = stopWatch.ElapsedMilliseconds,
                Completed = stopWatch.Elapsed <= TimeSpan.FromSeconds(stopWatchSeconds)
            };

            return results;
        }
    }
}
