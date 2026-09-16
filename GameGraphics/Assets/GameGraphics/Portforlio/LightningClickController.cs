using UnityEngine;
using UnityEngine.InputSystem;

public class LightningClickController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private ParticleSystem lightningEffect;
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
            lightningEffect.Play(true);

            shieldTarget.TakeDamage(25f);
        }
    }
}