using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoldemHand;


namespace PokerOdds.HoldemOdds
{
    public class HoldemOddsCalculator
    {
        /// <summary>
        /// Calcula las probabilidades de Texas Hold'em para un conjunto de cartas dado.
        /// </summary>
        /// <param name="odds">El objeto TexasHoldemOdds que contiene la información de las cartas.</param>
        /// <param name="saveState">Una acción que se invoca para guardar el estado de las probabilidades calculadas.</param>
        /// <returns>Un Task que representa la operación asincrónica. El valor de la tarea es un objeto TexasHoldemOdds con las probabilidades calculadas.</returns>
        public async Task<TexasHoldemOdds> Calculate(TexasHoldemOdds odds, Action<TexasHoldemOdds> saveState)
        {
            InitializeOutcomes(odds);
            SaveStateIfNotNull(odds, saveState);

            return await Task<TexasHoldemOdds>.Run(() =>
            {
                ulong playerMask = Hand.ParseHand(odds.Pocket);     // Player Pocket Cards
                ulong partialBoard = Hand.ParseHand(odds.Board);    // Partial Board

                // Calculate values for each hand type
                double[] playerWins = new double[9];
                double[] opponentWins = new double[9];

                // Count of total hands examined.
                long count = 0;

                // Iterate through all possible opponent hands
                foreach (ulong opponentMask in Hand.Hands(0UL, partialBoard | playerMask, 2).AsParallel())
                {
                    // Iterate through all possible boards
                    foreach (ulong boardMask in Hand.Hands(partialBoard, opponentMask | playerMask, 5).AsParallel())
                    {
                        CalculateWins(playerMask, opponentMask, boardMask, playerWins, opponentWins, ref count);
                    }

                    UpdateWinChances(odds, playerWins, count);
                    SaveStateIfNotNull(odds, saveState);
                }

                FinalizeOddsCalculation(odds);
                SaveStateIfNotNull(odds, saveState);

                return odds;
            });
        }
        /// <summary>
        /// Inicializa los resultados posibles para las probabilidades de Texas Hold'em.
        /// </summary>
        /// <param name="odds">El objeto TexasHoldemOdds que contiene la información de las cartas.</param>
        private void InitializeOutcomes(TexasHoldemOdds odds)
        {
            var outcomes = Enum.GetValues(typeof(Hand.HandTypes))
                .Cast<Hand.HandTypes>()
                .Select(handType => new PokerOutcome { HandType = handType.ToString(), WinChance = 0 })
                .ToList();

            odds.Outcomes = outcomes;
        }
        /// <summary>
        /// Invoca la acción de guardar el estado si no es nula.
        /// </summary>
        /// <param name="odds">El objeto TexasHoldemOdds que contiene la información de las cartas.</param>
        /// <param name="saveState">Una acción que se invoca para guardar el estado de las probabilidades calculadas.</param>
        private void SaveStateIfNotNull(TexasHoldemOdds odds, Action<TexasHoldemOdds> saveState)
        {
            saveState?.Invoke(odds);
        }
        /// <summary>
        /// Calcula las victorias para el jugador y el oponente.
        /// </summary>
        /// <param name="playerMask">La máscara de bits que representa las cartas del jugador.</param>
        /// <param name="opponentMask">La máscara de bits que representa las cartas del oponente.</param>
        /// <param name="boardMask">La máscara de bits que representa las cartas en el tablero.</param>
        /// <param name="playerWins">Un array que contiene el número de victorias para cada tipo de mano del jugador.</param>
        /// <param name="opponentWins">Un array que contiene el número de victorias para cada tipo de mano del oponente.</param>
        /// <param name="count">El número total de manos examinadas.</param>
        private void CalculateWins(ulong playerMask, ulong opponentMask, ulong boardMask, double[] playerWins, double[] opponentWins, ref long count)
        {
            // Create a hand value for each player
            uint playerHandValue = Hand.Evaluate(boardMask | playerMask, 7);
            uint opponentHandValue = Hand.Evaluate(boardMask | opponentMask, 7);

            // Calculate Winners
            if (playerHandValue > opponentHandValue)
            {
                // Player Win
                playerWins[Hand.HandType(playerHandValue)] += 1.0;
            }
            else if (playerHandValue < opponentHandValue)
            {
                // Opponent Win
                opponentWins[Hand.HandType(opponentHandValue)] += 1.0;
            }
            else if (playerHandValue == opponentHandValue)
            {
                // Give half credit for ties.
                playerWins[Hand.HandType(playerHandValue)] += 0.5;
                opponentWins[Hand.HandType(opponentHandValue)] += 0.5;
            }

            count++;
        }
        /// <summary>
        /// Actualiza las probabilidades de victoria para cada tipo de mano.
        /// </summary>
        /// <param name="odds">El objeto TexasHoldemOdds que contiene la información de las cartas.</param>
        /// <param name="playerWins">Un array que contiene el número de victorias para cada tipo de mano del jugador.</param>
        /// <param name="count">El número total de manos examinadas.</param>
        private void UpdateWinChances(TexasHoldemOdds odds, double[] playerWins, long count)
        {
            for (var index = 0; index < playerWins.Length; index++)
            {
                var outcomes = odds.Outcomes.ToList();
                outcomes[index].WinChance = playerWins[index] / ((double)count);
            }
        }
        /// <summary>
        /// Finaliza el cálculo de las probabilidades de Texas Hold'em.
        /// </summary>
        /// <param name="odds">El objeto TexasHoldemOdds que contiene la información de las cartas.</param>
        private void FinalizeOddsCalculation(TexasHoldemOdds odds)
        {
            odds.OverallWinSplitChance = odds.Outcomes.Sum(o => o.WinChance);
            odds.Completed = true;
        }

        public static string SortCards(string cards)
        {
            var cardList = cards.Trim().ToLowerInvariant().Split(' ').ToList();

            cardList.Sort();

            return cardList.Aggregate(string.Empty, (a, b) => string.Format("{0} {1}", a.Trim(), b.Trim())).Trim();
        }
    }
}