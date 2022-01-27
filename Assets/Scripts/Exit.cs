using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour {
    public Button exitButton = null;
    public Canvas main = null;

    private void Awake()
    {
        exitButton = GetComponent<Button>();

        exitButton.onClick.AddListener(() => hideMenu());

    }

    private void hideMenu()
    {
        main.GetComponent<CanvasGroup>().alpha = 0;
        main.sortingOrder = 0;
    }
}
