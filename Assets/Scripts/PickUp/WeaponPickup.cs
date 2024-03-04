using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private bool collected;
    public string theGun;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && !collected)
            {
                PlayerController.instance.AddGun(theGun);
                Destroy(gameObject);
                collected = true;
                AudioManager.instance.PlaySFX(4);
            }
        }
}