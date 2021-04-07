using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    
    public Animator anim;
    public Collider swordCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)))
        {
            anim.SetBool("attacking", true);
            anim.SetTrigger("attack");
            //swordCol.enabled = true;
        }

        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
            //anim.SetBool("attacking", false);
            //swordCol.enabled = false;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Enemy"))
        //{
            //targets.Add(other.transform);
        //}
    }
}
