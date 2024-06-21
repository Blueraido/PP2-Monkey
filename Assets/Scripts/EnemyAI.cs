using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] int animTransSpeed;

    [SerializeField] int HP;
    [SerializeField] int sightRange;
    [SerializeField] int facePlayer;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;
#if false
    [SerializeField] public int rangedAttackRange;
    [SerializeField] public int meleeAttackRange;
#endif
    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;

    protected bool isAttacking;
    protected bool playerInSightRange;
    protected bool destChosen;
#if false
    public bool playerInMeleeRange;
    public bool playerInRangedAttackRange;
#endif
    Vector3 playerDir;
    protected Vector3 startingPos;

    protected float angleToPlayer;
    protected float stoppingDistanceOrig;
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        GameManager.instance.updateGoalEnemy(1);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTransSpeed));

        if (playerInSightRange && playerSighted())
            StartCoroutine(roam());

        else if (!playerInSightRange)
            StartCoroutine(roam());
        
    }

    protected void circlePlayer()
    {
        //int enemyAngle = 0;

    }

    protected IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.05)
        {
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);

            agent.stoppingDistance = 0;
            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);
            destChosen = false;
        }
    }

    protected bool playerSighted()
    {
        // Get the direction of the player and the angle relative to LOS
        playerDir = Camera.main.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(Camera.main.transform.position - headPos.position, transform.forward);

        Debug.DrawRay(headPos.position, Camera.main.transform.position - headPos.position);
        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                    faceTarget();

                if (!isAttacking && angleToPlayer <= shootAngle)
                    StartCoroutine(attack());

                return true;
;           }

        }
        agent.stoppingDistance = stoppingDistanceOrig;
        return false;
    }
    protected virtual void faceTarget()
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInSightRange = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInSightRange = false;
    }

    protected virtual IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        model.material.color = Color.white;
    }

    protected abstract IEnumerator attack();

    void OnDrawGizmos()
    {
        // Draw a red line indicating the forward direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }


}

