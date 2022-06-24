using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviourPun
{
    public GameObject parent;
    public string EnemyTag = "Player";
    // Start is called before the first frame update
    float Damage = 20;
    private void Start()
    {
       //StartCoroutine(destroyBullet());
    }

    public void setParent(GameObject Parent)
    {
        parent = Parent;
        //Debug.Log(parent.name);
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

        PhotonView targetPV = other.gameObject.GetComponent<PhotonView>();

        if (targetPV != null && (targetPV.IsMine || targetPV.IsRoomView) && other.gameObject != parent && other.gameObject.tag != "Bullet")
        {
            if(other.tag == EnemyTag)
            {
                Debug.Log(other.tag);
                GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
                //target.RPC("ReduceHealth", RpcTarget.AllBuffered, Damage);
            }
            //Debug.Log(other.name);
           
        }
    }

    [PunRPC]
    public void DestroyBullet()
    {
       Destroy(gameObject);
    }
}
