using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class Health : MonoBehaviour
{
    public float MaxHitPoint = 100;
    public float CurrentHitPoint = 0;
    public Image CurrentHitPointImage;
    private bool isDead;
    public UnityEvent _isDeadEvent;
    private float _hitRatio;

    private void Awake()
    {
        CurrentHitPoint = MaxHitPoint;
    }

    [PunRPC] 
    public void ReduceHealth(float damage)
    {
        ApplyDamage(damage);
    }

    public void ApplyDamage(float amount)
    {
        if (isDead) { return; }
        CurrentHitPoint -= amount;
        UpdatePointsBars();

        //If player health is 0, kill player
        if (CurrentHitPoint <= 0)
            {
                isDead = true;
                CurrentHitPoint = 0;
                Dead();
            }
    }

    // private void LateUpdate()
    // {
    //     UpdatePointsBars();
    // }

    private void UpdatePointsBars()
    {
        _hitRatio = CurrentHitPoint / MaxHitPoint;
        CurrentHitPointImage.rectTransform.localScale = new Vector3(_hitRatio, 1, 1);
    }

    private void Dead()
    {
        _isDeadEvent?.Invoke();
        isDead = false;
        // StartCoroutine(DeathRespawnRoutine());
    }

    //Respawn with delay
    IEnumerator DeathRespawnRoutine()
    {
        yield return new WaitForSeconds(2f);
        _isDeadEvent?.Invoke();
        yield return new WaitForSeconds(1f);
        isDead = false;
    }

    [PunRPC]
    public void PunRespawn(float _x, float _y, float _z)
    {
        gameObject.transform.position = new Vector3(_x, _y, _z);
        CurrentHitPoint = 100;        
        UpdatePointsBars();
    }
}
