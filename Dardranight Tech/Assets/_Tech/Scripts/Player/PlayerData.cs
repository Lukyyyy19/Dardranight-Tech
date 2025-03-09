using UnityEngine;

[System.Serializable]
public class PlayerData
{
   public float speed = 5;
   public int health;
   public int maxHealth = 4;
   public int damage = 1;
   public int attackSpeed = 1;
   public int bulletQty = 1;
   public int score = 0;
   public float m_abilityDuration = 5f;
   public float m_abilityTimer = 0f;
}
