using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    float baseDamage;
    public float damageMultiplier = 2.5f;
    
    public float buildingRange = 5;
    
    float playerDist;
    public Transform playerTransform;

    public LayerMask isPlayer;

    [SerializeField]
    bool withinProx;
    [SerializeField]
    static bool recievedBuff;

    [SerializeField]
    Building[] buildings;

    WeaponCollision weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = FindObjectOfType<WeaponCollision>();
        baseDamage = WeaponCollision.attackDamage;
        //baseDamage = weapon.attackDamage;

        buildings = FindObjectsOfType<Building>();

        //isPlayer = FindObjectOfType<PlayerController>().gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        //Mathf.Clamp(WeaponCollision.attackDamage, 1, 2);

        withinProx = Physics.CheckSphere(transform.position, buildingRange, isPlayer);

        if (withinProx && !recievedBuff)
        {
            //Collider[] col = Physics.OverlapSphere(transform.position, buildingRange, isPlayer);
            //playerTransform = col[0].transform;

            WeaponCollision.attackDamage *= damageMultiplier;
            recievedBuff = true;
        }

        if (recievedBuff)
        {
            playerDist = Vector3.Distance(transform.position, playerTransform.transform.position);
            if (playerDist > buildingRange)
            {
                buildings = FindObjectsOfType<Building>();
                bool withinProxOfOthers = false;
                for (int i = 0; i < buildings.Length; i++)
                {
                    if (buildings[i].withinProx)
                    {
                        withinProxOfOthers = true;
                        break;
                    }
                }

                if (withinProxOfOthers == false)
                {
                    WeaponCollision.attackDamage = baseDamage;
                    recievedBuff = false;
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, buildingRange);         // Draw Building Range
    }
}
