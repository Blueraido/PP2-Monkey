using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MixerEnemyScript : EnemyAI
{
    [SerializeField] GameObject projectile;
    
    [SerializeField] int throwDamage;
    [SerializeField] int throwRange;
   
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeRange;
     
    [SerializeField] Collider fistLeft;
    [SerializeField] Collider fistRight;
    [SerializeField] Transform rangedAttackPos;

    bool fistLeftActive;
    bool fistRightActive;

    protected override IEnumerator attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);

        isAttacking = true;

        if (distanceToPlayer <= meleeRange)
        {
            meleeAttack();
        }

        if (distanceToPlayer <= throwRange)
        {
            throwAttack();
        }
        yield return new WaitForSeconds(attackRate);
    }

    public void meleeAttack()
    {
        anim.SetTrigger("Melee");
    }

    public void throwAttack()
    {
        anim.SetTrigger("Throw");
    }

    public void createProjectile()
    {
        Instantiate(projectile, rangedAttackPos.position, transform.rotation);
    }
}
