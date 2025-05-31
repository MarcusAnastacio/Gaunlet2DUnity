using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    
    public LayerMask enemyLayers;

    private bool isAttacking;
    public bool isFacingRight;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public AudioSource attackSound;
    

    void Start()
    {
        isAttacking = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                //Plays an attack animation
                animator.SetTrigger("Attack");
                attackSound.Play();
                nextAttackTime = Time.time + 1f / attackRate; 
            }
        }
    }

    public void Attack()
    {
        isAttacking = true;
        
        
        
        //yield return new WaitForSeconds(0.25f);

        //Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);//creates a circle around a point

        //Damage those enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, isFacingRight);
        }

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
