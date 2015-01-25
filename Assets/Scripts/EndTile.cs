using UnityEngine;
using System.Collections;

public class EndTile : MonoBehaviour {

	public AudioClip goodClip;

	private AudioSource goodAs;

	public bool Active;

	// Use this for initialization
	void Start () {
		Active = true;
		
		goodAs = this.gameObject.AddComponent<AudioSource>();
		goodAs.clip = goodClip;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (!Active) return;
		
		if (collider.gameObject.tag.Equals("Person"))
		{
			Active = false;
			//TODO - victory
			Debug.Log("VICTORY");
			GameObject.Find("GameManager").GetComponent<SandStorm>().VillageFound();
			Invoke("Victory", 4.5f);
		}
	}

	void Victory()
	{
		Application.LoadLevel("Victory");
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
