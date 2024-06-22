using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    [SerializeField] int meleeDamage;
    [SerializeField] int attackInterval;
    [SerializeField] Transform attackPosition1;
    [SerializeField] Transform attackPosition2;



    protected override IEnumerator attack()
    {
        isAttacking = true;
        anim.SetTrigger("Melee Attack");
#if false
        if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) <= meleeAttackRange )
        {
            RaycastHit hit;
            if (Physics.Raycast(attackPosition.transform.position, attackPosition.transform.forward, out hit, meleeAttackRange))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (hit.transform != transform && dmg != null)
                    dmg.takeDamage(meleeAttackRange);
            }

        }
#endif 
        yield return new WaitForSeconds(attackInterval);
        
        isAttacking = false;
    }

}
