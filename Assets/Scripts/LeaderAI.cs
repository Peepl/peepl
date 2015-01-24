using UnityEngine;
using System.Collections;

public class LeaderAI : MonoBehaviour {

    public Vector3 target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Mouse Down Hit Terrain " + hit.point);
                target = hit.point;
                target.y = 0.0f;
            }
        }

        rigidbody.velocity = target - transform.position;
        if ( rigidbody.velocity.magnitude > 10.0f)
        {
            rigidbody.velocity = 10.0f * rigidbody.velocity.normalized;
        }
	
	}
}
