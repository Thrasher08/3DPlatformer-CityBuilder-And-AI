using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public Transform carryPosition;

    Rigidbody rb;
    CarryManager manager;

    public float throwForce = 300;

    GameObject heldItem;

    public float pickUpRange = 5;
    public LayerMask isCarry;

    [SerializeField]
    bool canBePickedUp;
    [SerializeField]
    bool objectHeld = false;

    //CarryManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<CarryManager>();
        carryPosition = GameObject.Find("Carry Position").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (manager.isCarrying == false)
        {
            canBePickedUp = Physics.CheckSphere(transform.position, pickUpRange, isCarry);
        }

        if (manager.isCarrying == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && canBePickedUp)
            {
                Collider[] col = Physics.OverlapSphere(transform.position, pickUpRange, isCarry);
                float closest = pickUpRange + 1;

                for (int i = 0; i < col.Length; i++)
                {
                    float dist = Vector3.Distance(col[i].transform.position, transform.position);
                    if (dist <= closest)
                    {
                        closest = dist;
                        heldItem = col[i].gameObject;
                        //break;
                    }
                }

                heldItem.transform.position = carryPosition.transform.position;
                heldItem.transform.SetParent(carryPosition);
                heldItem.gameObject.layer = 12;

                rb = heldItem.GetComponent<Rigidbody>();
                rb.velocity = new Vector3(0, 0, 0);
                rb.useGravity = false;

                objectHeld = true;
                manager.isCarrying = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && manager.isCarrying == true && objectHeld)
        {
            heldItem.gameObject.transform.position = carryPosition.transform.position;
            heldItem.gameObject.transform.parent = null;
            heldItem.gameObject.layer = 10;

            rb.useGravity = true;
            rb.AddForce((carryPosition.forward + carryPosition.up) * throwForce);

            objectHeld = false;
            manager.isCarrying = false;

            if (heldItem.GetComponent<DestructableObject>())
            {
                heldItem.GetComponent<DestructableObject>().thrown = true;
                rb.velocity += Vector3.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
            }

            heldItem = null;
        }

        if (manager.isCarrying == true && objectHeld)
        {
            heldItem.gameObject.transform.position = carryPosition.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);         // Draw Pickup Range
    }
}
