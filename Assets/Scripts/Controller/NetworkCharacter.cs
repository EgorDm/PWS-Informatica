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
        public float turn;
        [SyncVar]
        public float walk;

        [SyncVar]
        public CharacterType CharacterType;

        public Animator Anim { get; set; }

        public CharacterController CharController { get; set; }

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

        public void Setup()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            CameraManager.Instance.Camera.Target = transform;
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
            if (!isLocalPlayer)
            {
                Move(turn, walk);
                return;
            }

            if (InputController.Instance == null)
            {
                return;
            }
            
            var speed = InputController.Instance.IsRunning ? 1f : 0.5f;
            turn = InputController.Instance.Horizontal;
            turn = Mathf.Clamp(Mathf.Clamp(InputController.Instance.MouseMovement.x, -0.5f, 0.5f) + turn * speed, -1f, 1f);
            walk = InputController.Instance.Vertical * speed; //Remove old shit egor pls

            if (CameraManager.Instance != null)
            {
                transform.eulerAngles = new Vector3(0, CameraManager.Instance.Camera.transform.eulerAngles.y, 0);
            }
           
            Vector3 moveDirection = moveDirection = new Vector3(turn, 0, walk);
            moveDirection = transform.TransformDirection(moveDirection);
            CharController.SimpleMove(moveDirection * 40f);
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
