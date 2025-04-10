using System;
using CustomerInteraction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveForce = 10f;
        public float bounceBackSpeed = 5f;
       
        private Vector2 _movementInput;
        private Vector2 _startPosition;
        private Rigidbody2D _rigidbody2D;
        private PlayerInputActions _playerInputActions;
        private Customer _customer;

        private bool _canMove = true;
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _startPosition = transform.position;
            
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerInputActions.Player.Move.canceled += ctx => _movementInput = Vector2.zero;
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Enable();
        }

        // Update is called once per frame
        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                _rigidbody2D.AddForce(_movementInput * moveForce, ForceMode2D.Impulse);    
            }

        }
        

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                StartCoroutine(BounceBack());
            }
        }

        System.Collections.IEnumerator BounceBack()
        {
            _canMove = false;
            _movementInput = Vector2.zero;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0f;
            
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = new Vector3(_startPosition.x, _startPosition.y, currentPosition.z);

            while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, bounceBackSpeed * Time.deltaTime);
                yield return null;
            }
            
            transform.position = targetPosition;
            _canMove = true;
        }
    }
}
