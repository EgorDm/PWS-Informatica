using System;
using PWS.Input;
using UnityEngine;

namespace PWS
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public InputController Controller { get; set; }
        public Camera MyCamera { get; set; }
        public Transform Target;
        public Vector3 TargetOffset = Vector3.zero;
        public float Distance = 4.0f;

        public LayerMask LineOfSightMask = 0;
        public float CloserRadius = 0.2f;
        public float CloserSnapLag = 0.2f;

        public float XSpeed = 200.0f;
        public float YSpeed = 80.0f;

        public int YMinLimit = -20;
        public int YMaxLimit = 80;

        private float _currentDistance = 10.0f;
        private float _x = 0.0f;
        private float _y = 0.0f;
        private float _distanceVelocity = 0.0f;

        private void Start()
        {
            var angles = transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;
            _currentDistance = Distance;

            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;

            //Controller = GetComponent<PlayerInputController>();
            MyCamera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (!Target) return;
            _x += Controller.MouseMovement.x* XSpeed*0.02f;
            _y -= Controller.MouseMovement.y* YSpeed*0.02f;

            _y = ClampAngle(_y, YMinLimit, YMaxLimit);

            var rotation = Quaternion.Euler(_y, _x, 0);
            var targetPos = Target.position + TargetOffset;
            var direction = rotation*-Vector3.forward;

            var targetDistance = AdjustLineOfSight(targetPos, direction);
            _currentDistance = Mathf.SmoothDamp(_currentDistance, targetDistance, ref _distanceVelocity,
                CloserSnapLag*.3f);

            transform.rotation = rotation;
            transform.position = targetPos + direction*_currentDistance;
        }

        private float AdjustLineOfSight(Vector3 target, Vector3 direction)
        {
            RaycastHit hit;
            if (Physics.Raycast(target, direction, out hit, Distance, LineOfSightMask.value))
                return hit.distance - CloserRadius;
            else
                return Distance;
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}