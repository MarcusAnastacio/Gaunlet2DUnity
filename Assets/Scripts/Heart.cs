using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{

    private Player thePlayer;

    public int healthToGive;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //If the player touches the pick up, this runs the GiveHealth function and deactivates the pickup
        if (other.tag == "Player")
        {
            thePlayer.Heal(healthToGive);
            gameObject.SetActive(false);
        }
    }
}
