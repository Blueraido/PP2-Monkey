using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    [SerializeField] public int meleeDamage;
    [SerializeField] int meleeRange;

    [SerializeField] Transform attackPosition1;
    [SerializeField] Transform attackPosition2;

    [SerializeField] Collider fistLeft;
    [SerializeField] Collider fistRight;

    bool playerInMeleeRange;
    bool fistLeftActive;
    bool fistRightActive;

    public override void Start()
    {
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
    }

    protected override IEnumerator attack()
    {
        isAttacking = true;

        if (playerInSightRange && agent.remainingDistance < meleeRange)
        {
            playerInMeleeRange = true;

            if (playerInMeleeRange)
            {
                anim.SetTrigger("Melee"); 
            }
        }
        else
            playerInMeleeRange = !playerInMeleeRange;

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void Melee()
    {
        fistLeftColliderOn(fistLeft);
        fistRightColliderOn(fistRight);

#if false
        fistLeftColliderOff(fistLeft);
        fistRightColliderOff(fistRight);
#endif
    }
    public void fistLeftColliderOn(Collider other) { fistLeft.enabled = true; }

    public void fistLeftColliderOff(Collider other) { fistLeft.enabled = false; }

    public void fistRightColliderOn(Collider other) { fistRight.enabled = true; }

    public void fistRightColliderOff(Collider other) { fistRight.enabled = false; }

}