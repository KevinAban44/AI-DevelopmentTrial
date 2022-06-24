using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public abstract class Mob : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] public Transform target;
    [SerializeField] public GameObject[] SpawnPoints;
    public LayerMask whatIsGround, whatIsEnemy;
    public GameObject[] targets;
    protected PhotonView _photonView;
    public int teamIndex = 0;

    [Header("Settings")]
    public bool canPatrol = false;
    public bool canFollowPath = true;
    public bool canChase = false;
    public bool canAttack = true;

    [Header("Patrol")]
    public Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkPointRange;

    [Header("Attack")]
    public float attackCooldown = 10f;

    
    [Header("States")]
    [SerializeField] protected float _sightRange;
    [SerializeField] protected float _attackRange;
    protected bool _isEnemyInSight, _isEnemyInRange;
    public int nearestPlayerIndex;
    public float rotationSpeed = 40f;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (teamIndex == 0)
            targets = GameObject.FindGameObjectsWithTag("RedTeam");
        else
            targets = GameObject.FindGameObjectsWithTag("BlueTeam");

        target = targets[0].transform;
        nearestPlayerIndex = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (Vector3.Distance(targets[nearestPlayerIndex].transform.position, transform.position)
              > Vector3.Distance(targets[i].transform.position, transform.position))
            {
                nearestPlayerIndex = i;
            }
        }

        _isEnemyInSight = Physics.CheckSphere(transform.position, _sightRange, whatIsEnemy);
        _isEnemyInRange = Physics.CheckSphere(transform.position, _attackRange, whatIsEnemy);

       if (!_isEnemyInSight && !_isEnemyInRange) Patrolling();
       if (!_isEnemyInSight && !_isEnemyInRange) Pathing();
       if (_isEnemyInSight && !_isEnemyInRange) Chase();
       if (_isEnemyInSight && _isEnemyInRange) Attack();
    }

    private void Pathing()
    {
        if (!canFollowPath) return;

        if(teamIndex == 0)
            _agent.SetDestination(SpawnPoints[1].transform.position);
        else
            _agent.SetDestination(SpawnPoints[0].transform.position);
    }

    private void SetWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    protected virtual void Patrolling()
    {
        if (!canPatrol) return;
        if (!walkPointSet) SetWalkPoint();

        if (walkPointSet)
            _agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    protected virtual void Chase()
    {
        if (!canChase) return;

        target = targets[nearestPlayerIndex].transform;
        _agent.SetDestination(target.position);
    }
    protected void Attack()
    {
        DoAttack();
    }

    protected abstract void DoAttack();

    protected IEnumerator resetAttack(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
