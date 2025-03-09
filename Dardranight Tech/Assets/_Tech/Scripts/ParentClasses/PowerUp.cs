using System;
using UnityEngine;

public enum PowerUpAbility
{
    Health,
    BulletsQTY,
}
public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpAbility m_ability;
    
    public PowerUpAbility GetAbility()
    {
        return m_ability;
    }

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
    }
}
