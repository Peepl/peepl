using UnityEngine;
using System.Collections;

public class Cavern : MonoBehaviour {

    GameColor Color;

    private bool Friendly;

	// Use this for initialization
	void Start () {
        Color = GameColor.Blue;
        GameObject swarm = GameObject.FindGameObjectWithTag("Swarm");
        bool colorFriendly = EventManager.IsFriendly(swarm.GetComponent<SwarmAI>().TribeColor, Color);
        float goodChance = colorFriendly ? 0.75f : 0.25f;
        Friendly = Random.Range(0.0f, 1.0f) < goodChance;
        Debug.Log("cavern is " + Friendly);
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Person"))
        {

            if ( Friendly )
            {
                collider.gameObject.GetComponent<PersonAI>().IsSheltered = true;
            }
            else 
            {
                if (Random.Range(0.0f, 1.0f) < 0.05f )
                {
                    GameObject swarm = GameObject.Find("Swarm");
                    swarm.GetComponent<SwarmAI>().KillInRadius(transform.position, transform.localScale.x, 0.1f);
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

	}
}
