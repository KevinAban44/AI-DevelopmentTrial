using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SampleMob : Mob
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public GameObject bulletStartPoint;
    public float bulletSpeed = 10f; 

    protected override void DoAttack()
    {
        target = targets[nearestPlayerIndex].transform;
        _agent.SetDestination(transform.position);

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
        if (canAttack)
        {
            canAttack = false;
            Shoot();
            StartCoroutine(resetAttack(2f));
        }
    }
    private void Shoot()
    {
        if (!_photonView.IsMine)
            return;
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletStartPoint.transform.position, Quaternion.identity);
        // Debug.Log("BulletSpawned");
        bullet.GetComponent<BulletScript>().setParent(gameObject);
        bullet.GetComponent<BulletScript>().EnemyTag = target.tag;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.AddForce(bulletStartPoint.transform.forward * bulletSpeed, ForceMode.Impulse);
    }
}
