using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zad4lab4_camera : MonoBehaviour
{
    public Transform player;  // Odniesienie do obiektu gracza
    public float sensitivity = 200f;
    public float maxLookAngle = 80f;
    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Zablokowanie kursora
    }

    void Update()
    {
        float mouseXMove = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseYMove = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        player.Rotate(Vector3.up * mouseXMove);

        // Obrót postaci wokół osi Y
        verticalRotation -= mouseYMove;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

        // Obrót kamery w osi X z ograniczeniem
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);;
    }
}
