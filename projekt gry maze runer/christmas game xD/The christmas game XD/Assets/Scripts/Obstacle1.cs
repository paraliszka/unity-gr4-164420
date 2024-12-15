using System.Collections;
using UnityEngine;

namespace Platformer
{
    public class Obstacle1 : MonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed = 3f;
        [SerializeField]
        private float _rotationSpeed = 40f;
        [SerializeField]
        private float _movementRange = 1f;

        private Vector3 _initialPosition;
        private bool _movingUp = true;

        private GameManager _gameManager;

        void Start()
        {
            _initialPosition = transform.position;
            _gameManager = FindObjectOfType<GameManager>(); 
        }

        void Update()
        {
            if (_movingUp)
            {
                transform.position += Vector3.up * _movementSpeed * Time.deltaTime;
                if (transform.position.y >= _initialPosition.y + _movementRange)
                    _movingUp = false;
            }
            else
            {
                transform.position -= Vector3.up * _movementSpeed * Time.deltaTime;
                if (transform.position.y <= _initialPosition.y - _movementRange)
                    _movingUp = true;
            }

            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<GameManager>().EndGame(false);
            }
        }
    }
}
