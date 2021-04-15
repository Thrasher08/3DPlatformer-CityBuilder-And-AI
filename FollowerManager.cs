using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    //[SerializeField] List<Followers> selectedActors = new List<Followers>();
    [SerializeField] Followers[] followers;

    public Transform selectionPoint;
    public LayerMask isGround;

    public float selectionRange = 3.5f;
    public float commandRange = 3.25f;
    Transform playerPos;

    Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        followers = FindObjectsOfType<Followers>();
        playerPos = FindObjectOfType<PlayerController>().transform;
        baseColor = selectionPoint.gameObject.GetComponent<Renderer>().material.GetColor("_HologramColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            UpdateSelectionPosition();
            allowFollow();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            sendToFindJob();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            sendToBuild();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            disableFollowAll();
        }

        if (Input.GetMouseButton(2))
        {
            UpdateSelectionPosition();
            disableFollow();
        }

        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            selectionPoint.gameObject.SetActive(false);
        }

    }

    void allowFollow()
    {
        selectionPoint.gameObject.GetComponent<Renderer>().material.SetColor("_HologramColor", baseColor);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            for (int i = 0; i < followers.Length; i++)
            {
                if (Vector3.Distance(followers[i].transform.position, hit.point) <= selectionRange)
                {
                    followers[i].allowFollow = true;
                }
            }
        }
    }


    void disableFollowAll()
    {
        for (int i = 0; i < followers.Length; i++)
        {
            followers[i].allowFollow = false;
        }
    }

    // Any followers within range will stop following
    void disableFollow()
    {
        selectionPoint.gameObject.GetComponent<Renderer>().material.SetColor("_HologramColor", Color.red);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            for (int i = 0; i < followers.Length; i++)
            {
                if (Vector3.Distance(followers[i].transform.position, hit.point) <= selectionRange)
                {
                    followers[i].allowFollow = false;
                    followers[i].agent.SetDestination(followers[i].transform.position);
                }
            }
        }
    }

    void sendToBuild()
    {
        for (int i = 0; i <= followers.Length; i++)
        {
            if (Vector3.Distance(followers[i].transform.position, playerPos.position) <= commandRange)
            {
                StartCoroutine(followers[i].BuildJob());
                break;
            }
        }
    }

    void sendToFindJob()
    {
        for (int i = 0; i <= followers.Length; i++)
        {
            if (Vector3.Distance(followers[i].transform.position, playerPos.position) <= commandRange)
            {
                StartCoroutine(followers[i].FindJob());
                break;
            }
        }
    }

    void UpdateSelectionPosition()
    {
        selectionPoint.gameObject.SetActive(true);
        selectionPoint.localScale = new Vector3(selectionRange, selectionPoint.localScale.y, selectionRange);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, isGround))
        {
            selectionPoint.position = hit.point;
        }
    }

}
