using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomerInteraction
{
    public enum Direction { Up, Down, Left, Right }

    public class OrderInputHandler : MonoBehaviour
    {
        public CustomerSpawner spawner;
        private readonly List<Direction> _currentInput = new List<Direction>();
        private readonly float _inputTimeout = 2f;
        private float _inputTimer = 0f;
        private PlayerInputActions _playerInputActions;

        void Awake()
        {
            _playerInputActions = new PlayerInputActions();
        }

        void OnEnable()
        {
            _playerInputActions.Gameplay.Enable();
            _playerInputActions.Gameplay.Move.performed += OnMove;
        }

        void OnDisable()
        {
            _playerInputActions.Gameplay.Move.performed -= OnMove;
            _playerInputActions.Gameplay.Disable();
        }

        void Update()
        {
            _inputTimer += Time.deltaTime;

            if (_inputTimer >= _inputTimeout)
            {
                _currentInput.Clear();
                _inputTimer = 0f;
            }

        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            Vector2 input = context.ReadValue<Vector2>();
        
            if (input.y > 0.5f) AddInput(Direction.Up);
            else if (input.y < -0.5f) AddInput(Direction.Down);
            else if (input.x > 0.5f) AddInput(Direction.Right);
            else if (input.x < -0.5f) AddInput(Direction.Left);
        }

        void AddInput(Direction direction)
        {
            _currentInput.Add(direction);
            _inputTimer = 0f;

            Customer front = spawner.GetFrontCustomer();
            if (front == null) return;

            if (_currentInput.Count == front.orderCombo.Count)
            {
                if (IsCorrectCombo(_currentInput, front.orderCombo))
                {
                    spawner.FulfiillFrontCustomer();
                }
                _currentInput.Clear();
            }
        }

        bool IsCorrectCombo(List<Direction> input, List<Direction> correct)
        {
            if (input.Count != correct.Count) return false;
            for (int i = 0; i < input.Count; i++)
            {
                if(input[i] != correct[i]) return false;
            }
            return true;
        }


    }
}