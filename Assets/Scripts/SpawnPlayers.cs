using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject [] spawnPositions;
    public GameObject aiPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[0].transform.position, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(aiPrefab.name, spawnPositions[1].transform.position, Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(aiPrefab.name, spawnPositions[1].transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
