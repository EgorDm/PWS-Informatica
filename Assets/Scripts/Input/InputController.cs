using UnityEngine;

namespace PWS.Input
{
    public class InputController : MonoBehaviour
    {
        public Vector2 MouseMovement { get; set; }

        public InputController()
        {
            Vertical = 0.0f;
            Horizontal = 0.0f;
            Jump = false;
            IsRunning = false;
            Attack = false;
            Block = false;
        }

        public bool IsRunning { get; protected set; }

        public bool Jump { get; set; }

        public float Horizontal { get; protected set; }

        public float Vertical { get; protected set; }

        public bool Attack { get; protected set; }

        public bool Block { get; protected set; }

        protected virtual void Start()
        {
        }

        private void Update()
        {
            UpdateInput();
        }

        protected virtual void UpdateInput()
        {
            Horizontal = UnityEngine.Input.GetAxis("Horizontal");
            Vertical = UnityEngine.Input.GetAxis("Vertical");
            Jump = UnityEngine.Input.GetButton("Jump");
            Attack = UnityEngine.Input.GetButtonDown("Attack");
            Block = UnityEngine.Input.GetButton("Block");
            IsRunning = UnityEngine.Input.GetButton("Run");
            MouseMovement = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, IsRunning: {4}, Jump: {5}, Horizontal: {6}, Vertical: {7}, Attack: {8}, Block: {9}",
                    base.ToString(), IsRunning, Jump, Horizontal,
                    Vertical, Attack, Block);
        }
    }
}