using UnityEngine;
using UnityEngine.InputSystem;

namespace Day10
{
    public class ClickEffectSpawner : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private ParticleSystem effectPrefab;
        [SerializeField] private LayerMask groundMask;

        [SerializeField] private EffectPool effectPool;

        private Vector2 pointerPosition;

        public void OnPoint(InputValue value)
        {
            pointerPosition = value.Get<Vector2>();
        }

        public void OnClick(InputValue value)
        {
            if (!value.isPressed)
            {
                return;
            }

            Ray ray = targetCamera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                effectPool.PlayEffect(hit.point);
                
                
            }
        }
    }
}
