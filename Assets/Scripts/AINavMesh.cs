using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AINavMesh : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private NavMeshAgent agent;

    PhotonView view;
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                agent.destination = target.transform.position;
            }
        }
        
    }
}
