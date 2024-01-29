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
            DRAW = 1,
            STRATEGY = 2,
            BATTLE = 3,
            GAME_END = 4,
        }

        private GameState currentGameState = GameState.STARTUP;
        public GameState CurrentGameState { get { return currentGameState; } }

        public List<ITickable> simulationTickables = new List<ITickable>();

        public float TimeScale { get; set; }
        public float Time { get; private set; }
        public float DeltaTime { get; private set; }
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

            DeckManager.Instance.CreateDeck();
            playerStatus.currentEnergy = 1;
        }

        public void BeginBattle()
        {
            Debug.Assert(currentGameState != GameState.BATTLE);
            currentGameState = GameState.BATTLE;
        }

        public void OnDrawComplete()
        {
            currentGameState = GameState.STRATEGY;
        }

        void Update()
        {
            if (currentGameState == GameState.STARTUP && Time > 3.0f)
            {
                currentGameState = GameState.DRAW;
                DeckManager.Instance.DrawCards();
            }

            Time += UnityEngine.Time.unscaledDeltaTime * TimeScale;
            DeltaTime = UnityEngine.Time.unscaledDeltaTime * TimeScale;

            if (currentGameState == GameState.BATTLE)
            {
                TickTime += UnityEngine.Time.unscaledDeltaTime * TimeScale;

                if (TickTime > lastBattleTickElapsed + BATTLE_DURATION)
                {
                    currentGameState = GameState.DRAW;
                    DeckManager.Instance.DrawCards();
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