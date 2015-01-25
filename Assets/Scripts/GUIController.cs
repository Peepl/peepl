using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    private Text MoraleCounter;
    private Text PopulationCounter;

    private long MoraleBlinkStartTime;
    private long PopulationBlinkStartTime;
	// Use this for initialization
	void Start () {
        PopulationCounter = GameObject.Find("PeopleCountText").GetComponent<Text>();
        MoraleCounter = GameObject.Find("MoraleCountText").GetComponent<Text>(); ;

        MoraleBlinkStartTime = 0;
        PopulationBlinkStartTime = 0;
		}
	
    public void MoraleChanged()
    {
        MoraleBlinkStartTime = System.DateTime.Now.Ticks;
    }

    public void PopulationChanged()
    {
        PopulationBlinkStartTime = System.DateTime.Now.Ticks;
    }

	// Update is called once per frame
	void Update () {
        SwarmAI swarm = GameObject.Find("Swarm").GetComponent<SwarmAI>();
        PopulationCounter.text = swarm.GetPopulation().ToString();
        MoraleCounter.text = Mathf.CeilToInt(swarm.Morale).ToString();

        long tmp = System.DateTime.Now.Ticks - MoraleBlinkStartTime ; 
        if ( tmp < 10000000)
        {
            MoraleCounter.fontSize = 14 + Mathf.RoundToInt( 8.0f * Mathf.Sin(2.0f * 3.141592f * tmp / (float)10000000) );
        }
        else
        {
            MoraleCounter.fontSize = 14;
        }
        tmp = System.DateTime.Now.Ticks - PopulationBlinkStartTime;
        if (tmp < 10000000)
        {
            PopulationCounter.fontSize = 14 + Mathf.RoundToInt(8.0f * Mathf.Sin(2.0f * 3.141592f * tmp / (float)10000000));
        }
        else
        {
            PopulationCounter.fontSize = 14;
        }
    }
}
