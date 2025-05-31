using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	Transform player;

	public bool isFlipped = false;

    //private bool isAttacking;

    public LayerMask playerLayers;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    

    void Start()
    {
        //finds nescessary components and sets them to a variable
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(0f, 0f, 0f); 
    }

    public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

    public void BossAttack()
    {
        //isAttacking = true;
       
        
        //Detect players in range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);//creates a circle around a point

        //Damage the player
        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("We hit " + player.name);
            player.GetComponent<Player>().TakeDamage(attackDamage);
        }

        //yield return new WaitForSeconds(1);

        //isAttacking = false;
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
