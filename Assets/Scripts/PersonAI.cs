using UnityEngine;
using System.Collections;

public class PersonAI : MonoBehaviour {

    public GameObject Leader;

    private Vector3 offset;
    private GameObject m_Swarm;
    private float cachedAngle;

    public static float Radius = 15.0f;
    public static float MaxVelo = 7.0f;

    public bool IsSheltered;

    public static Vector3 GetRandomOffset()
    {
        Vector3 r = new Vector3();
        do
        {
            r.x = Random.Range(-Radius, Radius);
            r.z = Random.Range(-Radius, Radius);
        } while (r.magnitude > Radius * Radius);
        return r;
    }
	// Use this for initialization
	void Start () {
        offset = GetRandomOffset();
        m_Swarm = GameObject.Find("Swarm");
	}
	
    void OnCollisionEnter(Collision collision)
    {
        if ( !collision.gameObject.tag.Equals("person") && !collision.gameObject.tag.Equals("Ground") )
        {
            rigidbody.AddForce(10.0f * (transform.position - collision.gameObject.transform.position) );
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (Random.Range(1, 300) < 1)
        {
            offset = GetRandomOffset();
        }

        //Vector3 v = offset + Leader.transform.position - transform.position;
        Vector3 v = Leader.transform.position - transform.position;
        float mag = 0.0f;
        float xzDistSquared = v.x * v.x + v.z * v.z;
        if (xzDistSquared < 40.0f)
        {
            mag = -100.0f / Mathf.Max(1.0f, xzDistSquared);
        } else if (xzDistSquared < 2000000.0f)
        {
            float morale = 0.01f * m_Swarm.GetComponent<SwarmAI>().Morale;
            mag = 100.0f * Mathf.Sqrt(morale) * 2.5f / Mathf.Sqrt(xzDistSquared);
        }
        v.Normalize();
        //rigidbody.AddForce( morale * mag * v );
        rigidbody.AddForce(mag * v);
        rigidbody.AddForce(1.1f * offset);

        if (rigidbody.velocity.magnitude > MaxVelo)
        {
            rigidbody.velocity = MaxVelo * rigidbody.velocity.normalized;
        }

        if (rigidbody.velocity.magnitude > 1.0f)
        {
            cachedAngle = Mathf.Atan2(rigidbody.velocity.x, rigidbody.velocity.z);
        }

        transform.localEulerAngles = new Vector3(0.0f, 180.0f * cachedAngle / 3.14159f, 0.0f);
    }
}
