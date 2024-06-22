using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeRange;

    [SerializeField] Transform attackPosition1;
    [SerializeField] Transform attackPosition2;

    [SerializeField] Collider fistLeft;
    [SerializeField] Collider fistRight;



    protected override IEnumerator attack()
    {
        isAttacking = true;

        if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) <= meleeRange)
        {
            fistLeftColliderOn(fistLeft);
            anim.SetTrigger("Throw");
        }
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
        yield return new WaitForSeconds(attackRate);
        
        isAttacking = false;
    }

    public void fistLeftColliderOn(Collider other)
    {
        fistLeft.enabled = true;
    }

    public void fistLeftColliderOff(Collider other)
    {
        fistLeft.enabled = false;
    }

    public void fistRightColliderOn(Collider other)
    {
        fistRight.enabled = true;
    }

    public void fightRight(Collider other)
    {
        fistRight.enabled = false;
    }
}
