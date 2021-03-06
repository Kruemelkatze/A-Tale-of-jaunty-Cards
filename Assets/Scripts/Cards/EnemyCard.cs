using System;
using Logics;

namespace Cards
{
    public class EnemyCard : Card
    {
        public FightResult FightTurn()
        {
            var statsPlayer = GameController.Instance.playerCard.stats;
            var enemyStats = stats;
            FightResult result;

            (statsPlayer, enemyStats, result) = Fight.FightTurn(statsPlayer, enemyStats);

            UpdateStats(enemyStats);
            GameController.Instance.playerCard.UpdateStats(statsPlayer);

            return result;
        }

        public override (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            visited++;

            var result = FightTurn();

            (bool, bool) output;
            switch (result)
            {
                case FightResult.PlayerWon:
                    output = (true, canBeDestroyed);
                    break;
                case FightResult.EnemyWon:
                case FightResult.Draw:
                default:
                    output = (false, false);
                    break;
            }

            switch (result)
            {
                case FightResult.Draw:
                    AudioController.Instance.PlayRandomSound("attack");
                    break;
                case FightResult.PlayerWon:
                    PlayAudioLine();
                    break;
                case FightResult.EnemyWon:
                    //AudioController.Instance.PlayRandomSound("player-die");
                    break;
            }

            if (result == FightResult.PlayerWon && canBeDestroyed)
            {
                CardSolved(true);
            }

            return output;
        }

        public override void Init(Deck deck, CardData data, int cn, bool destroyable = true)
        {
            base.Init(deck, data, cn, destroyable);
            cardDisplay.ShowFields(true, true, true);
        }

        public override (Stats ownStats, Stats playerStats) GetPreviewStats()
        {
            var statsPlayer = GameController.Instance.playerCard.stats;
            var enemyStats = stats;
            FightResult result;

            (statsPlayer, enemyStats, result) = Fight.FightTurn(statsPlayer, enemyStats);

            return (enemyStats, statsPlayer);
        }
    }
}