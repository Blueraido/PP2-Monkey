using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyAI
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform attackPosition;
    [SerializeField] int attackInterval;

    public override void Start()
    {
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
    }
    protected override IEnumerator attack()
    {
        // Begim the attacking state
        isAttacking = true;
        anim.SetTrigger("Throw");

        // Attack the player 
        Instantiate(projectile, attackPosition.position, transform.rotation);
        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }
}
