using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zad2 : MonoBehaviour
{
 public float speed = 2f;
    private float startPosition;
    private bool movingForward = true;

    void Start()
    {
        startPosition = transform.position.x;
    }

    void Update()
    {
        float distance = transform.position.x - startPosition; 

        if (movingForward)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (distance > 10f) 
            {
                movingForward = false; 
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (distance <= 0f)
            {
                movingForward = true;
            }
        }
    }
}
