using UnityEngine;
using System.Collections;

public class Obelisk : MonoBehaviour {

    GameColor Color;
	public AudioClip goodClip;
	public AudioClip badClip;
	
	private AudioSource goodAs;
	private AudioSource badAs;

    private bool Friendly;
    private bool Active;
	private int rotTarget = 0;
	private bool rotate = false;
	private int currot = 0;

	private GUIController guiController;

	// Use this for initialization
	void Start () {
        Color = GameColor.Blue;
        GameObject swarm = GameObject.Find("Swarm");
        bool colorFriendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);
        float goodChance = colorFriendly ? 0.75f : 0.25f;
        Friendly = Random.Range(0.0f, 1.0f) < goodChance;
        Active = true;

		guiController = GameObject.FindObjectOfType<GUIController>();

		badAs = this.gameObject.AddComponent<AudioSource>();
		goodAs = this.gameObject.AddComponent<AudioSource>();
		badAs.clip = badClip;
		goodAs.clip = goodClip;
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Person") &&
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
                swarm.GetComponent<SwarmAI>().Bless();
			//	transform.Find("obelisk").Find("obeliski").Find("bad").gameObject.SetActive(true);
				rotTarget += 760;
				rotate = true;
                collider.gameObject.GetComponent<PersonAI>().IsSheltered = true;

                
				goodAs.Play();
                Debug.Log("Friendly obelisk triggered");

				guiController.EventTriggered("The obelisk empowers you.");
            }
            else 
            {
                // show unfriendly effect here (death ray)
                GameObject swarm = GameObject.Find("Swarm");
                swarm.GetComponent<SwarmAI>().KillInRadius(transform.position, 50.0f, 0.5f);

				rotTarget += 760;
				rotate = true;

				if(swarm.GetComponent<SwarmAI>().TribeColor == GameColor.Blue)
					transform.Find("obelisk").Find("obeliski").Find("blue").gameObject.SetActive(true);
				else
					transform.Find("obelisk").Find("obeliski").Find("red").gameObject.SetActive(true);
				badAs.Play();
                Debug.Log("Unfriendly obelisk triggered");

				guiController.EventTriggered("The obelisk humms ominously.");
                
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
		if(rotate)
		{
			currot += 5;
			var t = this.transform.Find("obelisk").Find("obeliski");
			t.Rotate(new Vector3(0,5,0));
			if(currot > rotTarget)
			{
				transform.Find("obelisk").Find("obeliski").Find("blue").gameObject.SetActive(false);
				transform.Find("obelisk").Find("obeliski").Find("red").gameObject.SetActive(false);
				t.rotation = Quaternion.Euler(new Vector3(0,rotTarget,0));
			}
		}
	}
}
