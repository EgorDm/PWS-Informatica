using PWS.Input;
using UnityEngine;
using UnityEngine.Networking;

namespace PWS.Entities
{
    public class PlayerEnity : CharacterEnity<PlayerInputController>
    {
        private const float DampTime = 5f;
        private const float DampDeltaTime = 0.5f;

        public const float AttackTime = 0.02f;

        private bool _attackStarted = false;
        private float _attackTimePassed= 0f;

        protected override void UpdateMovement()
        {
            base.UpdateMovement();
            transform.eulerAngles = new Vector3(0, Controller.CameraController.MyCamera.transform.eulerAngles.y, 0);
            var right = transform.right;

            var hor = Controller.Horizontal;
            var ver = Controller.Vertical;
            var speed = Controller.IsRunning ? 1f : 0.5f;

            hor = Mathf.Clamp(Mathf.Clamp(Controller.MouseMovement.x, -0.5f, 0.5f) + hor*speed, -1f, 1f);

            Anim.SetFloat("turn", hor, DampTime, DampDeltaTime);
            Anim.SetFloat("walk", ver*speed, DampTime, DampDeltaTime);


            Anim.SetBool("attack", false);
            if (Controller.Attack && !_attackStarted)
            {
                _attackStarted = true;
                _attackTimePassed = 0.0f;
                Anim.SetFloat("attack_angle", 0);
            }
            if (_attackStarted)
            {
                if (_attackTimePassed > AttackTime)
                {
                    _attackStarted = false;
                    Anim.SetBool("attack", true);
                    Anim.SetFloat("attack_angle", VectorAngle(Controller.MouseMovement.normalized));
                }
                else
                {
                    _attackTimePassed += Time.deltaTime;
                }
            }
            
        }

        private static float VectorAngle(Vector2 vector)
        {
            var ret = Mathf.Rad2Deg*Mathf.Atan(vector.x/vector.y);
            return float.IsNaN(ret) ? 0f : ((vector.y >= 0f) ? ret + 180f : ret);
        }
    }
}