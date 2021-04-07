using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public Transform carryPosition;

    Rigidbody rb;
    CarryManager manager;

    public float throwForce = 300;

    public GameObject[] itemDrop;

    public float pickUpRange = 5;
    public LayerMask isPlayer;

    [SerializeField]
    bool canBePickedUp;
    [SerializeField]
    bool objectHeld = false;

    public bool destructable;
    bool destructableThrown;
    [SerializeField] bool containsItem;
    public ParticleSystem destroyParticle;

    //CarryManager manager;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        manager = FindObjectOfType<CarryManager>();
        carryPosition = GameObject.Find("Carry Position").transform;

        if (destructable)
        {
            containsItem = (Random.value > 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (manager.isCarrying == false)
        {
            canBePickedUp = Physics.CheckSphere(transform.position, pickUpRange, isPlayer);
        }

        if (manager.isCarrying == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && canBePickedUp)
            {
                this.gameObject.transform.position = carryPosition.transform.position;
                this.gameObject.transform.SetParent(carryPosition);
                //this.gameObject.layer = 10;

                rb.velocity = new Vector3(0, 0, 0);
                rb.useGravity = false;

                objectHeld = true;
                manager.isCarrying = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && manager.isCarrying == true && objectHeld)
        {
            this.gameObject.transform.position = carryPosition.transform.position;
            this.gameObject.transform.parent = null;
            //this.gameObject.layer = 0;

            rb.useGravity = true;
            rb.AddForce((carryPosition.forward + carryPosition.up) * throwForce);

            objectHeld = false;
            manager.isCarrying = false;

            if (destructable)
            {
                destructableThrown = true;
                rb.velocity += Vector3.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
            }
        }

        if (manager.isCarrying == true && objectHeld)
        {
            this.gameObject.transform.position = carryPosition.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destructableThrown)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<Enemy>().health -= 1;
            }

            if (containsItem)
            {
                int itemToDrop = Random.Range(0, itemDrop.Length);
                GameObject item = Instantiate(itemDrop[itemToDrop], this.transform);
                item.transform.parent = null;
            }

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
