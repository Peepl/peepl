using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    private Text MoraleCounter;
    private Text PopulationCounter;
	// Use this for initialization
	void Start () {
        PopulationCounter = GameObject.Find("PeopleCountText").GetComponent<Text>();
        MoraleCounter = GameObject.Find("MoraleCountText").GetComponent<Text>(); ;
		}
	
	// Update is called once per frame
	void Update () {
        SwarmAI swarm = GameObject.Find("Swarm").GetComponent<SwarmAI>();
        PopulationCounter.text = swarm.GetPopulation().ToString();
        MoraleCounter.text = Mathf.CeilToInt(swarm.Morale).ToString();
	}
}
