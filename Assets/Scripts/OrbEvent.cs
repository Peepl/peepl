using UnityEngine;
using System.Collections;

public class OrbEvent : MonoBehaviour {

	private AudioSource eventActive;
	public AudioClip eventActiveClip;

    bool active;
	bool Friendly;

	Transform orbTransform;
	
	// Use this for initialization
	void Start () {
	
		orbTransform = transform.FindChild("EvilSphere");

		orbTransform.gameObject.SetActive(false);

		active = true;
		Friendly = Random.Range(0.0f, 1.0f) < 0.25;

		eventActive = this.gameObject.AddComponent<AudioSource>();
		eventActive.clip = eventActiveClip;
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
			if(Friendly)
			{
				GameObject.Find("GameManager").GetComponent<SandStorm>().ForceDay();
			}
			else
			{
				GameObject.Find("GameManager").GetComponent<SandStorm>().ForceNight();
			}
			active = false;
			eventActive.Play();
			orbTransform.gameObject.SetActive(true);

			GameObject swarm = GameObject.Find("Swarm");
			swarm.GetComponent<SwarmAI>().KillInRadius(transform.position, 25.0f, 0.25f);
		}
	}
}
