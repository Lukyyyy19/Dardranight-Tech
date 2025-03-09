using System;
using UnityEngine;

public class SingeltonMonoBehaviourScript<T> : MonoBehaviour where T : SingeltonMonoBehaviourScript<T>
{
   protected static T m_instance;
   public static T Instance => m_instance;
   
   protected bool m_dontDestroyOnLoad;

   protected virtual void Awake()
   {
      if(m_instance == null)
      {
         m_instance = (T)this;
         if(m_dontDestroyOnLoad)
            DontDestroyOnLoad(this);
      }
      else if(m_dontDestroyOnLoad)
      {
         Destroy(gameObject);
      }
   }
}
