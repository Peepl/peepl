using UnityEngine;
using System.Collections;


public class DeathTrap : MonoBehaviour {

    public int Threshold = 20;
    public float Radius = 10.0f;
    private int m_Count;
    private bool m_Active;
    private long m_DeactivationTime;

	// Use this for initialization
	void Start () {
        m_Count = 0;
        m_Active = true;
	}
	
    void OnTriggerEnter(Collider collider)
    {
        if ( collider.gameObject.tag.Equals("Person") )
        {
            m_Count++;

            if (m_Active && m_Count > Threshold) 
            {
                Debug.Log("baa");
                Explode();
                Debug.Log("boo");
            }
        }

    }

    void Explode()
    {
        m_Active = false;
        renderer.material.color = Color.yellow;
        m_DeactivationTime = System.DateTime.Now.Ticks;
        GameObject.Find("Swarm").GetComponent<SwarmAI>().KillInRadius(transform.position, 25.0f, 0.5f);
        m_Count = 0;

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Person"))
        {
            m_Count--;
        }
    }

    // Update is called once per frame
	void Update () {
        if ( !m_Active && System.DateTime.Now.Ticks - m_DeactivationTime > 100000000 )
        {
            renderer.material.color = Color.white;
            m_Active = true;
        }
	}
}
