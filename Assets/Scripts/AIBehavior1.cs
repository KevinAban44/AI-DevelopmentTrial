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
    bool walkPointset;
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
        float randomx = Random.Range(-walkPointRange, walkPointRange);
    }

    private void Patrolling()
    {

    }
    private void Chase()
    {

    }
    private void Attack()
    {

    }
}
