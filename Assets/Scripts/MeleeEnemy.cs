using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    [SerializeField] int meleeDamage;
    [SerializeField] int attackRange;
    [SerializeField] int attackInterval;
    [SerializeField] Transform attackPosition;

    public override IEnumerator attack()
    {
        isAttacking = true;

        if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) <= attackRange )
        {
            RaycastHit hit;
            if (Physics.Raycast(attackPosition.transform.position, attackPosition.transform.forward, out hit, attackRange))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (hit.transform != transform && dmg != null)
                {
                    dmg.takeDamage(attackRange);
                }
            }

        }

        yield return new WaitForSeconds(attackInterval);
        
        isAttacking = false;
    }

}
