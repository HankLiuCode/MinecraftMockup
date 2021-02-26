using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attempt1
{
    public class Player : MonoBehaviour
    {
        CharacterController controller;
        Vector3 velocity;
        float gravity = -9.81f;
        bool isGrounded;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        public float jumpHeight = 5f;
        public float speed = 1f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            bool jump = Input.GetButtonDown("Jump");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            if (jump && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

}