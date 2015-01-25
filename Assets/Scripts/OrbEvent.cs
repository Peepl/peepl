using UnityEngine;
using System.Collections;

public class OrbEvent : MonoBehaviour {

	private AudioSource eventActive;
	public AudioClip eventActiveClip;

    bool active;
	bool Friendly;

	bool orbScale = false;

	Transform orbTransformGood;
	Transform orbTransformBad;
	Transform orbTransform;
    
	GUIController guiController;
	
	// Use this for initialization
	void Start () {
	
		orbTransformGood = transform.FindChild("good");
		orbTransformBad = transform.FindChild("bad");
        
		orbTransformGood.gameObject.SetActive(false);
		orbTransformBad.gameObject.SetActive(false);
        
		active = true;
		Friendly = Random.Range(0.0f, 1.0f) < 0.25;

		guiController = GameObject.FindObjectOfType<GUIController>();

		eventActive = this.gameObject.AddComponent<AudioSource>();
		eventActive.clip = eventActiveClip;
	}
	
	// Update is called once per frame
	void Update () {
		if(orbScale)
		{
			if(orbTransform.localScale.x > 1f)
			{
				orbScale = false;
				orbTransform.localScale = new Vector3(1f,1f,1f);
			}
			orbTransform.localScale = new Vector3(orbTransform.localScale.x+0.04f,orbTransform.localScale.x+0.04f,orbTransform.localScale.x+0.04f);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("orb trigger");

		if (!active) return;
		
		if ( collider.gameObject.tag.Equals("Person") &&
            collider.gameObject.GetComponent<PersonAI>().GetDistanceToLeader() < 40.0f)

		{
			orbScale = true;

			if(Friendly)
			{
				//orbTransform.Find("Sphere").gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f,0.8f,1.0f);
                
				orbTransform = orbTransformGood;

                GameObject.Find("GameManager").GetComponent<SandStorm>().ForceDay();
				orbTransform.Find("open").gameObject.SetActive(true);

				guiController.EventTriggered("Let there be light.");                
			}
			else
			{
				orbTransform = orbTransformBad;
                
                GameObject.Find("GameManager").GetComponent<SandStorm>().ForceNight();
				orbTransform.Find("open").gameObject.SetActive(true);
							
				guiController.EventTriggered("Darkness envelopes the tribe.");                
            }

			active = false;
			eventActive.Play();
			orbTransform.localScale = new Vector3(0,0,0);
			orbTransform.gameObject.SetActive(true);

			GameObject swarm = GameObject.Find("Swarm");
			swarm.GetComponent<SwarmAI>().KillInRadius(transform.position, 25.0f, 0.25f);
		}
	}
}
