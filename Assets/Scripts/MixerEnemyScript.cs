using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerEnemyScript : EnemyAI
{
    [SerializeField] GameObject projectile;
    
    [SerializeField] int rangedAttackInterval;
    [SerializeField] int rangedAttackDamage;
    [SerializeField] int rangedAttackRange;
    [SerializeField] Transform rangedAttackPos;

    [SerializeField] int meleeAttackInterval;
    [SerializeField] int meleeAttackDamage;
    [SerializeField] int meleeAttackRange;
    [SerializeField] Transform meleeAttackPos;
   

    public override void Update()
    {
        base.Update(); // Call the base class Update

        // Check if the enemy should attack based on the range
        if (playerInSightRange && !isAttacking)
        {
            StartCoroutine(attack());
        }
    }
    public override IEnumerator attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);

#if false
        if (distanceToPlayer < meleeAttackRange)
            playerInMeleeRange = true;

        if (distanceToPlayer < rangedAttackRange)
            playerInRangedAttackRange = true;
#endif 

        isAttacking = true;

#if false
        if (playerInRangedAttackRange && !playerInMeleeRange)
        {
            playerInMeleeRange = false;
            anim.SetTrigger("Ranged Attack");
            Instantiate(projectile, rangedAttackPos.position, transform.rotation);

            yield return new WaitForSeconds(rangedAttackInterval);
        }
        
        if (playerInMeleeRange && playerInRangedAttackRange)
        {
            
            RaycastHit hit;
            anim.SetTrigger("Melee Attack");
            if (Physics.Raycast(meleeAttackPos.transform.position, meleeAttackPos.transform.forward, out hit, meleeAttackRange))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform &&  dmg != null)
                    dmg.takeDamage(meleeAttackDamage);
            }

            yield return new WaitForSeconds(meleeAttackInterval);
        }
        isAttacking = false;
#endif
        yield return new WaitForSeconds(meleeAttackInterval);
    }
}
