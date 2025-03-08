using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemiesType
{
   BigEnemy = 0,
   MediumEnemy = 1,
   SmallEnemy = 2,
}
public class EnemiesPool : MonoBehaviour
{
   Dictionary<int,PoolObject<Enemy>> m_pools = new Dictionary<int, PoolObject<Enemy>>();
   public void IntantiateEnemies(int id, Enemy enemyType, int prewarm = 10)
   {
      var enemyPoolTemp = new GameObject();
      enemyPoolTemp.name = $"Pool de {(EnemiesType)id}";
      Func<Enemy> enemyFunc = () =>
      {
         Enemy enemy = GameObject.Instantiate(enemyType, Vector2.zero, Quaternion.identity);
         enemy.transform.parent = enemyPoolTemp.transform;
         enemy.Configure(id, Return);
         return enemy;
      };

      if (m_pools != null)
      {
         if (!m_pools.ContainsKey(id))
         {
            PoolObject<Enemy> currentPool = new PoolObject<Enemy>();
            currentPool.Intialize(TurnOnEnemy, TurnOffEnemy, enemyFunc, prewarm);
            m_pools.Add(id, currentPool);
         }
      }
   }
   public Enemy Get(int id, Vector3 pos)
   {
      Enemy myEnemy = m_pools[id].Get();
      myEnemy.transform.position = pos;
      return myEnemy;
   }


   public void Return(int id, Enemy obj)
   {
      m_pools[id].Return(obj);
      obj.transform.localPosition = Vector3.zero;
      // obj.timer = 0;
   }


   void TurnOnEnemy(Enemy enemy)
   {
      enemy.gameObject.SetActive(true);
      enemy.TurnOn();
   }

   void TurnOffEnemy(Enemy enemy)
   {
      enemy.TurnOff();
      enemy.gameObject.SetActive(false);
   }
}
