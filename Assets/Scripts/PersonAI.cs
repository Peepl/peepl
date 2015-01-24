using UnityEngine;
using System.Collections;

public class PersonAI : MonoBehaviour {

    public GameObject Leader;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        //rigidbody.velocity = Leader.transform.position - transform.position;
        rigidbody.AddForce(0.3f * (Leader.transform.position - transform.position) );
	}
}
