using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    private PlayerCombat thePlayerCombat;

    private SpriteRenderer      m_theSpriteRenderer;
    private Animator            m_animator;
    private Rigidbody2D         m_rigidBody;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    public int maxHealth = 100;
    int currentHealth;

    public HealthBar healthBar; 

    public bool canMove;

    public GameObject gameOverScreen;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;

    private int killCount;
    public Text killCountText;

    public float invincibilityLength;
    private float invincibilityCounter;
    private bool invincible = false; 

    // Use this for initialization
    void Start () {
        thePlayerCombat = FindObjectOfType<PlayerCombat>();
        //healthBar = FindObjectOfType<HealthBar>();

        m_theSpriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();

        canMove = true;
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth); 
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (knockbackCounter <= 0)
        {
            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            /*if (inputX > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);*/

            // Move
            m_rigidBody.velocity = new Vector2(inputX * m_speed, m_rigidBody.velocity.y);

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_rigidBody.velocity.y);

            
            if (canMove)
            {
                //Jump
                if (Input.GetButtonDown("Jump") && m_grounded)
                {
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, m_jumpForce);
                    m_groundSensor.Disable(0.2f);
                }

                //Run
                else if (Mathf.Abs(inputX) > Mathf.Epsilon)
                {
                    m_animator.SetInteger("AnimState", 2);
                    if (inputX > 0)
                    {
                        m_theSpriteRenderer.flipX = true;
                        thePlayerCombat.isFacingRight = false;
                        thePlayerCombat.attackPoint.transform.localPosition = new Vector3(0.4f, 0.68f, 0f);
                    }
                    else
                    {
                        m_theSpriteRenderer.flipX = false;
                        thePlayerCombat.isFacingRight = true;
                        thePlayerCombat.attackPoint.transform.localPosition = new Vector3(-0.4f, 0.68f, 0f);
                    }
                }

                //Combat Idle
                else if (m_combatIdle)
                    m_animator.SetInteger("AnimState", 0);

                //Idle
                else
                    m_animator.SetInteger("AnimState", 1);
            }
            
        }

        

        //checks if the player has been knocked back
        if (knockbackCounter > 0)
        {
            //makes our knockback counter count down for each frame that passes
            knockbackCounter -= Time.deltaTime;

            //checks what direction the player is facing and makes the knockback be the opposite of that direction.
            if (!thePlayerCombat.isFacingRight)
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

        //makes invincibilty count down
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }

        //turns off invincibility  when count down reaches zero or less
        if (invincibilityCounter <= 0)
        {
            invincible = false;
        }

        

       
    }

    public void Knockback()
    {
        //sets the knockback counter
        knockbackCounter = knockbackLength;
        //sets the invincibility counter
        invincibilityCounter = invincibilityLength;
        //makes player invincible for a little bit during and after knockback
        invincible = true;
    }


    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            currentHealth -= damage;
            healthBar.setHealth(currentHealth); 
            Debug.Log("Player health:" + currentHealth);
            m_animator.SetTrigger("Hurt");
             

            //play hurt animation

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());

            }
            else
            {
                Knockback();
            }
        }
    }

    IEnumerator Die()
    {
        m_isDead = true;

        Debug.Log("Player died");
        m_animator.SetTrigger("Death");
        

        //m_rigidBody.gravityScale = 0;
        canMove = false;
        invincible = true;
        thePlayerCombat.enabled = false;
        m_rigidBody.velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Collider2D>().enabled = false;
        m_rigidBody.bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        yield return new WaitForSeconds(1f);
        //Freezes the game
        Time.timeScale = 0;
        

        gameOverScreen.SetActive(true);
        m_isDead = false; 
        //this.enabled = false;

        yield break; 
            
    }

    public void UpdateKillCount()
    {
        killCount += 1;
        killCountText.text = "Kill Count: " + killCount;
    }

    public void Heal(int healthToGive)
    {
        currentHealth += healthToGive;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.setHealth(currentHealth);
        Debug.Log("Player health:" + currentHealth);
    }


}
