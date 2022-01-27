using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonController : MonoBehaviour {


    public static SeasonController SingletonInstance = null;
    public static SeasonController ThisInstance
    {
        get
        {
            if (SingletonInstance == null)
            {
                GameObject Controller = new GameObject("DefaultController");
                SingletonInstance = Controller.AddComponent<SeasonController>();
            }
            return SingletonInstance;
        }
    }
}
