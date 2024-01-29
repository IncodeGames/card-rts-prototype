using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private RectTransform playArea = null;
        [SerializeField] private LayerMask buildableMask = default(LayerMask);

        private Camera mainCamera = null;
        private CardEntity selectedCard = null;
        private PlayerStatus playerStatus = null;

        void Awake()
        {
            mainCamera = Camera.main;
            ReferenceManager.Instance.TryGetReference<PlayerStatus>(out playerStatus);
        }

        public void OnPointerEnter(CardEntity entity)
        {
            entity.RectTransform.localScale = Vector3.one * 1.1f;
        }

        public void OnPointerExit(CardEntity entity)
        {
            entity.RectTransform.localScale = Vector3.one;
        }

        public void OnPointerDown(CardEntity entity)
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.DRAW && selectedCard == null)
            {
                selectedCard = entity;
                DeckManager.Instance.MoveCardFromHand(selectedCard);
            }
        }

        public void OnPointerUp(CardEntity entity)
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.STRATEGY)
            {
                bool inBounds = RectTransformUtility.RectangleContainsScreenPoint(playArea, (Vector2)entity.RectTransform.position);
                if (inBounds)
                {
                    if (playerStatus.currentEnergy >= entity.CardAction.EnergyCost)
                    {
                        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(mouseRay, out hit, 100f, buildableMask.value))
                        {
                            if (entity.CardAction.EnqueueAction(hit.point))
                            {
                                playerStatus.currentEnergy -= entity.CardAction.EnergyCost;
                                DeckManager.Instance.DiscardCard(entity);
                            }
                            else
                            {
                                DeckManager.Instance.RestoreCardToHand(entity);
                            }
                        }
                    }
                }
            }

            selectedCard = null;
        }

        public void OnPointerDrag(CardEntity entity)
        {
            entity.RectTransform.position = Input.mousePosition;
        }
    }
}
