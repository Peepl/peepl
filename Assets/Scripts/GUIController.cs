using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    private Text MoraleCounter;
    private Text PopulationCounter;

	private GameObject InfoPanel;
	private Text InfoPanelText;
	private float InfoPanelStartTime;

	private float InfoPanelShowTime = 5.0f;

    private long MoraleBlinkStartTime;
    private long PopulationBlinkStartTime;
	// Use this for initialization
	void Start () {
        PopulationCounter = GameObject.Find("PeopleCountText").GetComponent<Text>();
        MoraleCounter = GameObject.Find("MoraleCountText").GetComponent<Text>(); ;
		InfoPanel = GameObject.Find ("InfoPanel");
		InfoPanelText = GameObject.Find ("InfoText").GetComponent<Text>();

        MoraleBlinkStartTime = 0;
        PopulationBlinkStartTime = 0;
		InfoPanelStartTime = 0;

		InfoPanel.SetActive(false);

		EventTriggered("Your village has been destroyed!");
		}
	
    public void MoraleChanged()
    {
        MoraleBlinkStartTime = System.DateTime.Now.Ticks;
    }

    public void PopulationChanged()
    {
        PopulationBlinkStartTime = System.DateTime.Now.Ticks;
    }

	public void EventTriggered(string eventText) {
	
		InfoPanelStartTime = Time.time;

		InfoPanelText.text = eventText;
		InfoPanel.SetActive(true);
	}

	public void SandStormKill() {

		if (!InfoPanel.activeSelf) {
			EventTriggered("The storm rages.");
		}
	}

	// Update is called once per frame
	void Update () {
        
		UpdateCounters();

		UpdateInfoPanel();

    }

	void UpdateCounters() {
	
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

	void UpdateInfoPanel() {
	
		float deltaTime = Time.time - InfoPanelStartTime;

		if (deltaTime >= InfoPanelShowTime && InfoPanel.activeSelf) {
		
			InfoPanel.SetActive(false);

		}
	
	}
}
