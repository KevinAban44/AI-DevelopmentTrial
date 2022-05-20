using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior1 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkPointRange;

    //Attacking
    public float attackCooldown;
    bool hasAttacked;

    //States
    public float sightRange, attackRange;
    bool isEnemyInSight, isEnemyInRange;



    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        isEnemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isEnemyInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!isEnemyInSight && !isEnemyInRange) Patrolling();
        if (isEnemyInSight && !isEnemyInRange) Chase();
        if (isEnemyInSight && isEnemyInRange) Attack();
    }

    private void SetWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void Patrolling()
    {
        if (!walkPointSet) SetWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void Chase()
    {
        agent.SetDestination(target.position);
    }
    private void Attack()
    {
        
    }
}
