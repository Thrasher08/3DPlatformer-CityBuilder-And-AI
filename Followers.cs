using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Followers : MonoBehaviour
{

    public Transform targetLocation;
    NavMeshAgent agent;

    bool assignedJob = false;
    bool jobInSight = false;

    BuildingManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        buildManager = FindObjectOfType<BuildingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!assignedJob)
        {
            agent.SetDestination(targetLocation.transform.position);

            if (Input.GetKeyDown(KeyCode.V))
            {
                StartCoroutine(FindJob());
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(BuildJob());
            }
        }

    }

    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }

    IEnumerator FindJob()
    {
        assignedJob = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            agent.SetDestination(hit.point);
        }
        yield return WaitForNavMesh();
        Collider[] col = Physics.OverlapSphere(transform.position, 5f);
        Vector3 newJobDest = new Vector3();
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].CompareTag("Resource"))
            {
                jobInSight = true;
                newJobDest = col[i].transform.position;
            }
        }
        if (jobInSight)
        {
            agent.SetDestination(newJobDest);
            yield return WaitForNavMesh();
            yield return new WaitForSeconds(2);

            Vector3 buildingLocation = new Vector3(newJobDest.x, newJobDest.y * 2, newJobDest.z);

            Building building;
            building = Instantiate(buildManager.buildingPrefabs[0], buildingLocation, Quaternion.identity);
            //building.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1);
            //building.GetComponent<Rigidbody>().useGravity = true;
        }
        assignedJob = false;
    }

    IEnumerator BuildJob()
    {
        assignedJob = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            agent.SetDestination(hit.point);
        }

        yield return WaitForNavMesh();
        yield return new WaitForSeconds(2);

        Vector3 buildingLocation = new Vector3(hit.point.x, agent.transform.position.y * 2, hit.point.z);
        
        Building building;
        building = Instantiate(buildManager.buildingPrefabs[0], buildingLocation, Quaternion.identity);

        yield return new WaitForSeconds(1);

        assignedJob = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);         // Draw Sight Range
    }
}
