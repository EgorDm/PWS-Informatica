using UnityEngine;
using System.Collections;
using PWS.Input;
using UnityEditor.AnimatedValues;
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

        public float WalkSpeed = 80f;

        public float JumpVel = 500f;
        public float GroundMargin = 1f;
        public float GroundCheckMargin = 1f;

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
                Move();

                CmdSyncMove(Turn, Walk);
                if (Grounded)
                {
                    if (Camera != null)
                    {
                        var newRot = transform.eulerAngles;
                        newRot.y = Camera.transform.eulerAngles.y;
                        transform.eulerAngles = newRot;

                    }
                    Anim.SetBool("jump", InputController.Jump);
                }
            }
            UpdateMoveAnim();
        }

        public void UpdateMoveAnim()
        {
            Anim.SetFloat("turn", Turn, DampTime, DampDeltaTime);
            Anim.SetFloat("walk", Walk, DampTime, DampDeltaTime);
        }

        public void Move()
        {
            if (!Grounded)
                return;

            var speed = InputController.IsRunning ? 1f : 0.5f;
            Walk = InputController.Vertical*speed;
            Turn = InputController.Horizontal*speed;
           
            Body.velocity = transform.TransformDirection(new Vector3(Turn * WalkSpeed, 0,  Walk * WalkSpeed));
        }

        [Command]
        void CmdSyncMove(float turn, float walk)
        {
            Turn = turn;
            Walk = walk;
        }

        public void ApplyJump()
        {
            Body.AddForce(new Vector3(0, JumpVel, 0), ForceMode.Impulse);
        }

        public void UpdateGrounded()
        {
            var checkPos = new Vector3(transform.position.x, transform.position.y + GroundCheckMargin, transform.position.z);
            Grounded = Physics.Raycast(checkPos, -Vector3.up, GroundMargin + 0.1f);
            Anim.SetBool("grounded", Grounded);
        }

        
    }
}
