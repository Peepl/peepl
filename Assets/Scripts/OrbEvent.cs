﻿using UnityEngine;
using System.Collections;

public class OrbEvent : MonoBehaviour {

	private AudioSource eventActive;
	public AudioClip eventActiveClip;

    bool active;
	bool Friendly;

	bool orbScale = false;

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
		
		if ( collider.gameObject.tag.Equals("Person"))
		{
			orbScale = true;
			if(Friendly)
			{
				orbTransform.Find("Sphere").gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f,0.8f,1.0f);
                
                GameObject.Find("GameManager").GetComponent<SandStorm>().ForceDay();
				orbTransform.Find("open").gameObject.SetActive(true);
			}
			else
			{
				GameObject.Find("GameManager").GetComponent<SandStorm>().ForceNight();
				orbTransform.Find("open").gameObject.SetActive(true);
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
