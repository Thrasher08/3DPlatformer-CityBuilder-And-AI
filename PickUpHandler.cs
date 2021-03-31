using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public Transform carryPosition;

    Rigidbody rb;

    public float throwForce = 300;

    public float pickUpRange = 5;
    public LayerMask isPlayer;

    [SerializeField]
    bool canBePickedUp;
    [SerializeField]
    bool isCarrying = false;

    public bool destructable;
    bool destructableThrown;
    public ParticleSystem destroyParticle;

    //CarryManager manager;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        //manager = FindObjectOfType<CarryManager>();
        carryPosition = GameObject.Find("Carry Position").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isCarrying == false)
        {
            canBePickedUp = Physics.CheckSphere(transform.position, pickUpRange, isPlayer);
        }

        if (isCarrying == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && canBePickedUp)
            {
                this.gameObject.transform.position = carryPosition.transform.position;
                this.gameObject.transform.SetParent(carryPosition);
                //this.gameObject.layer = 10;

                rb.velocity = new Vector3(0, 0, 0);
                rb.useGravity = false;

                isCarrying = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && isCarrying == true)
        {
            this.gameObject.transform.position = carryPosition.transform.position;
            this.gameObject.transform.parent = null;
            //this.gameObject.layer = 0;

            rb.useGravity = true;
            rb.AddForce((carryPosition.forward + carryPosition.up) * throwForce);

            isCarrying = false;

            if (destructable)
            {
                destructableThrown = Physics.CheckSphere(transform.position, pickUpRange);
                rb.velocity += Vector3.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
            }
        }

        if (isCarrying == true)
        {
            this.gameObject.transform.position = carryPosition.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destructableThrown)
        {
            Transform particlePos = this.transform;
            ParticleSystem particle = Instantiate(destroyParticle, particlePos);
            particle.transform.parent = null;
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);         // Draw Pickup Range
    }
}
