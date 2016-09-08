using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace PWS.Managers
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        [SyncVar(hook = "OnMyName")]
        public string PlayerName = "";

        public void OnMyName(string newName)
        {
            PlayerName = newName;
        }

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();
            SendReadyToBeginMessage();
        }
    }
}
