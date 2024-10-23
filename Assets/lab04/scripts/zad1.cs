using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class zad1 : MonoBehaviour
{
    public GameObject block; // Prefab obiektu
    public int numberOfObjects = 10; // Liczba obiektów do wygenerowania (ustawiane w inspektorze)
    public float delay = 3.0f; // Opóźnienie między generacjami
    public Material[] materials; // Lista materiałów do przypisania (ustawiane w inspektorze)

    private List<Vector3> positions = new List<Vector3>();
    private int objectCounter = 0;
    private Bounds platformBounds;

    void Start()
    {
        // Pobranie rozmiaru platformy, na której generowane są obiekty
        platformBounds = GetComponent<Renderer>().bounds;

        // Sprawdzenie, czy rozmiar platformy jest wystarczający
        if (platformBounds.size.x < 1 || platformBounds.size.z < 1)
        {
            Debug.LogError("Platforma jest za mała, aby wygenerować obiekty.");
            return;
        }

        // Losowanie pozycji x i z w oparciu o wielkość platformy
        for (int i = 0; i < numberOfObjects; i++)
        {
            float randomX = UnityEngine.Random.Range(platformBounds.min.x, platformBounds.max.x);
            float randomZ = UnityEngine.Random.Range(platformBounds.min.z, platformBounds.max.z);
            positions.Add(new Vector3(randomX, platformBounds.max.y, randomZ));
        }

        // Uruchomienie coroutine, aby generować obiekty z opóźnieniem
        StartCoroutine(GenerujObiekt());
    }

    IEnumerator GenerujObiekt()
    {
        Debug.Log("Wywołano coroutine");

        foreach (Vector3 pos in positions)
        {
            // Tworzenie obiektu w pozycji
            GameObject newBlock = Instantiate(block, pos, Quaternion.identity);

            // Losowe przypisanie materiału z listy
            if (materials.Length > 0)
            {
                Renderer renderer = newBlock.GetComponent<Renderer>();
                renderer.material = materials[UnityEngine.Random.Range(0, materials.Length)];
            }

            objectCounter++;
            yield return new WaitForSeconds(delay);
        }

        // Zakończenie coroutine
        StopCoroutine(GenerujObiekt());
    }
}
