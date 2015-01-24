using UnityEngine;
using System.Collections;

public class Tomb : MonoBehaviour {
	
	GameColor Color;
	
	private bool Friendly;
	private bool Active;

	private GameObject good;

	// Use this for initialization
	void Start () {
		Color = GameColor.Blue;
		GameObject swarm = GameObject.Find("Swarm");
		bool colorFriendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);
		float goodChance = colorFriendly ? 0.75f : 0.25f;
		Friendly = Random.Range(0.0f, 1.0f) < goodChance;
		Active = true;
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("temple trigger");
		if (!Active) return;
		
		if (collider.gameObject.tag.Equals("Person"))
		{
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
				
				Debug.Log("Friendly tomb triggered - set good visible");
				transform.Find("good").gameObject.SetActive(true);
			}
			else 
			{
				// show unfriendly effect here (change model to skeletons)
				GameObject swarm = GameObject.Find("Swarm");
				swarm.GetComponent<SwarmAI>().Morale -= 20;
				
				Debug.Log("Unfriendly tomb triggered - no animation");
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
