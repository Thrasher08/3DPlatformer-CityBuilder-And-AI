using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    Animator anim;

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

        if (health <= 0)
        {
            Transform particlePos = this.transform;
            ParticleSystem particle = Instantiate(destroyParticle, particlePos);
            particle.transform.parent = null;
            Destroy(this.gameObject);
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            //anim.SetBool("attacking", false);
            //attackTimer = 0;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);         // Draw Sight Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);         // Draw Sight Range
    }
}
