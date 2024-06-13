using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAI
{
    [SerializeField] GameObject projectile;
    [SerializeField] Animator anim;
    [SerializeField] Transform attackPosition;
    [SerializeField] int attackInterval;

    public override IEnumerator attack()
    {
        isAttacking = true;
        anim.SetTrigger("Ranged Attack");
        Instantiate(projectile, attackPosition.position, transform.rotation);
        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }
}
