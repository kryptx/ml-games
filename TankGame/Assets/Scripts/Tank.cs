using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moltensoft.TankGame {
    public class Tank : MonoBehaviour
    {
        public const float maxSpeed = 30.0f;
        public const float acceleration = 10.0f;
        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }
        public float CurrentSpeed { get; private set; }

        private void Start()
        {
            
        }

        public void Fire() {
            GetComponentInChildren<Cannon>().Fire();
        }

        void Update()
        {
            float rotate = HorizontalInput;
            float move = VerticalInput;
            var rigidbody = GetComponent<Rigidbody>();
            CurrentSpeed = Vector3.Dot(rigidbody.velocity, -transform.right);
            var currentSpeedMagnitude = rigidbody.velocity.magnitude;
            var targetSpeed = 0f;

            if (move != 0) {
                if (currentSpeedMagnitude > maxSpeed) {
                    targetSpeed = CurrentSpeed;
                } else{
                    targetSpeed = maxSpeed * Mathf.Sign(move);
                }
            }

            // get the portion of transform.left (my model is sideways) that is in the x/z plane
            Vector3 forwardNoFlying = Vector3.ProjectOnPlane(-transform.right, Vector3.up);
            if (targetSpeed < CurrentSpeed) {
                
                rigidbody.AddForce(-acceleration * forwardNoFlying, ForceMode.Acceleration);
                if (move < 0) {
                    rigidbody.AddForce(-acceleration * forwardNoFlying, ForceMode.Acceleration);
                }
            }
            
            if (targetSpeed > CurrentSpeed) {
                // apply a positive force to the rigidbody
                rigidbody.AddForce(acceleration * forwardNoFlying, ForceMode.Acceleration);
                if (move > 0) {
                    rigidbody.AddForce(acceleration * forwardNoFlying, ForceMode.Acceleration);
                }
            }
        
            var rotationAmount = rotate * Time.deltaTime * 250;
            // if we are already rotating too much, do nothing
            if (Mathf.Abs(rigidbody.angularVelocity.y) < 2 && rotationAmount != 0) {
                rigidbody.AddTorque(0, rotationAmount, 0, ForceMode.Acceleration);
            } 
            
            if (Mathf.Abs(rigidbody.angularVelocity.y) > 0.1 && rotationAmount == 0) {
                rigidbody.AddTorque(0, -rigidbody.angularVelocity.y, 0, ForceMode.Acceleration);
            }
            

            // finally if we are moving to the side, apply a force to reduce the sliding
            if (Mathf.Abs(Vector3.Dot(rigidbody.velocity, transform.forward)) > 0.1f) {
                rigidbody.AddForce(-Vector3.Dot(rigidbody.velocity, transform.forward) * transform.forward, ForceMode.Acceleration);
            }
        }
    }
}
