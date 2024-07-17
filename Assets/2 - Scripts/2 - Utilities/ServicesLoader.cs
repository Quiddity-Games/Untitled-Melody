using UnityEngine;

public static class ServicesLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() {
      //  Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("GameManager"))); 
        //Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("EventSystem"))); 
     }
}
