using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PoolObject<T> {
    public PoolObject() { }

    Stack<T> pool = new Stack<T>();

    Action<T> turnOn;
    Action<T> turnOff;
    Func<T> build;

    int prewarm;

    public void Intialize(Action<T> _turnOn, Action<T> _turnOff, Func<T> _build, int prewarm = 5) {
        this.prewarm = prewarm;
        this.turnOn = _turnOn;
        this.turnOff = _turnOff;
        this.build = _build;

        AddMore();
    }
    public void Intialize(Func<T> _build, int prewarm = 5) {
        this.prewarm = prewarm;
        this.build = _build;

        AddMore();
    }

    public T Get() {
        if (pool.Count <= 0) AddMore();
        var obj = pool.Pop();
        if (turnOn != null)
            turnOn(obj);
        return obj;
    }

    public void Return(T obj) {
        pool.Push(obj);
        if (turnOff != null)
            turnOff(obj);
    }

    void AddMore() {
        for (int i = 0; i < prewarm; i++) {
            var obj = build.Invoke();
            pool.Push(obj);
            if (turnOff != null)
                turnOff(obj);
        }
    }

}
