using UnityEngine;
using UnityEngine.InputSystem;

namespace Day10
{
    public class ClickEffectSpawner : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private EffectPool effectPool;

        [Header("Hold Settings")]
        [SerializeField] private float holdDelay = 0.2f;
        [SerializeField] private float spawnInterval = 0.1f;

        private Vector2 pointerPosition;

        private bool isPressed;
        private bool isHolding;

        private float holdTimer;
        private float spawnTimer;

        public void OnPoint(InputAction.CallbackContext context)
        {
            pointerPosition = context.ReadValue<Vector2>();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isPressed = true;
                isHolding = false;

                holdTimer = 0f;
                spawnTimer = 0f;

                SpawnEffect();
            }

            if (context.canceled)
            {
                isPressed = false;
                isHolding = false;

                holdTimer = 0f;
                spawnTimer = 0f;
            }
        }

        private void Update()
        {
            if (!isPressed)
                return;

            holdTimer += Time.deltaTime;

            // 아직 꾹 누르기로 판단하지 않음 
            if (!isHolding)
            {
                if (holdTimer >= holdDelay)
                {
                    isHolding = true;
                    spawnTimer = 0f;
                }

                return;
            }

            // 꾹 누르는 중 
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEffect();
            }
        }

        private void SpawnEffect()
        {
            Ray ray = targetCamera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                effectPool.PlayEffect(hit.point);
            }
        }
    }
} 