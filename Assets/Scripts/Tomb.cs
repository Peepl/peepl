﻿using UnityEngine;
using System.Collections;

public class Tomb : MonoBehaviour {
	
	GameColor Color;
	public AudioClip goodClip;
	public AudioClip badClip;
	
	private AudioSource goodAs;
	private AudioSource badAs;
    
	private bool Friendly;
	private bool Active;

	private GameObject good;

	private GUIController guiController;

	// Use this for initialization
	void Start () {
		Color = GameColor.Blue;
		GameObject swarm = GameObject.Find("Swarm");
		bool colorFriendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);
		float goodChance = colorFriendly ? 0.75f : 0.25f;
		Friendly = Random.Range(0.0f, 1.0f) < goodChance;
		Active = true;
		badAs = this.gameObject.AddComponent<AudioSource>();
		goodAs = this.gameObject.AddComponent<AudioSource>();
		badAs.clip = badClip;
		goodAs.clip = goodClip;

		guiController = GameObject.FindObjectOfType<GUIController>();
	}
	
	void OnTriggerEnter(Collider collider)
	{
		
		if ( collider.gameObject.tag.Equals("Person") &&
            collider.gameObject.GetComponent<PersonAI>().GetDistanceToLeader() < 40.0f )
		{
            if (Friendly) collider.gameObject.GetComponent<PersonAI>().IsSheltered = true;
            if (!Active) return;

            Active = false;
			if ( Friendly )
			{
				// show friendly effect here
				GameObject swarm = GameObject.Find("Swarm");
				SwarmAI swarmai = swarm.GetComponent<SwarmAI>();
				int bonuspeople = Random.Range(2, 6);
				for (int i = 0; i < bonuspeople; ++i)
				{
					swarmai.AddPerson(swarmai.Leader);
				}
				swarm.GetComponent<SwarmAI>().Morale += 30;
                GameObject.Find("GameManager").GetComponent<GUIController>().MoraleChanged();

				goodAs.Play();
				Debug.Log("Friendly tomb triggered - set good visible");
				transform.Find("good").gameObject.SetActive(true);

				guiController.EventTriggered("The Holy tomb brings joy for the tribe .");
			}
			else 
			{
				transform.Find("bad").gameObject.SetActive(true);
				// show unfriendly effect here (change model to skeletons)
				GameObject swarm = GameObject.Find("Swarm");
				swarm.GetComponent<SwarmAI>().Morale -= 20;
                GameObject.Find("GameManager").GetComponent<GUIController>().MoraleChanged();
				badAs.Play();
				Debug.Log("Unfriendly tomb triggered - no animation");

				guiController.EventTriggered("The gods have abandoned this place.");
                
			}
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		
		if (collider.gameObject.tag.Equals("Person"))
		{
			collider.gameObject.GetComponent<PersonAI>().IsSheltered = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
