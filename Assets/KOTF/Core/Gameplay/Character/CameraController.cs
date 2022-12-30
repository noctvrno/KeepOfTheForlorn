using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _rotationXSpeed = 1.0f;
        [SerializeField] private float _rotationYSpeed = 1.0f;
        [SerializeField] private float _rotationXMin = 0.0f;
        [SerializeField] private float _rotationXMax = 360.0f;
        private Vector2 _rotation = Vector2.zero;

        private void Start()
        {
            if (!TryGetComponent<CharacterBase>(out _))
                Debug.LogError($"{gameObject.name} is not attached to an {nameof(CharacterBase)}.");
        }

        private void Update()
        {
            Look();
        }

        private void Look()
        {
            // The mouse moves in a 2D space.
            // Translating the mouse on the X axis will result in a rotation along the Y axis.
            _rotation.y += UnityEngine.InputSystem.Mouse.current.delta.x.ReadValue() * _rotationXSpeed;

            // Translating the mouse on the Y axis will result in a rotation along the X axis.
            _rotation.x -= UnityEngine.InputSystem.Mouse.current.delta.y.ReadValue() * _rotationYSpeed;
            _rotation.x = Mathf.Clamp(_rotation.x, _rotationXMin, _rotationXMax);

            // Apply the rotation.
            transform.eulerAngles = _rotation;
        }
    }
}
