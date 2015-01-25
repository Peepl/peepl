using UnityEngine;
using System.Collections;

public class EndTile : MonoBehaviour {

	public AudioClip goodClip;

	private AudioSource goodAs;

	public bool Active;

	private GUIController guiController;

	// Use this for initialization
	void Start () {
		Active = true;
		
		goodAs = this.gameObject.AddComponent<AudioSource>();
		goodAs.clip = goodClip;
		guiController = GameObject.FindObjectOfType<GUIController>();
        

	}

	void OnTriggerEnter(Collider collider)
	{
		if (!Active) return;
		
		if (collider.gameObject.tag.Equals("Person") &&
                        collider.gameObject.GetComponent<PersonAI>().GetDistanceToLeader() < 40.0f )
		{
			Active = false;
			//TODO - victory
			Debug.Log("VICTORY");
			goodAs.Play();
			Invoke("VictoryPrePre", 2.5f);
            Invoke("VictoryPre", 3.5f);
//			GameObject.Find("GameManager").GetComponent<SandStorm>().VillageFound();
			Invoke("Victory", 6.5f);
		}
	}
	void VictoryPrePre()
	{
		guiController.EventTriggered("What do we do now?");
		
    }
    void VictoryPre()
	{
        
		GameObject.Find("GameManager").GetComponent<SandStorm>().VillageFound();
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
