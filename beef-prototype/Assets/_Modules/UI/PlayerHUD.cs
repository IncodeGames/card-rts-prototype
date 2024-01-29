using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.Utilities;

namespace Incode.Prototype
{
    public class PlayerHUD : MonoBehaviour
    {
        private int lastEnergyCount = 0;


        [SerializeField] private TextMeshProUGUI energyCountText = null;
        [SerializeField] private Button fightButton = null;
        [SerializeField] private TextMeshProUGUI fightButtonText = null;
        private PlayerStatus playerStatus = null;

        [SerializeField] private RectTransform HandLayoutTransform;
        [SerializeField] private RectTransform drawPileTransform;
        [SerializeField] private RectTransform discardPileTransform;

        private const float DISCARD_DURATION = 0.15f;
        private const float DRAW_DURATION = 0.25f;

        WaitForSeconds discardWait = new WaitForSeconds(DISCARD_DURATION);

        void Awake()
        {
            ReferenceManager.Instance.TryGetReference<PlayerStatus>(out playerStatus);
        }

        private IEnumerator DrawCardsAnimationRoutine()
        {
            yield return discardWait;

            float elapsedDuration = 0.0f;
            float lerpValue = 0.0f;
            for (int i = 0; i < playerStatus.currentHand.Count; ++i)
            {
                CardEntity card = playerStatus.currentHand[i];
                card.RectTransform.SetParent(HandLayoutTransform);
                card.transform.localScale = Vector3.one;
                card.RectTransform.anchorMin = Vector3.one * 0.5f;
                card.RectTransform.anchorMax = Vector3.one * 0.5f;
                card.gameObject.SetActive(true);

                while (lerpValue < 1.0f)
                {
                    elapsedDuration += GameManager.Instance.DeltaTime;
                    lerpValue = elapsedDuration / DRAW_DURATION;

                    Vector3 targetPosition = HandLayoutTransform.anchoredPosition + (Vector2.right * (card.RectTransform.rect.width + 8) * (i - (playerStatus.currentHand.Count / 2)));
                    card.RectTransform.anchoredPosition = Vector3.Lerp(drawPileTransform.anchoredPosition, targetPosition, lerpValue);
                    yield return null;
                }

                elapsedDuration = 0f;
                lerpValue = 0f;
            }
            GameManager.Instance.OnDrawComplete();
        }

        private IEnumerator DiscardCardsAnimationRoutine(params CardEntity[] cards)
        {
            float elapsedDuration = 0.0f;
            float lerpValue = 0.0f;

            for (int i = 0; i < cards.Length; ++i)
            {
                CardEntity card = cards[i];
                Vector3 startPosition = card.RectTransform.anchoredPosition;

                while (lerpValue < 1.0f)
                {
                    elapsedDuration += GameManager.Instance.DeltaTime;
                    lerpValue = elapsedDuration / DISCARD_DURATION;
                    card.RectTransform.anchoredPosition = Vector3.Lerp(startPosition, discardPileTransform.anchoredPosition, lerpValue);
                    yield return null;
                }

                elapsedDuration = 0f;
                lerpValue = 0f;

                card.gameObject.SetActive(false);
            }
        }

        private IEnumerator RestoreCardToHandRoutine(CardEntity card, Vector3 targetPosition)
        {
            float elapsedDuration = 0.0f;
            float lerpValue = 0.0f;
            Vector3 startPosition = card.RectTransform.anchoredPosition;

            while (lerpValue < 1.0f)
            {
                elapsedDuration += GameManager.Instance.DeltaTime;
                lerpValue = elapsedDuration / DISCARD_DURATION;
                card.RectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, lerpValue);
                yield return null;
            }
        }

        public void DrawCardsAnimation()
        {
            StartCoroutine(DrawCardsAnimationRoutine());
        }

        public void DiscardCardsAnimation(params CardEntity[] cards)
        {
            StartCoroutine(DiscardCardsAnimationRoutine(cards));
        }

        public void RestoreCardToHandAnimation(CardEntity card, Vector3 targetPosition)
        {
            StartCoroutine(RestoreCardToHandRoutine(card, targetPosition));
        }

        void Update()
        {
            if (lastEnergyCount != playerStatus.currentEnergy)
            {
                energyCountText.text = playerStatus.currentEnergy.ToString();
                lastEnergyCount = playerStatus.currentEnergy;
            }

            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BATTLE)
            {
                if (fightButton.interactable == true) { fightButton.interactable = false; }

                fightButtonText.text = Mathf.RoundToInt(GameManager.Instance.BattleElapsed).ToString();
            }
            else if (fightButton.interactable == false && GameManager.Instance.CurrentGameState != GameManager.GameState.BATTLE)
            {
                fightButton.interactable = true;
                fightButtonText.text = "Fight!";
            }
        }
    }
}
