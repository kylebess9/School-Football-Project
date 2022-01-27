using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacklePlayer : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.tag == "Player")
        {
            GameController.ThisInstance.tacklePlayer();
        }
    }
}
