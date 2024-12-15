using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer 
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;
        
        [Header("Collision Settings")]
        public float wallCheckDistance = 0.5f;
        public LayerMask wallLayer;
        
        [Header("Camera Settings")]
        public Transform cameraTransform;
        public float cameraDistance = 5f;
        public float cameraHeight = 2f;
        public float cameraLerpSpeed = 5f;
        
        private Rigidbody rb;
        private Vector3 movement;
        private Vector3 currentCameraPosition;
        private Vector3 lastMovementDirection;
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            SetupRigidbody();
            InitializeCamera();
            lastMovementDirection = transform.forward;
        }
        
        void SetupRigidbody()
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.freezeRotation = true;
        }
        
        void InitializeCamera()
        {
            if (cameraTransform != null)
            {
                UpdateCameraPosition();
                currentCameraPosition = cameraTransform.position;
            }
        }
        
        void Update()
        {
            ProcessInput();
            UpdateAnimation();
        }
        
        void ProcessInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 right = transform.right;
            Vector3 forward = transform.forward;
            
            movement = (right * horizontalInput + forward * verticalInput).normalized;

            if (movement.magnitude > 0.1f)
            {
                lastMovementDirection = movement;
            }
        }
        
        void UpdateAnimation()
        {
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetBool("isRunning", movement.magnitude > 0);
            }
        }
        
        void FixedUpdate()
        {
            if (movement.magnitude > 0)
            {
                MovePlayer();
                RotatePlayer();
            }
        }
        
        void MovePlayer()
        {
            Vector3 desiredMove = movement * moveSpeed * Time.fixedDeltaTime;
            Vector3 newPosition = rb.position + desiredMove;
            
            if (!WallCheck(desiredMove))
            {
                rb.MovePosition(newPosition);
            }
        }
        
        bool WallCheck(Vector3 moveDirection)
        {
            Vector3[] rayDirections = new Vector3[]
            {
                moveDirection.normalized,
                Quaternion.Euler(0, 30, 0) * moveDirection.normalized,
                Quaternion.Euler(0, -30, 0) * moveDirection.normalized
            };
            
            foreach (Vector3 direction in rayDirections)
            {
                Ray ray = new Ray(transform.position + Vector3.up * 0.5f, direction);
                Debug.DrawRay(ray.origin, ray.direction * wallCheckDistance, Color.red);
                
                if (Physics.Raycast(ray, wallCheckDistance, wallLayer))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        void RotatePlayer()
        {
            if (lastMovementDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lastMovementDirection);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        }
        
        void LateUpdate()
        {
            if (cameraTransform != null)
            {
                UpdateCameraPosition();
            }
        }
        
        void UpdateCameraPosition()
        {
            Vector3 targetPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out hit, 
                cameraDistance, wallLayer))
            {
                targetPosition = hit.point + (transform.position - hit.point).normalized * 0.5f;
            }
            
            currentCameraPosition = Vector3.Lerp(currentCameraPosition, targetPosition, 
                Time.deltaTime * cameraLerpSpeed);
            
            cameraTransform.position = currentCameraPosition;
            cameraTransform.LookAt(transform.position + Vector3.up);
        }
    }
}