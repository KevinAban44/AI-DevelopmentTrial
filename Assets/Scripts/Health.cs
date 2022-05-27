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
    private float _hitRatio;

    private void Awake()
    {
        CurrentHitPoint = MaxHitPoint;
    }

    public void ApplyDamage(float amount)
    {
        if (isDead) { return; }
        CurrentHitPoint -= amount;
        UpdatePointsBars();
    }

    private void UpdatePointsBars()
    {
        _hitRatio = CurrentHitPoint / MaxHitPoint;
        CurrentHitPointImage.rectTransform.localScale = new Vector3(_hitRatio, 1, 1);
    }
}