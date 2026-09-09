using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] private ParticleSystem effectPrefab;
    [SerializeField] private int poolSize = 10;

    private List<ParticleSystem> pool = new List<ParticleSystem>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ParticleSystem effect = Instantiate(effectPrefab, transform);

            effect.gameObject.SetActive(false);
            pool.Add(effect);
        }
    }

    public void PlayEffect(Vector3 position)
    {
        ParticleSystem effect = GetPooledEffect();

        if (effect == null)
            return;

        effect.transform.position = position;
        effect.gameObject.SetActive(true);

        effect.Clear(true);
        effect.Play(true);

        StartCoroutine(ReturnEffect(effect));
    }

    private ParticleSystem GetPooledEffect()
    {
        foreach (ParticleSystem effect in pool)
        {
            if (!effect.gameObject.activeSelf)
            {
                return effect;
            }
        }

        return null;
    }

    private IEnumerator ReturnEffect(ParticleSystem effect)
    {
        yield return new WaitUntil(() => !effect.IsAlive(true));

        effect.gameObject.SetActive(false);
    }
}