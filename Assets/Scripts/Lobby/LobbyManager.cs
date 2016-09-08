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

        protected ulong CurrentMatchId;

        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            CurrentMatchId = (ulong) matchInfo.networkId;
        }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
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


    }
}