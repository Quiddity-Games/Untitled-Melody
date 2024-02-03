using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton Manager to Call Events for Dreamworld
/// All Events should be noted in DreamworldEventEnum.cs
/// </summary>
public class DreamworldEventManager : MonoBehaviour
{

    public Dictionary<DreamworldVoidEventEnum, Action> m_voidEventDict;
    public Dictionary<DreamworldVector3EventEnum, Action<Vector3>> m_vector3EventDict;
    public Dictionary<DreamworldBoolEventEnum, Action<bool>> m_boolEventDict;

    public static DreamworldEventManager Instance;
    

    void OnDisable() { Instance = null; }

    public void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            Init();
        } 
    }

    public void Init()
    {
        m_voidEventDict = new Dictionary<DreamworldVoidEventEnum, Action>();
        m_vector3EventDict = new Dictionary<DreamworldVector3EventEnum, Action<Vector3>>();
        m_boolEventDict = new Dictionary<DreamworldBoolEventEnum, Action<bool>>();
    }
    
   public void RegisterVoidEventResponse(DreamworldVoidEventEnum eventType, Action callback)
   {
       if (!m_voidEventDict.ContainsKey(eventType))
       {
           m_voidEventDict.Add(eventType, null);
       }
       m_voidEventDict[eventType] += callback;
   }

   public void DeregisterVoidEventResponse(DreamworldVoidEventEnum eventType, Action callback)
   {
       if (!m_voidEventDict.ContainsKey(eventType))
       {
           m_voidEventDict.Add(eventType, null);
       }
       m_voidEventDict[eventType] -= callback;
   }

   public void CallVoidEvent(DreamworldVoidEventEnum eventType)
   {
       if (!m_voidEventDict.ContainsKey(eventType))
       {
           m_voidEventDict.Add(eventType, null);
       }
       
       m_voidEventDict[eventType]?.Invoke();
   }

   public void ResetVoidEvent(DreamworldVoidEventEnum eventType)
   {
       m_voidEventDict[eventType] = () => { };
   }

   public void RegisterVector3EventResponse(DreamworldVector3EventEnum eventType, Action<Vector3> callback)
   {
       if (!m_vector3EventDict.ContainsKey(eventType))
       {
           m_vector3EventDict.Add(eventType, null);
       }
       m_vector3EventDict[eventType] += callback;
   }

   public void DeregisterVector3EventResponse(DreamworldVector3EventEnum eventType, Action<Vector3> callback)
   {
       if (!m_vector3EventDict.ContainsKey(eventType))
       {
           throw new Exception();
       }
       m_vector3EventDict[eventType] -= callback;
   }
   
   public void CallVector3Event(DreamworldVector3EventEnum eventType, Vector3 value)
   {
       if (!m_vector3EventDict.ContainsKey(eventType))
       {
           m_vector3EventDict.Add(eventType, vector3 => {});
       }
       m_vector3EventDict[eventType]?.Invoke(value);
   }
   
   public void RegisterBoolEventResponse(DreamworldBoolEventEnum eventType, Action<bool> callback)
   {
       if (!m_boolEventDict.ContainsKey(eventType))
       {
           m_boolEventDict.Add(eventType, (b => {}));
       }
        m_boolEventDict[eventType] += callback;
   }

   public void DeregisterBoolEventResponse(DreamworldBoolEventEnum eventType, Action<bool> callback)
   {
       if (!m_boolEventDict.ContainsKey(eventType))
       {
           m_boolEventDict.Add(eventType, null);
       }
       m_boolEventDict[eventType] -= callback;
   }
   
   public void CallBoolEvent(DreamworldBoolEventEnum eventType, bool value)
   {
       if (!m_boolEventDict.ContainsKey(eventType))
       {

           m_boolEventDict.Add(eventType, null);
       }
       m_boolEventDict[eventType].Invoke(value);
   }
}
