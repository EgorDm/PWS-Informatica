using System.Collections;
using System.Collections.Generic;
using PWS.Entities;
using PWS.Input;
using UnityEngine;
using UnityEngine.Networking;

namespace PWS.Managers
{
    //TODO: Merely a placeholder for upcoming gamemodes
    public enum GameMode
    {
        FreeForAll,
        TeamDeathMatch
    }

    //TODO: A placeholder for game states
    public enum GameState
    {
        WaitingForPlayers,
        Started,
        Finished
    }

    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        private static List<NetworkCharacter> _players = new List<NetworkCharacter>();
            //TODO: Change to correct player type

        public CameraManager CameraManager { get; private set; }
        //TODO: A level manager which creates, deletes characters and assigns spawnpoints.
        //TODO: A GUI manager which handles GUI, thus all the messages

        [HideInInspector] [SyncVar] public GameState GameState = GameState.WaitingForPlayers;

        public InputController InputController { get; private set; }

        private void Awake()
        {
            Instance = this;
            InputController = GetComponent<InputController>();
            CameraManager = GetComponent<CameraManager>();
            CameraManager.Camera.Controller = InputController;
        }

        [ServerCallback]
        private void Start()
        {
            StartCoroutine(GameLoop());
        }

        private IEnumerator GameLoop()
        {
            if (_players.Count < 2)
            {
                //TODO: Stop the game?
            }

            yield return StartCoroutine(StartGame());

            yield return StartCoroutine(UpdateGame());

            yield return StartCoroutine(EndGame());

            StartCoroutine(GameLoop()); //Restart the game
        }

        private IEnumerator StartGame()
        {
            RpcGameStarting();

            yield return 1.0f; //TODO: Countdown or smt like that
        }

        [ClientRpc]
        private void RpcGameStarting()
        {
            ResetAllPlayers();
        }

        private IEnumerator UpdateGame()
        {
            RpcGameUpdate();

            while (GameState != GameState.Finished)
            {
                //TODO: Use other checks to determine if the game has ended?
                yield return null;
            }
        }

        [ClientRpc]
        private void RpcGameUpdate()
        {
            //TODO: Enable movement/controls whatever
        }

        private IEnumerator EndGame()
        {
            RpcRoundEnding();
            yield return 1.0f;
        }

        [ClientRpc]
        private void RpcRoundEnding()
        {
            //TODO: Show user game ended bla bla bla stats etc.
        }

        private void ResetAllPlayers()
        {
            foreach (var player in _players)
            {
                player.Reset();
                if (player.isLocalPlayer)
                {
                    player.InputController = InputController;
                    CameraManager.Camera.Target = player.transform;
                }
            }
        }

        public static void AddPlayer(GameObject character, string playerName)
        {
            var netChar = character.GetComponent<NetworkCharacter>();
            netChar.PlayerName = playerName;
            _players.Add(netChar);
        }

        public static void RemovePlayer(NetworkCharacter character)
        {
            if (_players.Contains(character))
            {
                _players.Remove(character);
            }
        }
    }
}