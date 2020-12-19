using Extensions;
using UnityEngine;

namespace Cards
{
    public class DoorCard : Card
    {
        public override (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            visited++;
            AddPointsAfterSolved();
            GameController.Instance.DoorReached(deck.x, deck.y);

            return (true, false);
        }
    }
}