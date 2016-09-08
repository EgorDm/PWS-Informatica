using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWS.Input;
using PWS.Managers;
using UnityEngine;
using UnityEngine.Networking;

namespace PWS.Entities
{
    public enum CharacterType
    {
        Medieval_Medium
    }

    public class NetworkCharacter : NetworkBehaviour
    {
        private const float DampTime = 5f;
        private const float DampDeltaTime = 0.5f;

        [SyncVar]
        public string PlayerName;

        [SyncVar]
        public CharacterType CharacterType;

        public Animator Anim { get; set; }

        public CharacterController CharController { get; set; }

        public InputController InputController { get; set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!isServer)
            {
                GameManager.AddPlayer(gameObject, PlayerName);
            }
        }

        private void Awake()
        {
            Anim = GetComponent<Animator>();
            CharController = GetComponent<CharacterController>();
        }

        public void Initialize(LobbyPlayer player)
        {
            PlayerName = player.PlayerName;
        }

        public void Reset()
        {
           
        }

        private void Update()
        {
            UpdateInput();
        }

        public void UpdateInput()
        {
            if (!isLocalPlayer || InputController == null)
            {
                return;
            }
            
            var speed = InputController.IsRunning ? 1f : 0.5f;
            var turn = InputController.Horizontal;
            turn = Mathf.Clamp(Mathf.Clamp(InputController.MouseMovement.x, -0.5f, 0.5f) + turn * speed, -1f, 1f);
            var walk = InputController.Vertical * speed;
            Move(turn, walk);
        }

        public void Move(float turn, float walk)
        {
            Anim.SetFloat("turn", turn, DampTime, DampDeltaTime);
            Anim.SetFloat("walk", walk, DampTime, DampDeltaTime);
        }

        public override void OnNetworkDestroy()
        {
            base.OnNetworkDestroy();
        }
    }
}
