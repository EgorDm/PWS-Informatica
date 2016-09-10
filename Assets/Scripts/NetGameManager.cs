using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PWS.Managers
{
    //Warning: most is run host only!
    public class NetGameManager : NetworkManager
    {
        public List<NetPlayer> Players = new List<NetPlayer>();

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            var netPlayer = player.GetComponent<NetPlayer>();
            Players.Add(netPlayer);
        }


    }
}
