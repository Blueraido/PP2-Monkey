using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerEnemyScript : EnemyAI
{
    [SerializeField] GameObject projectile;
    
    [SerializeField] int rangedAttackInterval;
    [SerializeField] int rangedAttackDamage;
    
    [SerializeField] int meleeAttackInterval;
    [SerializeField] int meleeAttackDamage;
    [SerializeField] Transform meleeAttackPos;
    [SerializeField] Transform rangedAttackPos;

    //bool playerInMeleeRange;
    //bool playerInRangedAttackRange;

    public override void Update()
    {
        base.Update(); // Call the base class Update

        // Check if the enemy should attack based on the range
        if ((playerInMeleeRange || playerInRangedAttackRange) && !isAttacking)
        {
            StartCoroutine(attack());
        }
    }
    public override IEnumerator attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
        playerInMeleeRange = distanceToPlayer <= rangedAttackRange;
        playerInRangedAttackRange = distanceToPlayer <= rangedAttackRange;

        isAttacking = true;
        if (playerInRangedAttackRange && !playerInMeleeRange)
        {
            anim.SetTrigger("Ranged Attack");
            Instantiate(projectile, rangedAttackPos.position, transform.rotation);

            yield return new WaitForSeconds(rangedAttackInterval);
        }
        
        if (playerInMeleeRange)
        {
            
            RaycastHit hit;
            anim.SetTrigger("Melee Attack");
            if (Physics.Raycast(meleeAttackPos.transform.position, meleeAttackPos.transform.forward, out hit, meleeAttackRange))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform &&  dmg != null)
                    dmg.takeDamage(meleeAttackDamage);

                yield return new WaitForSeconds(meleeAttackInterval);
            }
        }
        isAttacking = false;
    }
}
