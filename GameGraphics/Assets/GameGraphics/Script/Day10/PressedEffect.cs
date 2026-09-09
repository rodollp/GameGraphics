using UnityEngine;
using UnityEngine.InputSystem;

public class PressedEffect : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private EffectPool effectPool;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float spawnInterval = 0.1f;

    private Vector2 pointerPosition;
    private bool isPressed;
    private float spawnTimer;

    public void OnPoint(InputValue value)
    {
        pointerPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            isPressed = true;
            spawnTimer = spawnInterval;
        }
        else
        {
            isPressed = false;
        }
    }
    private void Update()
    {
        if (!isPressed)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;

        Ray ray = targetCamera.ScreenPointToRay(pointerPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            effectPool.PlayEffect(hit.point);
        }
    }
}
