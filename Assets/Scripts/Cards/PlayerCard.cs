﻿using System;
using Cards.Data;
using UnityEngine;

namespace Cards
{
    public class PlayerCard : Card
    {
        [SerializeField] private PlayerCardData playerCardData;

        public int x = -1000;
        public int y = -1000;

        protected override void Awake()
        {
            base.Awake();

            x = -1000;
            y = -1000;

            Init(playerCardData, 0, false);

            Show(false, true);
        }

        public void SetPosition(int x, int y, Vector3 pos)
        {
            this.x = x;
            this.y = y;
            SetPosition(pos);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public override void Init(CardData data, int cn, bool destroyable = true)
        {
            cardData = data;

            canBeDestroyed = false;
            stats.attack = cardData.attack;
            stats.armor = cardData.armor;
            stats.health = cardData.health;

            cardDisplay.Init();
            cardDisplay.ShowFields(true, true, true);
        }
    }
}