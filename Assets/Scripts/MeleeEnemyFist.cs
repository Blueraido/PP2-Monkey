using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyFist : MonoBehaviour
{
    int damage;
    
    public void OnTriggerEnter(Collider other)
    {
        damage = other.gameObject.GetComponent<MeleeEnemy>().meleeDamage;

        if (other.CompareTag("Player"))
        {

            IDamage dmg = other.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }
#if false
            if (playerDamage != null &&
                ((fistLeftActive && collision.contacts[0].thisCollider == fistLeft) ||
                (fistRightActive && collision.contacts[0].thisCollider == fistRight)))
                playerDamage.takeDamage(meleeDamage);
#endif
        }
    }
}
