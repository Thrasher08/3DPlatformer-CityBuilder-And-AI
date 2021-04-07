using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    PickUpHandler handler;
    
    public bool thrown = false;
    
    bool containsItem;
    public GameObject[] itemDrop;
    public ParticleSystem destroyParticle;

    private void Start()
    {
        handler = FindObjectOfType<PickUpHandler>();
        containsItem = (Random.value > 0.5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<Enemy>().health -= 1;
            }

            DestroyObject();
        }
    }

    void DestroyObject()
    {
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
