using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sr;
    [SerializeField] private GameObject m_ExplosionParticles;
    [SerializeField] Enemy m_enemy;
    Coroutine m_hitEffect;
    Tween m_tween;

    private void OnEnable()
    {
        m_enemy.OnDeath += OnDeath;
    }

    public void ResetVisuals()
    {
        m_tween.Kill();
        m_sr.color = Color.white;
        transform.localScale = Vector3.one;
    }

    private IEnumerator HitEffect()
    {
        ResetVisuals();
        m_tween = transform.DOPunchScale(Vector3.one * 1.1f, 0.3f);
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
        if (!gameObject.activeSelf) return;
        m_hitEffect = StartCoroutine(HitEffect());
    }

    private void InstantiateExplosionParticles()
    {
        Instantiate(m_ExplosionParticles, transform.position, Quaternion.identity);
    }

    private void OnDeath(Enemy enemy)
    {
        InstantiateExplosionParticles();
        StopCoroutine(m_hitEffect);
        ResetVisuals();
    }

    private void OnDisable()
    {
        m_enemy.OnDeath -= OnDeath;
    }
}