using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class LightningClickController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private VisualEffect lightningVFX;
    [SerializeField] private ShieldTarget shieldTarget;
    [SerializeField] private LayerMask shieldLayer;

    private Vector2 mousePosition;

    public void OnPoint(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed)
            return;

        Ray ray = targetCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, shieldLayer))
        {
            lightningVFX.transform.position = hit.point;

            lightningVFX.Reinit();
            lightningVFX.Play();

            shieldTarget.TakeDamage(25f);
        }
    }
}