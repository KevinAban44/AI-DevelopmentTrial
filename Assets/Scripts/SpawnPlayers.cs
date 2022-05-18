using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject aiPrefab;
    public GameObject[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, spawnPositions.Length-1);
        PhotonNetwork.Instantiate(aiPrefab.name, spawnPositions[random].transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
