using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyFist : MonoBehaviour
{
    [SerializeField] MeleeEnemy meleeEnemy;
    //[SerializeField] float damage;

    [SerializeField] Collider fist;

    private void Start()
    {
        if (meleeEnemy == null)
            meleeEnemy = GetComponentInParent<MeleeEnemy>();
        
    }

    public void OnTriggerEnter(Collider other)
    {
        float damage = meleeEnemy.meleeDamage;

        if (other.CompareTag("Player"))
        {

            IDamage dmg = other.gameObject.GetComponent<IDamage>();

            if (dmg != null)
                dmg.takeDamage(damage);
        }
    }
}
