using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Data;
using Cards.DragDrop;
using Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour, IDropHandler
{
    [SerializeField] private List<Card> cards; // down-top
    [SerializeField] private float cardOffset = 0.05f;

    [Header("Prefabs and co.")] [SerializeField]
    private CardTypeGameObjectDictionary cardPrefabs = new CardTypeGameObjectDictionary();

    private CardList _availableCards;
    private int _startedWithCards = 1;
    private bool _hasDoor;

    public int InitWithCards(CardList availableCards, int numberCards, bool hasDoor)
    {
        _availableCards = availableCards;
        _startedWithCards = numberCards;
        _hasDoor = hasDoor;

        cards ??= new List<Card>();

        for (var i = 0; i < numberCards; i++)
        {
            CardType? type = i != 0 ? (CardType?) null : (hasDoor ? CardType.Door : CardType.Trap);
            var card = SpawnCard(i, type, i == 0);
            cards.Add(card);
        }


        return numberCards;
    }

    private Card SpawnCard(int number, CardType? type = null, bool destroyable = true)
    {
        var offset = number * cardOffset;

        CardType cardType;
        CardData cardVariant;
        if (type != null)
        {
            (cardType, cardVariant) = _availableCards.GetRandom();
        }
        else
        {
            cardType = CardTypes.FreelySpawnable.RandomEntry();
            cardVariant = _availableCards.GetRandomVariantForType(cardType);
        }

        var prefab = cardPrefabs[cardType];

        var cardGo = Instantiate(prefab, transform, false);
        var card = cardGo.GetComponent<Card>();
        card.Init(cardVariant, number + 1, destroyable);

        card.transform.position += new Vector3(-offset, -offset, 0);


        return card;
    }

    private void ReShuffle()
    {
        if (cards != null)
        {
            foreach (var card in cards)
            {
                Destroy(card.gameObject);
            }

            cards.Clear();
        }

        InitWithCards(_availableCards, _startedWithCards, _hasDoor);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedObj = eventData.pointerDrag;
        if (!draggedObj)
            return;

        var dndScript = draggedObj.GetComponent<DragDrop>();
        dndScript.SetValidDrop();
        
        var playerCard = draggedObj.GetComponent<PlayerCard>();

        if (!playerCard)
            return;

        var pos = GetTopCardPosition();
        playerCard.transform.position = pos;
        Debug.Log("Dropped " + playerCard.name);
    }

    private Vector3 GetTopCardPosition()
    {
        return !cards.Any() ? transform.position : cards.Last().transform.position;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Deck))]
    private class DeckEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target is Deck script))
                return;

            if (Application.isPlaying && GUILayout.Button("Reshuffle"))
            {
                script.ReShuffle();
            }
        }
    }
#endif
}