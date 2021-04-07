using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponCollision : MonoBehaviour
{

    public float attackDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().health -= attackDamage;
            other.transform.DOShakeRotation(0.5f, 10, 8, 70);
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
