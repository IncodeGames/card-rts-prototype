using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Incode.Prototype
{
    public class PlayerHUD : MonoBehaviour
    {
        private int lastEnergyCount = 0;


        [SerializeField] private TextMeshProUGUI energyCountText = null;
        [SerializeField] private Button fightButton = null;
        [SerializeField] private TextMeshProUGUI fightButtonText = null;
        private PlayerStatus playerStatus = null;

        void Awake()
        {
            ReferenceManager.Instance.TryGetReference<PlayerStatus>(out playerStatus);
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
