using UnityEngine;
using System.Collections;

public class Oasis : MonoBehaviour {

    GameColor Color;

	public AudioClip goodClip;
	public AudioClip badClip;

	private AudioSource goodAs;
	private AudioSource badAs;

    private bool Friendly;
    private bool Active;

	private GUIController guiController;

	// Use this for initialization
	void Start () {
        //Color = GameColor.Blue;
        //GameObject swarm = GameObject.Find("Swarm");
        //bool colorFriendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);
        //float goodChance = colorFriendly ? 0.75f : 0.25f;
        //Friendly = Random.Range(0.0f, 1.0f) < goodChance;
        Friendly = Random.Range(0.0f, 1.0f) < 0.25;
        Active = true;

		guiController = GameObject.FindObjectOfType<GUIController>();

		badAs = this.gameObject.AddComponent<AudioSource>();
		goodAs = this.gameObject.AddComponent<AudioSource>();
		badAs.clip = badClip;
		goodAs.clip = goodClip;
	}

    void OnTriggerEnter(Collider collider)
    {
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
                    swarmai.Morale = Mathf.Max(swarmai.Morale, 100.0f);
                    GameObject.Find("GameManager").GetComponent<GUIController>().MoraleChanged();
                }
				transform.Find("good").gameObject.SetActive(true);
                Debug.Log("Friendly oasis triggered");

				guiController.EventTriggered("Oasis found! Morale boosted!");

				goodAs.Play();
            }
            else 
            {
                // show unfriendly effect here (change model to skeletons)
                GameObject swarm = GameObject.Find("Swarm");
                swarm.GetComponent<SwarmAI>().Morale -= 20;
                GameObject.Find("GameManager").GetComponent<GUIController>().MoraleChanged();
                transform.Find("bad").gameObject.SetActive(true);

				guiController.EventTriggered("Mirage found! Morale degraded!");

				Debug.Log("Unfriendly oasis triggered");
				badAs.Play();
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
