using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform attackPostion;
    [SerializeField] int animTransSpeed;

    [SerializeField] int sightRange;
    [SerializeField] int attackRange;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int rotateTowardTarget;
    [SerializeField] float attackInterval;
    [SerializeField] GameObject projectile;

    [SerializeField] int HP;

    bool isAttacking;
    bool playerInSightRange;

    Vector3 playerDir;

#if false
    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange; 
#endif


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGoalEnemy(1);
    }

    // Update is called once per frame
    void Update()
    {
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
#if false
        if (!playerInSightRange && !playerInAttackRange)
            patrolling();
        else if (playerInSightRange && !playerInAttackRange)
            chasePlayer();
        /*else if (playerInSightRange && playerInAttackRange)
            attackPlayer();*/ 
#endif
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }



    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            GameManager.instance.updateGoalEnemy(-1);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInSightRange = true;
    }

    public void OnTriggerExit(Collider other)
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

IEnumerator flashDamage()
{
    model.material.color = Color.red;
    yield return new WaitForSeconds(1.0f);
    model.material.color = Color.white;
}

IEnumerator attack()
{
    isAttacking = true;

    Instantiate(projectile, attackPostion.position, transform.rotation);

    yield return new WaitForSeconds(attackInterval);

    isAttacking = false;
}
}
