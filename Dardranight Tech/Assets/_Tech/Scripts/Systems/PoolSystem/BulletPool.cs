using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletPool : MonoBehaviour
{
    Dictionary<int, PoolObject<Bullet>> pools = new Dictionary<int, PoolObject<Bullet>>();


    public void IntantiateBullets(int id, Bullet bulletType, int prewarm = 20)
    {
        var bulletPoolTemp = new GameObject();
        bulletPoolTemp.name = "Pool de " + id;
        Func<Bullet> bulletFunc = () =>
        {
            Bullet bullet = GameObject.Instantiate(bulletType, Vector2.zero, Quaternion.identity);
            bullet.transform.parent = bulletPoolTemp.transform;
            bullet.Configure(id, Return);
            return bullet;
        };

        if (pools != null)
        {
            if (!pools.ContainsKey(id))
            {
                PoolObject<Bullet> currentPool = new PoolObject<Bullet>();
                currentPool.Intialize(TurnOnBullet, TurnOffBullet, bulletFunc, prewarm);
                pools.Add(id, currentPool);
            }
        }
    }

    public Bullet Get(int id, Vector3 pos, Vector3 dir)
    {
        Debug.Log("el id es:" + id);
        Bullet myBullet = pools[id].Get();
        Debug.Log(myBullet.name);
        myBullet.Move(pos, dir);
        return myBullet;
    }


    public void Return(int id, Bullet obj)
    {
        pools[id].Return(obj);
        obj.transform.localPosition = Vector3.zero;
        obj.timer = 0;
    }


    void TurnOnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.TurnOn();
    }

    void TurnOffBullet(Bullet bullet)
    {
        bullet.TurnOff();
        bullet.gameObject.SetActive(false);
    }
}