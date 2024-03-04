using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int  healAmount;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerhealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(5);
        }
    }
}
