using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Award : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<GameManager>().EndGame(true);
            }
        }
    }
}
