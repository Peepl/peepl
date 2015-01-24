using UnityEngine;
using System.Collections;

public class OrbEvent : MonoBehaviour {

	bool active;

	Transform orbTransform;
	
	// Use this for initialization
	void Start () {
	
		orbTransform = transform.FindChild("EvilSphere");

		orbTransform.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("orb trigger");

		if (!active) return;
		
		if ( collider.gameObject.tag.Equals("Person"))
		{
			active = false;

			orbTransform.gameObject.SetActive(true);

			GameObject swarm = GameObject.Find("Swarm");
			swarm.GetComponent<SwarmAI>().KillInRadius(transform.position, 25.0f, 0.25f);
		}
	}
}
