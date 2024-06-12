using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    [SerializeField] int meleeDamage;
    [SerializeField] int attackRange;
    [SerializeField] int attackInterval;
    [SerializeField] int meleeStrikePoint;
    [SerializeField] Transform attackPosition;

    public override IEnumerator attack()
    {
        isAttacking = true;

        if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) <= attackRange )
        {
            {
                
            }
        }

        yield return new WaitForSeconds(attackInterval);
        
        isAttacking = false;
    }

}
