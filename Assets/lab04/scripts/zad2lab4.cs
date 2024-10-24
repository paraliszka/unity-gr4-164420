using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zad2lab4 : MonoBehaviour
{
 public float speed = 6.0f;  
    public float gravity = -9.81f;   
    public float jumpHeight = 1.5f;    

    private CharacterController controller;
    private Vector3 velocity;    
    private bool isGrounded;   

    public Transform groundCheck;
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
