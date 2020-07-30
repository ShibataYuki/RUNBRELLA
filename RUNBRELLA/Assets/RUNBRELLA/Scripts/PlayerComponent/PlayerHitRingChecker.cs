using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitRingChecker : MonoBehaviour
{

    [SerializeField]
    private PlayerPlusText playerPlusText = null;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Ring")
        {
            StartCoroutine(playerPlusText.Show());
        }
    }

}
