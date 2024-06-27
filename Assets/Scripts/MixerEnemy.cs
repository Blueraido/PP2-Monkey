using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MixerEnemy : EnemyAI
{
    [SerializeField] GameObject projectile;

    [SerializeField] public int meleeDamage;
    [SerializeField] int meleeRange;
    [SerializeField] float meleeRate;

    [SerializeField] int throwRange;

    [SerializeField] Collider legLeft;
    [SerializeField] Transform rangedAttackPos;

    bool inMeleeRange;
    bool inThrowRange;

    protected override IEnumerator attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
#if true
        inMeleeRange = distanceToPlayer < meleeRange;
        inThrowRange = distanceToPlayer > meleeRange && distanceToPlayer < throwRange;
#endif
        isAttacking = true;

        if (inThrowRange)
        {
            throwAttack();
            yield return new WaitForSeconds(attackRate);
        }
        else if (inMeleeRange)
        {
            meleeKick();
            yield return new WaitForSeconds(meleeRate);
        }
        isAttacking = false;
    }

    public void meleeKick()
    {
        leftLegColliderOn(legLeft);
        anim.SetTrigger("Kick");
        leftLegColliderOff(legLeft);
    }

    public void throwAttack() { anim.SetTrigger("Throw"); }

    public void createProjectile() { Instantiate(projectile, rangedAttackPos.position, transform.rotation); }

    public void leftLegColliderOn(Collider collider) { legLeft.enabled = true; }

    public void leftLegColliderOff(Collider collider) { legLeft.enabled = false; }
}
