using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    [SerializeField] int meleeDamage;

    [SerializeField] int attackInterval;
    [SerializeField] Transform attackPosition;



    public override IEnumerator attack()
    {
        isAttacking = true;
        anim.SetTrigger("Melee Attack");
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

        yield return new WaitForSeconds(attackInterval);
        
        isAttacking = false;
    }

}
