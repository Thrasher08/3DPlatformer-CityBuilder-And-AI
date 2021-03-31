using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{

    public float health = 5;
    public int resourceValue = 10;
    ResourceManager rm;

    // Start is called before the first frame update
    void Start()
    {
        rm = FindObjectOfType<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            rm.woodResource += resourceValue;
            this.gameObject.SetActive(false);
        }
    }
}
