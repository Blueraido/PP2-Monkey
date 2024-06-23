using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyAI
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform attackPosition;

    public override IEnumerator attack()
    {
        // Begim the attacking state
        isAttacking = true;
        anim.SetTrigger("Throw");

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void createProjectile()
    {
        Instantiate(projectile, attackPosition.position, transform.rotation);
    }
}
