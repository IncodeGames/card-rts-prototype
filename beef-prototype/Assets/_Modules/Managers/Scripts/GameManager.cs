using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance = null;
        public static GameManager Instance { get { return _instance; } }

        public enum GameState
        {
            STARTUP = 0,
            STRATEGY = 1,
            DRAW = 2,
            BATTLE = 3,
            GAME_END = 4,
        }

        private GameState currentGameState = GameState.STRATEGY;
        public GameState CurrentGameState { get { return currentGameState; } }

        public List<ITickable> simulationTickables = new List<ITickable>();

        public float TimeScale { get; set; }
        public float Time { get; private set; }
        public float TickTime { get; private set; }

        public float BattleElapsed { get { return BATTLE_DURATION - (TickTime - lastBattleTickElapsed); } }

        private const float BATTLE_DURATION = 10.0f;
        private float lastBattleTickElapsed = 0.0f;


        private PlayerStatus playerStatus;

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
        }

        void Start()
        {
            TimeScale = 1.0f;

            playerStatus.currentEnergy = 1;
        }

        public void BeginBattle()
        {
            Debug.Assert(currentGameState != GameState.BATTLE);
            currentGameState = GameState.BATTLE;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentGameState = GameState.BATTLE;
            }

            Time += UnityEngine.Time.unscaledDeltaTime * TimeScale;

            if (currentGameState == GameState.BATTLE)
            {
                TickTime += UnityEngine.Time.unscaledDeltaTime * TimeScale;

                if (TickTime > lastBattleTickElapsed + BATTLE_DURATION)
                {
                    currentGameState = GameState.STRATEGY;
                    playerStatus.currentEnergy += 3;

                    lastBattleTickElapsed = TickTime;
                }
            }

            for (int i = 0; i < simulationTickables.Count; ++i)
            {
                simulationTickables[i].Tick();
            }
        }
    }
}