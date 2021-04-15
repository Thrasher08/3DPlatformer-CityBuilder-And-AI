using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class WeaponCollision : MonoBehaviour
{

    public static float attackDamage = 1;
    public float test;

    CameraShake cShake;

    private void Start()
    {
        cShake = FindObjectOfType<CameraShake>();
    }

    private void Update()
    {
        test = attackDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().health -= attackDamage;
            other.GetComponent<Enemy>().visual.transform.DOShakeRotation(0.5f, 10, 8, 70);
            StartCoroutine(cShake.Shake(0.2f));
            //other.transform.DOShakeRotation(0.5f, 10, 8, 70);
        }

        if (other.CompareTag("Resource"))
        {
            other.GetComponent<Wood>().health -= attackDamage;
            other.transform.DOShakeRotation(0.5f, 10, 8, 70);
        }

        if (other.CompareTag("Player"))
        {
            //other.GetComponent<Enemy>().health -= 1;
            other.transform.DOShakeRotation(0.1f, 10, 8, 70);
        }
    }

}
