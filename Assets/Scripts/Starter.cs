using UnityEngine;
using System.Collections;

public class Starter : MonoBehaviour {

    public GameObject LeaderPrefab;
    public GameObject SwarmPrefab;
    public GameObject TheLeader = null;
	public GameObject WorldPrefab;

    private GameObject Swarm;
	private GameObject World;

	// Use this for initialization
	void Start () {
        TheLeader = Instantiate(LeaderPrefab, new Vector3(10.0f, 5.0f, 0.0f), Quaternion.identity) as GameObject;
        Swarm = Instantiate(SwarmPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        Swarm.name = "Swarm";
        Swarm.GetComponent<SwarmAI>().InitSwarm(TheLeader);
        GameObject.Find("Main Camera").GetComponent<CameraAI>().leader = TheLeader;
		World = Instantiate(WorldPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
		World.GetComponent<WorldGenerator>().Init(TheLeader);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
