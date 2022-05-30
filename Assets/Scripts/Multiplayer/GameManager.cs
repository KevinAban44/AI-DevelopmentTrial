using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject [] spawnPositions;
    public GameObject aiPrefab;
    public byte maxPlayer = 3;

    IEnumerator Start()
    {
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        
        Initialize();
    }

    private void Initialize()
    {
        var _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[0].transform.position, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log($"Number of players in game {playerCount}");

            while(playerCount < maxPlayer)
            {
                Debug.Log("Added bot");
                PhotonNetwork.InstantiateRoomObject(aiPrefab.name, spawnPositions[1].transform.position, Quaternion.identity);
                playerCount++;
            }
        }

        //If player is dead
        _player.GetComponent<Health>()._isDeadEvent.AddListener(() =>
        {
            Respawn(_player);
        });
    }

    public void Respawn(GameObject _player)
    {
        var _spawnPoint =  spawnPositions[0].transform;
        _player.GetPhotonView().RPC("PunRespawn", RpcTarget.All,
            _spawnPoint.position.x, _spawnPoint.position.y, _spawnPoint.position.z);
    }
}
