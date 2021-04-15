using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Cinemachine;

public class Enemy : MonoBehaviour
{

    Animator anim;
    public GameObject visual;

    public float health;

    public Transform targetLocation;
    NavMeshAgent agent;

    public float sightRange;
    bool playerInSight;

    public float attackRange;
    bool playerInAttackRange;

    public LayerMask isPlayer;

    public float attackCooldown = 2f;
    [SerializeField]
    float attackTimer = 0;
    [SerializeField]
    bool canAttack = true;
    bool dead = false;
    public ParticleSystem destroyParticle;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        playerInSight = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (playerInSight && !playerInAttackRange)
        {
            agent.SetDestination(targetLocation.transform.position);
        }
        
        if (playerInAttackRange)
        {
            agent.SetDestination(transform.position);
            Vector3 targetLookOffset = new Vector3(targetLocation.position.x, 1, targetLocation.position.z);
            transform.LookAt(targetLookOffset);
            
            if (canAttack)
            {
                anim.SetTrigger("attack");
                canAttack = false;
                attackTimer = attackCooldown;
            }

        }

        if (health <= 0 && dead == false)
        {
            dead = true;
            StartCoroutine(Kill());
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            canAttack = true;
        }

        if (!playerInSight && !playerInAttackRange)
        {
            agent.SetDestination(transform.position);
        }

    }

    private void ResetAttack()
    {
        
    }

    IEnumerator Kill()
    {
        sightRange = 0;
        attackRange = 0;

        Rigidbody rb;
        rb = this.gameObject.AddComponent<Rigidbody>();
        Vector3 direction = new Vector3(-10, 0, -10);
        rb.velocity = (-this.transform.forward * 5);

        Sequence killSequence = DOTween.Sequence();

        killSequence.Append(visual.transform.DORotate(new Vector3(0, 360, 0), 0.85f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear));
        killSequence.Append(visual.transform.DOShakeScale(0.5f, 1, 8, 70));

        yield return new WaitForSeconds(0.85f);
        rb.velocity = new Vector3(0, 0, 0);

        yield return killSequence.WaitForCompletion();
        Camera.main.transform.DOShakePosition(0.2f, 15.5f, 14, 90, false, true);

        Transform particlePos = this.transform;
        ParticleSystem particle = Instantiate(destroyParticle, particlePos);
        particle.transform.parent = null;

        visual.SetActive(false);
        CameraShake cShake = FindObjectOfType<CameraShake>();
        yield return StartCoroutine(cShake.Shake(10));
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);         // Draw Sight Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);         // Draw Sight Range
    }
}
