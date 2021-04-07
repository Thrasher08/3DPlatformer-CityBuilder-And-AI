using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public Building[] buildingPrefabs;
    public ResourceManager rm;

    public bool recievedDamageBuff;

    // Start is called before the first frame update
    void Start()
    {
        rm = FindObjectOfType<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (rm.woodResource >= 10)
                {
                    Building building = buildingPrefabs[0];
                    building = Instantiate(buildingPrefabs[0], hit.point, Quaternion.identity);
                    rm.woodResource -= 10;
                }

            }

        }*/
    }
}
