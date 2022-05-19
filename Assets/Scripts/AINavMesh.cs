using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AINavMesh : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private NavMeshAgent agent;

    private GameObject[] players;
    PhotonView view;
    public LayerMask whatIsPlayer;
    bool isEnemyInSight;
    public float sightRange = 5f;

    RaycastHit hit;
    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        view = GetComponent<PhotonView>();
        target = players[0];
    }

    void Update()
    {
        


        if (view.IsMine)
        {
            agent.destination = target.transform.position;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            target = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        target = gameObject;
    }
}
