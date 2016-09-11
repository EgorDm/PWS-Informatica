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
        public Rigidbody Body { get; set; }
        public TPCamera Camera { get; set; }

        [SyncVar] public float Walk = 0.0f;
        [SyncVar] public float Turn = 0.0f;

        public float JumpVel = 4500f;
        public float JumpDirectionalMultip = 0.5f;
        public float GroundMargin = 1f;
        public float GroundCheckMargin = 1f;

        private Vector3 _startJumpVel = Vector3.zero;
        private float _rotationTime = 0f;

        public bool Grounded { get; protected set; }

        private void Start()
        {
            Anim = GetComponent<Animator>();
            Body = GetComponent<Rigidbody>();
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
            UpdateGrounded();
            if (isLocalPlayer)
            {
                if (InputController == null)
                {
                    return;
                }

                InputController.UpdateInput();

                var speed = InputController.IsRunning ? 1f : 0.5f;
                Turn = InputController.Horizontal;
                Turn = Mathf.Clamp(Mathf.Clamp(InputController.MouseMovement.x, -0.5f, 0.5f) + Turn * speed, -1f, 1f);
                Walk = InputController.Vertical * speed;
                CmdSyncMove(Turn, Walk);
                if (Grounded)
                {
                    if (Camera != null)
                    {
                        var newRot = transform.eulerAngles;
                        newRot.y = Camera.transform.eulerAngles.y;
                        transform.eulerAngles = newRot;

                    }

                    Anim.applyRootMotion = true;
                    if (InputController.Jump)
                    {
                        if (!Anim.GetBool("jump"))
                        {
                            _startJumpVel = Body.velocity;
                        }
                    }
                    Anim.SetBool("jump", InputController.Jump);
                }
            }
            UpdateMove();
        }

        public void UpdateMove()
        {
            Anim.SetFloat("turn", Turn, DampTime, DampDeltaTime);
            Anim.SetFloat("walk", Walk, DampTime, DampDeltaTime);
           /* AnimatorStateInfo currentState = Anim.GetCurrentAnimatorStateInfo(0);
            float playbackTime = currentState.normalizedTime%1;*/
        }

        [Command]
        void CmdSyncMove(float turn, float walk)
        {
            Turn = turn;
            Walk = walk;
        }

        public void ApplyJump()
        {
            Anim.applyRootMotion = false;
            var newVel = _startJumpVel * JumpDirectionalMultip;
            newVel.y = JumpVel;
            Body.velocity += newVel;
            Debug.Log(Body.velocity);
        }

        public void UpdateGrounded()
        {
            var checkPos = new Vector3(transform.position.x, transform.position.y + GroundCheckMargin, transform.position.z);
            Grounded = Physics.Raycast(checkPos, -Vector3.up, GroundMargin + 0.1f);
            Anim.SetBool("grounded", Grounded);
        }

        
    }
}
