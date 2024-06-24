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

    bool playerInMeleeRange;
    bool fistLeftActive;
    bool fistRightActive;

    public override IEnumerator attack()
    {
        isAttacking = true;

        if (playerInSightRange && agent.remainingDistance < meleeRange) anim.SetTrigger("Melee");

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void melee()
    {
        fistLeftColliderOn(fistLeft);
        fistRightColliderOn(fistRight);

        GameManager.instance.player.GetComponent<IDamage>().takeDamage(meleeDamage);

        fistLeftColliderOff(fistLeft);
        fistRightColliderOff(fistRight);
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

    public void fistRightColliderOff(Collider other)
    {
        fistRight.enabled = false;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamage playerDamage = collision.gameObject.GetComponent<IDamage>();

            if (playerDamage != null && (fistLeftActive || fistRightActive))
                playerDamage.takeDamage(meleeDamage);
        }
    }
}