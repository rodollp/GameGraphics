using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldTarget : MonoBehaviour
{
    [SerializeField] private float maxShield = 100f;
    [SerializeField] private float currentShield;

    [SerializeField] private Renderer shieldRenderer;

    private Material shieldMaterial;

    private void Awake()
    {
        currentShield = maxShield;
        shieldMaterial = shieldRenderer.material;

        UpdateShieldShader();
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TakeDamage(25f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentShield -= damage;
        currentShield = Mathf.Clamp(currentShield, 0f, maxShield);

        UpdateShieldShader();

        if (currentShield <= 0f)
        {
            BreakShield();
        }
    }

    private void UpdateShieldShader()
    {
        float shieldRatio = currentShield / maxShield;

        shieldMaterial.SetFloat("_Shield_Ratio", shieldRatio);
    }

    private void BreakShield()
    {
        shieldRenderer.gameObject.SetActive(false);
    }
}