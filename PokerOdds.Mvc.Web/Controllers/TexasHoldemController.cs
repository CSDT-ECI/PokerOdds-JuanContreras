using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using HoldemHand;
using PokerOdds.Mvc.Web.Models.TexasHoldem;

namespace PokerOdds.Mvc.Web.Controllers
{
    public class TexasHoldemController : ApiController
    {
        int stopWatchSeconds = 10;
        int cardsNumber = 7;
        int twoCards = 2;
        int fiveCards = 5;
        long winner = 1.0;
        long HalfCreditsTies = 0.5;
        long Percentage = 100.0;

        [OutputCache]
        public TexasHoldemOdds Get(string pocket, string board)
        {
            try
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
                IterateOpponentHands(partialBoard, playerMask, playerWins, opponentWins, ref count, ref stopWatch);

                stopWatch.Stop();

                var outcomes = new List<PokerOutcome>();
                CreateOutcomes(playerWins, count, outcomes);

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
            catch (Exception ex)
            {
                // Agrega el manejo de la excepción según tus necesidades (puedes loggearla, retornar un mensaje de error, etc.)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        private void IterateOpponentHands(ulong partialBoard, ulong playerMask, double[] playerWins, double[] opponentWins, ref long count, ref Stopwatch stopWatch)
        {
            foreach (ulong opponentMask in Hand.Hands(0UL, partialBoard | playerMask, twoCards).AsParallel())
            {
                // Iterate through all possible boards
                IterateBoards(partialBoard, playerMask, playerWins, opponentWins, ref count, ref stopWatch, opponentMask);
                if (stopWatch.Elapsed > TimeSpan.FromSeconds(stopWatchSeconds))
                    break;
            }
        }

        private void IterateBoards(ulong partialBoard, ulong playerMask, double[] playerWins, double[] opponentWins, ref long count, ref Stopwatch stopWatch, ulong opponentMask)
        {
            foreach (ulong boardMask in Hand.Hands(partialBoard, opponentMask | playerMask, fiveCards).AsParallel())
            {
                EvaluateHands(playerMask, opponentMask, playerWins, opponentWins, ref count, ref stopWatch, boardMask);
                if (stopWatch.Elapsed > TimeSpan.FromSeconds(stopWatchSeconds))
                    break;
            }
        }

        private void EvaluateHands(ulong playerMask, ulong opponentMask, double[] playerWins, double[] opponentWins, ref long count, ref Stopwatch stopWatch, ulong boardMask)
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
                playerWins[Hand.HandType(playerHandValue)] += HalfCreditsTies;
                opponentWins[Hand.HandType(opponentHandValue)] += HalfCreditsTies;
            }
            count++;
        }

        private void CreateOutcomes(double[] playerWins, long count, List<PokerOutcome> outcomes)
        {
            for (var index = 0; index < playerWins.Length; index++)
            {
                outcomes.Add(new PokerOutcome
                {
                    HandType = (Hand.HandTypes)index,
                    HandTypeName = ((Hand.HandTypes)index).ToString(),
                    WinPercentage = playerWins[index] / ((double)count) * Percentage
                });
            }
        }
    }
}
