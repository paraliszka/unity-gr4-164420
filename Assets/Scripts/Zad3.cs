using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad3 : MonoBehaviour
{
public float speed = 5f; 
    private float sideLength = 10f;  
    private Vector3 startPosition; 
    private int cornerReached = 0;  
    private Quaternion targetRotation;

    void Start()
    {
        startPosition = transform.position;
        targetRotation = transform.rotation; 
    }

    void Update()
    {
        MoveAlongSquare();
        RotateAtCorner();
    }

    void MoveAlongSquare()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= sideLength)
        {
            startPosition = transform.position;
            cornerReached++;
            targetRotation *= Quaternion.Euler(0, 90, 0);
        }
    }

    void RotateAtCorner()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}
