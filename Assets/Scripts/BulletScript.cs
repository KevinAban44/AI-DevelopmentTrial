using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviourPun
{
    public GameObject parent;
    // Start is called before the first frame update
    float Damage = 20;
    private void Start()
    {
       // StartCoroutine(destroyBullet());
    }

    public void setParent(GameObject Parent)
    {
        parent = Parent;
        // Debug.Log(parent.name);
    }
    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        PhotonView target = other.gameObject.GetComponent<PhotonView>();

        if (target != null && (target.IsMine || target.IsRoomView) && other.gameObject != parent && other.gameObject.tag != "Bullet")
        {
            if(other.tag == "Player")
            {
                target.RPC("ReduceHealth", RpcTarget.AllBuffered, Damage);
            }
           // Debug.Log(other.name);
            GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
           
        }
    }

    [PunRPC]
    public void DestroyBullet()
    {
       //Destroy(gameObject);
    }
}
