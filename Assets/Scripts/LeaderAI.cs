﻿using UnityEngine;
using System.Collections;

public class LeaderAI : MonoBehaviour {

    public Vector3 target;
    private float cachedAngle = 0.0f;
    public float MaxSpeed = 8.0f;

	private bool fireDown = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
			fireDown = true;
		}
		if(Input.GetButtonUp("Fire1"))
			fireDown = false;
		if(fireDown)
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Mouse Down Hit Terrain " + hit.point);
                target = hit.point;
                //target.y = 0.0f;
            }
        }

        rigidbody.velocity = target - transform.position;
        if ( rigidbody.velocity.magnitude > MaxSpeed)
        {
            rigidbody.velocity = MaxSpeed * rigidbody.velocity.normalized;
        }

        transform.rotation = Quaternion.identity;

        if (rigidbody.velocity.magnitude > 1.0f)
        {
            cachedAngle = Mathf.Atan2(rigidbody.velocity.x, rigidbody.velocity.z);
        }
        transform.localEulerAngles = new Vector3(0.0f, 180.0f * cachedAngle / 3.14159f, 0.0f);
	}
}
