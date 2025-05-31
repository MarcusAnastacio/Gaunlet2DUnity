using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public int attackDamage;

    public float moveSpeed;
    private bool canMove;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    private Rigidbody2D myRigidbody;

    private Player thePlayer;
    private Enemy enemy;  

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        thePlayer = FindObjectOfType<Player>();

        enemy = GetComponent<Enemy>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (canMove && enemy.knockbackCounter <= 0 && isGrounded)
        {
            //moves the spider enemy if canMove is true
            myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
            
            if (moveSpeed > 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 0f);
            }

            else
            {
                transform.localScale = new Vector3(1f, 1f, 0f);
            }
        }

    }

    //happens when object enters the cameras view
    void OnBecameVisible()
    {
        //makes the spider able to move
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Player")
        {
            Debug.Log("Spider hit Player");
            thePlayer.TakeDamage(attackDamage);
        }

        if (other.tag == "Wall" && isGrounded)
        {
            Debug.Log("Spider hit a wall");
            moveSpeed = -moveSpeed; 
        }
    }

    //happens whenever an object is enabled/set active
    void OnEnable()
    {
        //makes it so the spider doesn't move while the player can't see it
        canMove = false;
    }

    
}
