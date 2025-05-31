using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditEnemy : MonoBehaviour
{

    public float speed = 2.5f;


    Transform player;
    Rigidbody2D rb;
    private Animator m_animator;

    private bool isAttacking;

    public LayerMask playerLayers;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        lookAtPlayer(); 
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        m_animator.SetInteger("AnimState", 2);

        if (Vector2.Distance(player.position, rb.position) <= attackRange && !isAttacking)
        {
            isAttacking = true;
            m_animator.SetTrigger("Attack"); 
        }

        

    }

    public void BanditAttack()
    {
        
        //yield return new WaitForSeconds(0.5f);
        //Detect enemies in range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);//creates a circle around a point

        //Damage those enemies
        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("We hit " + player.name);
            player.GetComponent<Player>().TakeDamage(attackDamage);
        }

        //yield return new WaitForSeconds(1);
        //m_animator.SetInteger("AnimState", 1);
        isAttacking = false;
    }

    public void lookAtPlayer()
    {
        if (transform.position.x > player.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 0f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 0f);
        }
    }
}
