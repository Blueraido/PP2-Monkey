using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] int animTransSpeed;

    [SerializeField] int sightRange;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int rotateTowardTarget;
    [SerializeField] public int rangedAttackRange;
    [SerializeField] public int meleeAttackRange;

    [SerializeField] int HP;
    [SerializeField] int Exp;

    public bool isAttacking;
    public bool playerInSightRange;

    public bool playerInMeleeRange;
    public bool playerInRangedAttackRange;

    Vector3 playerDir;

#if false
    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange; 
#endif
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        GameManager.instance.updateGoalEnemy(1);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetFloat("Blend", agent.velocity.normalized.magnitude);
        // Get the direction of the player
        playerDir = GameManager.instance.player.transform.position - transform.position;

        if (playerInSightRange)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();
            }

            if (!isAttacking)
            {
                StartCoroutine(attack());
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
        playerInMeleeRange = distanceToPlayer <= meleeAttackRange;
        playerInRangedAttackRange = distanceToPlayer <= rangedAttackRange;
#if false
        if (!playerInSightRange && !playerInAttackRange)
            patrolling();
        else if (playerInSightRange && !playerInAttackRange)
            chasePlayer();
        /*else if (playerInSightRange && playerInAttackRange)
            attackPlayer();*/ 
#endif
    }

    public virtual void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }



    public virtual void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            GameManager.instance.updateGoalEnemy(-1);
            ExpManager.instance.updateExp(Exp);
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInSightRange = true;
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInSightRange = false;
    }


#if false
    void patrolling()
    {
        if (!walkPointSet)
            searchWalkPoint();
        else if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // When walk point is reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    } 


    void searchWalkPoint()
    {
        float randomZPoint = Random.Range(-walkPointRange, walkPointRange);
        float randomXPoint = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomXPoint, transform.position.y, transform.position.z + randomZPoint);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround))
            walkPointSet = true;


    }

    void chasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }


    void attackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        if (!hasAttacked)
        {
            hasAttacked = true;
            Invoke(nameof(resetAttack), attackInterval);
        }
    }

    void resetAttack()
    {
        hasAttacked = false;
    }
#endif

    public virtual IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        model.material.color = Color.white;
    }

    public abstract IEnumerator attack();

    void OnDrawGizmos()
    {
        // Draw a red line indicating the forward direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }


}

#if false
isAttacking = true;
Instantiate(meleeStrikePoint, attackPostion.position, transform.rotation);
yield return new WaitForSeconds(attackInterval);
isAttacking = false;
#endif