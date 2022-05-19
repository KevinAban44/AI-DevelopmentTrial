using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    

    PhotonView view;
    void Awake()
    {
        view = GetComponent<PhotonView>();
        Debug.Log("PlayerJoined");
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = 5.0f;

        if (view.IsMine)
        {
              transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        }

    }
}
