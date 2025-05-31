using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    private SpriteRenderer m_theSpriteRenderer;

    public int maxHealth;
    public int currentHealth;

    private int heartDrop;
    public Transform heart;
    private bool invulnerable = false;

    private Player thePlayer; 

    public float knockbackForce;
    public float knockbackLength;
    public float knockbackCounter;
    private bool knockbackIsRight;
    //private Vector3 lastPosition;
    //private Transform myTransform; 

    private Animator m_animator;
    private Rigidbody2D m_rigidBody;


    // Start is called before the first frame update
    void Start()
    {
       // myTransform = transform;
       // lastPosition = myTransform.position;

        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_theSpriteRenderer = GetComponent<SpriteRenderer>();

        thePlayer = FindObjectOfType<Player>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (knockbackCounter > 0 && currentHealth > 0)
        {
            //makes our knockback counter count down for each frame that passes
            knockbackCounter -= Time.deltaTime;

            //checks what direction the player is facing and makes the knockback be the opposite of that direction.
            if (knockbackIsRight)
            {
                //sends the player flying back diagnoly to the left
                m_rigidBody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
            }
            else
            {
                //sends the player flying back diagnoly to the right
                m_rigidBody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);
            }
        }
  

        /*else if (knockbackCounter <= 0 && !isGrounded)
        {
            //checks what direction the player is facing and makes the knockback be the opposite of that direction.
            if (knockbackIsRight)
            {
                //sends the player flying back diagnoly to the left
                m_rigidBody.velocity = new Vector3(-knockbackForce, -knockbackForce, 0f);
            }
            else
            {
                //sends the player flying back diagnoly to the right
                m_rigidBody.velocity = new Vector3(knockbackForce, -knockbackForce, 0f);
            }
        }*/
    }

    public void Knockback()
    {
        //sets the knockback counter
        knockbackCounter = knockbackLength;

    }

    public void TakeDamage(int damage, bool direction)
    {
        if (!invulnerable)
        {
            currentHealth -= damage;
            Debug.Log("Enemy Health: " + currentHealth);
            m_animator.SetTrigger("Hurt");
            knockbackIsRight = direction;


            Knockback();

            //play hurt animation

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());

            }
        }
        
    }

    private IEnumerator Die()
    {
        invulnerable = true; 
        Debug.Log("Enemy died");
        m_animator.SetTrigger("Death");
        
        m_rigidBody.velocity = new Vector3(0f, 0f, 0f);

        heartDrop = Random.Range(0, 4);
        if (heartDrop == 3)
        {
            Instantiate(heart, transform.position, transform.rotation);
        }

        if (GetComponent<Bat>() != null)
        {
            m_rigidBody.gravityScale = 1;
            GetComponent<Bat>().enabled = false;
            m_theSpriteRenderer.flipY = true;
            GetComponent<Collider2D>().offset = new Vector2(0.03f, 0.18f);
            GetComponentInChildren<AudioSource>().Stop();
        }
        if (GetComponent<EnemyAI>() != null)
        {
            thePlayer.UpdateKillCount();
            GetComponent<EnemyAI>().enabled = false;
            Debug.Log("AI Disabled");
        }

        if (GetComponent<Spider>() != null)
        {
            thePlayer.UpdateKillCount();
            GetComponent<Spider>().enabled = false;
            Debug.Log("Spider Disabled");
        }

        if (GetComponent<Boss>() != null)
        {
            thePlayer.UpdateKillCount();
            GetComponent<Boss>().enabled = false;
        }
        

        yield return new WaitForSeconds(0.5f);

        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
        }
        

        

        

        //m_rigidBody.gravityScale = 0;
        m_rigidBody.bodyType = RigidbodyType2D.Static;


        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

        this.enabled = false; 
       
    }
}
