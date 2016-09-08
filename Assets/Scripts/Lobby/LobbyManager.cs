using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace PWS.Managers
{
    public class LobbyManager : NetworkLobbyManager
    {
        public static LobbyManager Instance { get; private set; }

        private const string LocalHost = "127.0.0.1";

        public string NetworkAddress;

        protected bool DisconnectServer = false;

        protected ulong CurrentMatchId;

        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //StartCoroutine(ConnectDefault());
        }

        //TODO: For testing purposes. Tries to connect to localhost. If error then create a new match.
        private IEnumerator ConnectDefault()
        {
            //networkAddress = LocalHost;
            //var myClient = StartClient();
            //yield return new WaitForSeconds(5.0f);
            var myClient = StartHost();
            yield return null;
            if (!myClient.isConnected)
            {
                Debug.Log("No local host found. Creating a new one.");
                myClient.Disconnect();
                myClient = StartHost();
            }
            
        }

        public void AddLocalPlayer()
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            player.RemovePlayer();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            NetworkAddress = networkAddress;
        }

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            CurrentMatchId = (ulong) matchInfo.networkId;
        }

        public override void OnDestroyMatch(bool success, string extendedInfo)
        {
            base.OnDestroyMatch(success, extendedInfo);
            if (!DisconnectServer) return;
            StopMatchMaker();
            StopHost();
        }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            var obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
            return obj;
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            if (lobbyPlayer == null)
                return false;

            var lp = lobbyPlayer.GetComponent<LobbyPlayer>();
            if (lp == null)
                return false;

            GameManager.AddPlayer(gamePlayer, lp.PlayerName);
            return true;
        }



        public override void OnLobbyClientAddPlayerFailed()
        {
            base.OnLobbyClientAddPlayerFailed();
            Debug.Log("What?");
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            Debug.Log("Connected to " + networkAddress);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            Debug.Log("Disconnected from " + networkAddress);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            base.OnClientError(conn, errorCode);
            Debug.Log("Client error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()));
        }
    }
}