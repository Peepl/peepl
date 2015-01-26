using UnityEngine;
using System.Collections;

public class Cavern : MonoBehaviour {

    GameColor Color;
	public AudioClip goodClip;

    private bool Friendly;
	private AudioSource goodAs;

	private bool Active;
	private bool rotate;

	private Transform sphere;

	// Use this for initialization
	void Start () {
        Color = GameColor.Blue;
        GameObject swarm = GameObject.FindGameObjectWithTag("Swarm");
        bool colorFriendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);
        float goodChance = colorFriendly ? 0.75f : 0.25f;
		Friendly = true;// Random.Range(0.0f, 1.0f) < goodChance;
        Debug.Log("cavern is " + Friendly);

		goodAs = this.gameObject.AddComponent<AudioSource>();
		goodAs.clip = goodClip;
	//	rotate = true;
	//	sphere = this.transform.Find("good");
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Person") &&
            collider.gameObject.GetComponent<PersonAI>().GetDistanceToLeader() < 40.0f )
        {
            if ( Friendly )
            {
			  collider.gameObject.GetComponent<PersonAI>().IsSheltered = true;
				goodAs.Play();
				rotate = true;
            }
            else 
            {
                if (Random.Range(0.0f, 1.0f) < 0.05f )
                {
                    GameObject swarm = GameObject.Find("Swarm");
                    swarm.GetComponent<SwarmAI>().KillInRadius(transform.position, 50.0f, 0.1f);
                }
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
		//if(rotate)
		{
		//	this.sphere.Rotate(new Vector3(0, 0.3f, 0f));
		}
	}
}
