using PWS.Input;
using UnityEngine;
using UnityEngine.Networking;

namespace PWS
{
    public class NetPlayer : NetworkBehaviour
    {
        public NetPlayerMovement PlayerMovement { get; set; }

        private void Awake()
        {
            PlayerMovement = GetComponent<NetPlayerMovement>();
        }

        private void Start ()
        {
            Initiate();
        }

        public void Initiate()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            var levelManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
            levelManager.Camera.Target = this;
        }

        // Update is called once per frame
        private void Update () {
           
        }

       
    }
}
