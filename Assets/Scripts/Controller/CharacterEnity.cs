using PWS.Input;
using UnityEngine;

namespace PWS.Entities
{
    public class CharacterEnity<T> : MonoBehaviour where T : InputController
    {
        public T Controller;
        public Animator Anim { get; set; }
        public CharacterController Character { get; set; }

        // Use this for initialization
        private void Start () {
            Anim = GetComponent<Animator>();
            Character = GetComponent<CharacterController>();
        }
	
        // Update is called once per frame
        private void Update () {
	        UpdateMovement();
        }

        protected virtual void UpdateMovement()
        {
            
        }
    }
}
