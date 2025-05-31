using UnityEngine;
using System.Collections;

public class Sensor_Bandit : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;

    public AudioSource landSound;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            m_ColCount++;
            landSound.Play();
            Debug.Log("Landed");
        }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
            m_ColCount--;
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
