using UnityEngine;

namespace PWS.Input
{
    public class PlayerInputController : InputController
    {
        public Vector2 MouseMovement { get; set; }
        public ThirdPersonCamera CameraController { get; set; }


        public PlayerInputController()
        {
        }

        protected override void Start()
        {
            base.Start();
            CameraController = GetComponent<ThirdPersonCamera>();
        }

        protected override void UpdateInput()
        {
            Horizontal = UnityEngine.Input.GetAxis("Horizontal");
            Vertical = UnityEngine.Input.GetAxis("Vertical");
            Jump = UnityEngine.Input.GetButton("Jump");
            Attack = UnityEngine.Input.GetButtonDown("Attack");
            Block = UnityEngine.Input.GetButton("Block");
            IsRunning = UnityEngine.Input.GetButton("Run");
            MouseMovement = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        }
    }
}