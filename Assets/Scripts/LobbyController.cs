using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviourPunCallbacks
{
    private byte _maxPlayer = 4;
    void Start()
    {
        StartCoroutine(InitializePhoton());
    }

    IEnumerator InitializePhoton()
    {
        Debug.Log("Connecting to master...");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    ///<summary>
    /// connect and join a random room
    ///</summary>
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined a room");
        Debug.Log($"Number of players in room: {PhotonNetwork.PlayerList.Length}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Someone joined the room!");
        Debug.Log($"Number of players in room: {PhotonNetwork.PlayerList.Length}");
        StartGame();
    }

    ///<summary>
    /// if there are no rooms, create one
    ///</summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join, Creating room...");
        RoomOptions _options = new RoomOptions()
        {
            MaxPlayers = _maxPlayer,
            IsVisible = true
        };
        PhotonNetwork.CreateRoom(null, _options);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }

}
