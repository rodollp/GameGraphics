using UnityEngine;
using UnityEngine.InputSystem;

public class LightningClickController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private ShieldTarget shieldTarget;
    [SerializeField] private LayerMask shieldLayer;

    [SerializeField] private ParticleSystem hitSpark;

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
            // 스파크를 실제 맞은 위치로 이동
            hitSpark.transform.position = hit.point;

            // 처음부터 다시 재생
            hitSpark.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            hitSpark.Play(true);

            // 쉴드 데미지
            shieldTarget.TakeDamage(25f);
        }
    }
}