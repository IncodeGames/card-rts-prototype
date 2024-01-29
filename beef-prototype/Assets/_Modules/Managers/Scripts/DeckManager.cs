using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Incode.Utils;
using System.Linq;

namespace Incode.Prototype
{
    public class DeckManager : MonoBehaviour
    {
        private static DeckManager _instance = null;
        public static DeckManager Instance { get { return _instance; } }

        private PlayerStatus playerStatus = null;
        private PlayerHUD playerHUD = null;

        [SerializeField] private CardEntity[] cardsInDeck = null;

        private Vector3 cardCachedPosition = Vector3.zero;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }

            ReferenceManager.Instance.TryGetReference<PlayerStatus>(out playerStatus);
            ReferenceManager.Instance.TryGetReference<PlayerHUD>(out playerHUD);
        }
        public void CreateDeck()
        {
            for (int i = 0; i < cardsInDeck.Length; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    CardEntity card = Instantiate(cardsInDeck[i]).GetComponent<CardEntity>();
                    card.gameObject.SetActive(false);
                    playerStatus.drawPile.Add(card);
                }
            }

            ShuffleDeck();
        }

        public void ShuffleDeck()
        {
            playerStatus.drawPile.Shuffle();
        }

        public void DrawCards()
        {
            if (playerStatus.currentHand.Count > 0)
            {
                DiscardHand();
            }

            //Refill deck if there aren't enough cards
            if (playerStatus.drawPile.Count < 5)
            {
                playerStatus.discardPile.Shuffle();
                playerStatus.drawPile.AddRange(playerStatus.discardPile);
                playerStatus.discardPile.Clear();
            }

            int drawCount = 0;
            for (int i = 0; i < 5; ++i)
            {
                drawCount++;
                playerStatus.currentHand.Add(playerStatus.drawPile[i]);
            }
            playerStatus.drawPile.RemoveRange(0, drawCount);

            playerHUD.DrawCardsAnimation();
        }

        public void DiscardCard(CardEntity card)
        {
            playerStatus.discardPile.Add(card);
            playerStatus.currentHand.Remove(card);

            playerHUD.DiscardCardsAnimation(card);
        }

        public void DiscardHand()
        {
            playerHUD.DiscardCardsAnimation(playerStatus.currentHand.ToArray());

            playerStatus.discardPile.AddRange(playerStatus.currentHand);
            playerStatus.currentHand.Clear();
        }

        public void MoveCardFromHand(CardEntity card)
        {
            cardCachedPosition = card.RectTransform.anchoredPosition;
        }

        public void RestoreCardToHand(CardEntity card)
        {
            playerHUD.RestoreCardToHandAnimation(card, cardCachedPosition);
        }
    }
}