using UnityEngine;
using System.Collections;
using PWS.Input;
using UnityEngine.Networking;

namespace PWS
{
    public class NetPlayerMovement : NetworkBehaviour
    {
        private const float DampTime = 5f;
        private const float DampDeltaTime = 0.5f;

        public InputController InputController { get; set; }
        public Animator Anim { get; set; }

        [SyncVar] public float Walk = 0.0f;
        [SyncVar] public float Turn = 0.0f;

        private void Start()
        {
            Anim = GetComponent<Animator>();
            Initiate();
        }

        public void Initiate()
        {
            if (!isLocalPlayer)
            {
                Anim.applyRootMotion = false;
                return;
            }

            if (InputController == null)
            {
                InputController = new InputController();
            }
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                UpdateMove();
                return;
            }

            if (InputController == null)
            {
                return;
            }

            InputController.UpdateInput();
            var speed = InputController.IsRunning ? 1f : 0.5f;
            Turn = InputController.Horizontal;
            Turn = Mathf.Clamp(Mathf.Clamp(InputController.MouseMovement.x, -0.5f, 0.5f) + Turn*speed, -1f, 1f);
            Walk = InputController.Vertical*speed;
            CmdSyncMove(Turn, Walk);
            UpdateMove();
        }

        public void UpdateMove()
        {
            Anim.SetFloat("turn", Turn, DampTime, DampDeltaTime);
            Anim.SetFloat("walk", Walk, DampTime, DampDeltaTime);
            AnimatorStateInfo currentState = Anim.GetCurrentAnimatorStateInfo(0);
            float playbackTime = currentState.normalizedTime%1;
            //Debug.Log(playbackTime);
            //Anim.
        }

        [Command]
        void CmdSyncMove(float turn, float walk)
        {
            Turn = turn;
            Walk = walk;
        }
    }
}
