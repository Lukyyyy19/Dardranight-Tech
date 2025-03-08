using System;
using System.Collections;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sr;
    [SerializeField] private GameObject m_ExplosionParticles;
    [SerializeField] Enemy m_enemy;
    Coroutine m_hitEffect;

    private void Awake()
    {
        m_enemy.OnDeath += OnDeath;
    }

    private IEnumerator HitEffect()
    {
        m_sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        m_sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        m_sr.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        m_sr.color = Color.white;
    }

    public void StartHitEffect()
    {
        m_hitEffect = StartCoroutine(HitEffect());
    }

    public void InstantiateExplosionParticles()
    {
        Instantiate(m_ExplosionParticles, transform.position, Quaternion.identity);
    }

    private void OnDeath()
    {
        InstantiateExplosionParticles();
        StopCoroutine(m_hitEffect);
    }

    private void OnDisable()
    {
        m_enemy.OnDeath -= OnDeath;
    }
}