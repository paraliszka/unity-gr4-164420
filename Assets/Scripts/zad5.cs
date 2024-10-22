using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zad5 : MonoBehaviour
{
 public GameObject cubePrefab;
    public int numberOfCubes = 10;
    public float planeSize = 10f;

    private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>(); 

    void Start()
    {
        SpawnRandomCubes();
    }

    void SpawnRandomCubes()
    {
        int cubesSpawned = 0;

        while (cubesSpawned < numberOfCubes)
        {
            float randomX = Random.Range(-planeSize / 2f, planeSize / 2f);
            float randomZ = Random.Range(-planeSize / 2f, planeSize / 2f);

            Vector2 position2D = new Vector2(Mathf.Round(randomX), Mathf.Round(randomZ));

            if (!occupiedPositions.Contains(position2D))
            {
                occupiedPositions.Add(position2D);

                Vector3 spawnPosition = new Vector3(position2D.x, 2f, position2D.y);

                Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

                cubesSpawned++;
            }
        }
    }
}
