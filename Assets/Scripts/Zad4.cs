using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad4 : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 movement;

    private float minX = -10f, maxX = 10f, minZ = -10f, maxZ = 10f;
    public Transform cameraTransform; 
    public Vector3 cameraOffset = new Vector3(0, 10, -10);  

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical"); 
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical); 
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y,  
            Mathf.Clamp(transform.position.z, minZ, maxZ)
        );
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        cameraTransform.position = transform.position + cameraOffset;
        cameraTransform.LookAt(transform);
    }
}
