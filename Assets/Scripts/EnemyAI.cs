using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 1f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    private Animator m_animator;

    private bool isAttacking;

    public LayerMask playerLayers;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public AudioSource attackSound;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found");
        }

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        isAttacking = false; 
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
            
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++; 
        }

        //will flip the enemy to face the direction it's moving and also set the running animation
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 0f);
            m_animator.SetInteger("AnimState", 2);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 0f);
            m_animator.SetInteger("AnimState", 2);
        }

        
        /*else
        {
            string gameObjectName = gameObject.name;

            if (!isAttacking && (gameObjectName == "BanditEnemy" || gameObjectName == "BanditEnemy(Clone)"))
            {
                m_animator.SetTrigger("Attack");
               
            }
            
        }*/

        if (Vector2.Distance(target.position, rb.position) <= attackRange && !isAttacking)
        {
            string gameObjectName = gameObject.name;

            if (gameObjectName == "BanditEnemy" || gameObjectName == "BanditEnemy(Clone)")
            {
                m_animator.SetTrigger("Attack");
                

            }
        }

    }

    public void Attack()
    {
        isAttacking = true;
        attackSound.Play();
        //m_animator.SetTrigger("Attack");
        
        //yield return new WaitForSeconds(0.5f);
        //Detect players in range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);//creates a circle around a point

        //Damage the player
        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("We hit " + player.name);
            player.GetComponent<Player>().TakeDamage(attackDamage);
        }

        //yield return new WaitForSeconds(1);
        
        //m_animator.SetInteger("AnimState", 1);
        isAttacking = false;      
    }

    void OnDrawGizmosSelected()
    {
        //makes sure there's no errors
        if (attackPoint == null)
            return;

        //draws a circle so we can see the attackRange
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
