using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;

    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    [SerializeField] float attackInterval;

    [SerializeField] int HP;

    public GameObject player;
    public LayerMask IsGround, IsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    bool hasAttacked;

    // States
    public bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the player
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            patrolling();
        else if (playerInSightRange && !playerInAttackRange)
            chasePlayer();
        else if (playerInSightRange && playerInAttackRange)
            attackPlayer();
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashDamage());
    }

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

    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        model.material.color = Color.white;
    }
}
