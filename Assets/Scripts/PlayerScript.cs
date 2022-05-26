using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    PhotonView view;
    [SerializeField] private GameObject myCamera;
    private Rigidbody _rb;
    public float jumpForce = 5f;

    void Awake()
    {
        
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            myCamera.SetActive(true);

            _rb = GetComponent<Rigidbody>();
        }
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
