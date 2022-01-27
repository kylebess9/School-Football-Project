using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCanvas : MonoBehaviour {
    public Canvas menuOpen = null;
    public Button thisButton = null;

    private void Awake()
    {
        thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(() => openMenu());
    }

    private void openMenu()
    {
        menuOpen.sortingOrder = 1;
        menuOpen.GetComponent<CanvasGroup>().alpha = 1;
    }
}
