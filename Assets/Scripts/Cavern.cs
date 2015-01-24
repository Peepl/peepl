using UnityEngine;
using System.Collections;

public class Cavern : MonoBehaviour {

    GameColor Color;

    private bool Friendly;

	// Use this for initialization
	void Start () {
        Color = GameColor.Blue;

	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Person"))
        {
            GameObject swarm = GameObject.Find("Swarm");
            bool Friendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);

            if ( Friendly )
            {
                collider.gameObject.GetComponent<PersonAI>().IsSheltered = true;
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
