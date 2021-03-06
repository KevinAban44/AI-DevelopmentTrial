using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AIBehavior1 : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] public Transform target;
    [SerializeField] public GameObject[] SpawnPoints;
    public LayerMask whatIsGround, whatIsEnemy;
    public GameObject[] targets;
    private PhotonView photonView;
    public int teamIndex = 0;
    

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkPointRange;

    //Attacking
    public float attackCooldown = 10f;
    bool canAttack = true;
    public GameObject bulletPrefab;
    public GameObject bulletStartPoint;
    public float bulletSpeed = 10f;
    

    //States
    public float sightRange, attackRange;
    bool isEnemyInSight, isEnemyInRange;
    public int nearestPlayerIndex;
    public float rotationSpeed = 40f;

    

    // Start is called before the first frame update
    void Awake()
    {
       
        agent = GetComponent<NavMeshAgent>();
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
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

        isEnemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        isEnemyInRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

       // if (!isEnemyInSight && !isEnemyInRange) Patrolling();
        if (!isEnemyInSight && !isEnemyInRange) Pathing();
       // if (isEnemyInSight && !isEnemyInRange) Chase();
        if (isEnemyInSight && isEnemyInRange) Attack();
    }

    private void Pathing()
    {
        if(teamIndex == 0)
            agent.SetDestination(SpawnPoints[1].transform.position);
        else
            agent.SetDestination(SpawnPoints[0].transform.position);
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
        target = targets[nearestPlayerIndex].transform;
        agent.SetDestination(target.position);
    }
    private void Attack()
    {
        target = targets[nearestPlayerIndex].transform;
        agent.SetDestination(transform.position);

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
        if (canAttack)
        {
            canAttack = false;
            Shoot();
            StartCoroutine(resetAttack());
        }
    
    
    }

    IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    private void Shoot()
    {
        if (!photonView.IsMine)
            return;
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletStartPoint.transform.position, Quaternion.identity);
        // Debug.Log("BulletSpawned");
        bullet.GetComponent<BulletScript>().setParent(gameObject);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.AddForce(bulletStartPoint.transform.forward * bulletSpeed, ForceMode.Impulse);
    }
     
}
