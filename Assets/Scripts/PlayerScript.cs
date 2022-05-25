using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    PhotonView view;
    private Rigidbody _rb;
    public float jumpForce = 5f;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        Debug.Log("PlayerJoined");
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = 5.0f;

        if (view.IsMine)
        {
            transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
                _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

    }
}
