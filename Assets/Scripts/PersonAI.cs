﻿using UnityEngine;
using System.Collections;

public class PersonAI : MonoBehaviour {

    public GameObject Leader;

    private Vector3 offset;
    private GameObject m_Swarm;

    public static float Radius = 15.0f;
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
        } else if (xzDistSquared < 1000000.0f)
        {
            float morale = m_Swarm.GetComponent<SwarmAI>().Morale;
            mag = morale * 2.0f / Mathf.Sqrt(xzDistSquared);
        }
        v.Normalize();
        //rigidbody.AddForce( morale * mag * v );
        rigidbody.AddForce(mag * v);
        rigidbody.AddForce(1.2f * offset);

        if (rigidbody.velocity.magnitude > MaxVelo)
        {
            rigidbody.velocity = MaxVelo * rigidbody.velocity.normalized;
        }
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        transform.rotation = Quaternion.identity;
        float angle = Mathf.Atan2(rigidbody.velocity.z, rigidbody.velocity.x);
        transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), angle);
	}
}
