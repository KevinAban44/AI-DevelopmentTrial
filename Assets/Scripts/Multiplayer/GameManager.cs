using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject [] spawnPositions;
    public GameObject[] aiPrefab;
    public byte maxPlayer = 3;
    public int minionCount = 2;
    

    private bool isGameOngoing = true;

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

            StartCoroutine(spawnCycle());
        }

        //If player is dead
        _player.GetComponent<Health>()._isDeadEvent.AddListener(() =>
        {
            Respawn(_player);
        });
    }

    IEnumerator spawnCycle()
    {
        
        while (isGameOngoing)
        {
            for(int i = 0; i < minionCount; i++)
            {
                //Spawn Minion on blue team
                var _blueMinion = PhotonNetwork.InstantiateRoomObject(aiPrefab[0].name, spawnPositions[0].transform.position, Quaternion.identity);
                Mob _blueMinionScript = _blueMinion.GetComponent<Mob>();
                _blueMinionScript.teamIndex = 0;
                _blueMinionScript.SpawnPoints = spawnPositions;
                _blueMinion.tag = "BlueTeam";
                int layerIndex = LayerMask.NameToLayer("BlueTeam");
                _blueMinion.layer = layerIndex;
                LayerMask enemy = LayerMask.GetMask("RedTeam");
                _blueMinionScript.whatIsEnemy = enemy;

                //Spawn Minion on red team
                var _redMinion = PhotonNetwork.InstantiateRoomObject(aiPrefab[0].name, spawnPositions[1].transform.position, Quaternion.identity);
                Mob _redMinionScript = _redMinion.GetComponent<Mob>();
                _redMinionScript.teamIndex = 1;
                _redMinionScript.SpawnPoints = spawnPositions;
                _redMinion.tag = "RedTeam";
                int layerIndex2 = LayerMask.NameToLayer("RedTeam");
                _redMinion.layer = layerIndex2;
                enemy = LayerMask.GetMask("BlueTeam");
                _redMinionScript.whatIsEnemy = enemy;

                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(30f);
        }
    }

    public void Respawn(GameObject _player)
    {
        var _spawnPoint =  spawnPositions[0].transform;
        _player.GetPhotonView().RPC("PunRespawn", RpcTarget.All,
            _spawnPoint.position.x, _spawnPoint.position.y, _spawnPoint.position.z);
    }
}
