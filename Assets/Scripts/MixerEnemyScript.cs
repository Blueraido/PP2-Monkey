using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerEnemyScript : EnemyAI
{
    [SerializeField] GameObject projectile;
    [SerializeField] int rangedAttackRange;
    [SerializeField] int rangedAttackInterval;
    [SerializeField] int rangedAttackDamage;
    [SerializeField] int meleeAttackRange;
    [SerializeField] int meleeAttackInterval;
    [SerializeField] int meleeAttackDamage;
    [SerializeField] Transform meleeAttackPos;
    [SerializeField] Transform RangedAttackPos;

    bool playerInMeleeRange;
    bool playerInRangedAttackRange;
    public override IEnumerator attack()
    {
        isAttacking = true;


        if (playerInMeleeRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(meleeAttackPos.transform.position, meleeAttackPos.transform.forward, out hit, meleeAttackRange))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (hit.transform != transform &&  dmg != null)
                {
                    dmg.takeDamage(meleeAttackDamage);
                }
            }
        }
        else if (playerInRangedAttackRange)
        {
            Instantiate(projectile, RangedAttackPos.position, transform.rotation);
            yield return new WaitForSeconds(rangedAttackInterval);
        }

        isAttacking = false;
    }
}
