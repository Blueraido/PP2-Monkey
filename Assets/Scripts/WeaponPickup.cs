using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    [SerializeField] WeaponStats weap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            GameManager.instance.playerScript.GetWeaponStats(weap);
            Destroy(gameObject);
        
        }

    }

}
