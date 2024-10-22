using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zad6 : MonoBehaviour
{
    public Transform player; 
    public float smoothTime = 0.3f; 
    public float maxDistance = 3f; 
    public float maxSpeed = 10f;

    private Vector3 velocity = Vector3.zero; 

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > maxDistance)
            {
                Vector3 targetPosition = player.position;
                targetPosition.y = transform.position.y; 

                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, maxSpeed);
            }
        }
    }
}
