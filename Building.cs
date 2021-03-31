using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    float baseDamage;
    public float damageMultiplier = 2;

    public float buildingRange = 5;
    public LayerMask isPlayer;

    [SerializeField]
    bool withinProx;
    [SerializeField]
    bool recievedBuff;

    [SerializeField]
    Building[] buildings;

    WeaponCollision weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = FindObjectOfType<WeaponCollision>();
        baseDamage = weapon.attackDamage;

        buildings = FindObjectsOfType<Building>();

        //isPlayer = FindObjectOfType<PlayerController>().gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        withinProx = Physics.CheckSphere(transform.position, buildingRange, isPlayer);

        if (withinProx && !recievedBuff)
        {
            weapon.attackDamage *= damageMultiplier;
            recievedBuff = true;
        }

        if (recievedBuff)
        {

        }

        //Need to check if within any other buildings, a for loop

        if (!withinProx)
        {
            buildings = FindObjectsOfType<Building>();
            bool withinProxOfOthers = false;
            for (int i = 0; i < buildings.Length; i++)
            {
                if (buildings[i].withinProx)
                {
                    withinProxOfOthers = true;
                }
            }

            if (withinProxOfOthers == false)
            {
                weapon.attackDamage = baseDamage;
                recievedBuff = false;
            }
            //weapon.attackDamage = baseDamage;
            //recievedBuff = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, buildingRange);         // Draw Building Range
    }
}
