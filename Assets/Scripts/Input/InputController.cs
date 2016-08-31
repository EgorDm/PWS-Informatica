using UnityEngine;

namespace PWS.Input
{
    public class InputController : MonoBehaviour
    {
        public Transform Target;

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
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, Target(x,y,z): ({1}, {2}, {3}), IsRunning: {4}, Jump: {5}, Horizontal: {6}, Vertical: {7}, Attack: {8}, Block: {9}",
                    base.ToString(), Target.position.x, Target.position.y, Target.position.z, IsRunning, Jump, Horizontal,
                    Vertical, Attack, Block);
        }
    }
}