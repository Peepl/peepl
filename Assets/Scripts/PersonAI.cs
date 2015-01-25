using UnityEngine;
using System.Collections;

public class PersonAI : MonoBehaviour {

    public GameObject Leader;

    private Vector3 offset;
    public static float Radius = 25.0f;
    public static float MaxVelo = 5.0f;

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
	}
	
    void OnCollisionEnter(Collision collision)
    {
        if ( !collision.gameObject.tag.Equals("person") && !collision.gameObject.tag.Equals("Ground") )
        {
            rigidbody.AddForce(10.0f * (transform.position - collision.gameObject.transform.position) );
        }
    }

	// Update is called once per frame
	void Update () {
        if (Random.Range(1, 300) < 1)
        {
            offset = GetRandomOffset();
        }

        Vector3 v = offset + Leader.transform.position - transform.position;
        float mag = 0.0f;
        if (v.magnitude > 10.0f && v.magnitude < 150.0f)
        {
            mag = 200.0f / Mathf.Max(1.0f, v.magnitude);
        }
        v.Normalize();
        rigidbody.AddForce( mag * v );

        if (rigidbody.velocity.magnitude > MaxVelo)
        {
            rigidbody.velocity = MaxVelo * rigidbody.velocity.normalized;
        }
	}
}
