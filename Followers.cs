using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Followers : MonoBehaviour
{

    Transform targetLocation;
    NavMeshAgent agent;

    float jobRadius = 1.5f;

    bool assignedJob = false;
    bool jobInSight = false;

    public bool allowFollow = false;

    BuildingManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        buildManager = FindObjectOfType<BuildingManager>();
        targetLocation = GameObject.Find("Follow Position").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!assignedJob && allowFollow)
        {
            agent.SetDestination(targetLocation.transform.position);
        }

    }

    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }

    public IEnumerator FindJob()
    {
        assignedJob = true;
        Collider target = new Collider();

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
            if (col[i].CompareTag("Resource") || col[i].CompareTag("Enemy"))
            {
                jobInSight = true;
                newJobDest = col[i].transform.position;
                target = col[i];
                break;
            }
        }

        if (jobInSight)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * jobRadius;
            newJobDest.x += randomOffset.x;
            newJobDest.z += randomOffset.y;

            agent.SetDestination(newJobDest);
            yield return WaitForNavMesh();
            yield return new WaitForSeconds(2);

            yield return StartCoroutine(AttackJob(target));

            yield return new WaitForSeconds(1);

        }

        assignedJob = false;
    }

    public IEnumerator BuildJob()
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
        if (buildManager.rm.woodResource >= 10)
        {
            building = Instantiate(buildManager.buildingPrefabs[0], buildingLocation, Quaternion.identity);
            buildManager.rm.woodResource -= 10;
        }
        
        yield return new WaitForSeconds(1);

        assignedJob = false;
    }

    IEnumerator AttackJob(Collider target)
    {
        if (target.CompareTag("Resource"))
        {
            float totalHealth = target.GetComponent<Wood>().health - 1;

            for (int i = 0; i <= totalHealth; i++)
            {
                target.GetComponent<Wood>().health -= 1;
                target.transform.DOShakeRotation(0.5f, 10, 8, 70);
                yield return new WaitForSeconds(1);
            }
        }
        else if (target.CompareTag("Enemy"))
        {
            float totalHealth = target.GetComponent<Enemy>().health - 1;

            for (int i = 0; i <= totalHealth; i++)
            {
                // If the enemy moves out of range move towards it
                if (Vector3.Distance(target.transform.position, agent.transform.position) >= jobRadius)
                {
                    agent.SetDestination(target.transform.position);
                    yield return WaitForNavMesh();
                }
                target.GetComponent<Enemy>().health -= 1;
                target.transform.DOShakeRotation(0.5f, 10, 8, 70);
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);         // Draw Sight Range
    }
}
