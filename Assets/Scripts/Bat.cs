using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{

    public int attackDamage;

    private Player thePlayer;
    private Enemy enemy;


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" && enemy.currentHealth > 0)
        {
            Debug.Log("Bat hit Player");
            thePlayer.TakeDamage(attackDamage);
        }

        
    }
}
