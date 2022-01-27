using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievementCheck : MonoBehaviour {
    public Sprite unCheck = null;
    public Sprite Check = null;
    private Image thisimage = null;
    private void Start()
    {
        thisimage = GetComponent<Image>();
        uncomplete();
    }

    public void complete()
    {
        thisimage.sprite = Check;
    }

    public void uncomplete()
    {
        thisimage.sprite = unCheck;
    }
}
