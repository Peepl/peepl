using UnityEngine;
using System.Collections;

public class LeaderAI : MonoBehaviour {

    public Vector3 target;
    private float cachedAngle = 0.0f;
    public float MaxSpeed = 8.0f;

	private bool fireDown = false;

	private Transform sphere;

	// Use this for initialization
	void Start () {
		this.sphere = this.transform.Find ("Sphere");
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
		this.sphere.Rotate(new Vector3(0f,7f,0f));
        rigidbody.velocity = target - transform.position;
        if ( rigidbody.velocity.magnitude > MaxSpeed)
        {
            rigidbody.velocity = MaxSpeed * rigidbody.velocity.normalized;
        }
		var moralspeed =GameObject.Find("Swarm").GetComponent<SwarmAI>().blessCount;
		Debug.Log(moralspeed);
		this.sphere.localScale = new Vector3(
			moralspeed*0.2f+0.3f,
			moralspeed*0.2f+0.3f,
			moralspeed*0.2f+0.3f
			);
        transform.rotation = Quaternion.identity;

        if (rigidbody.velocity.magnitude > 1.0f)
        {
            cachedAngle = Mathf.Atan2(rigidbody.velocity.x, rigidbody.velocity.z);
        }
        transform.localEulerAngles = new Vector3(0.0f, 180.0f * cachedAngle / 3.14159f, 0.0f);
	}
}
