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
	void Awake () {
		World = Instantiate(WorldPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
		WorldGenerator gen = World.GetComponent<WorldGenerator>();
		gen.Generate();

		Debug.Log ("Get Village pos");

		TheLeader = Instantiate(LeaderPrefab, new Vector3(50.0f, 5.0f, -20.0f) + gen.GetVillagePosition(), Quaternion.identity) as GameObject;
        Swarm = Instantiate(SwarmPrefab, new Vector3(0.0f, 0.0f, 0.0f) + gen.GetVillagePosition(), Quaternion.identity) as GameObject;
        Swarm.name = "Swarm";
        Swarm.GetComponent<SwarmAI>().InitSwarm(TheLeader);
		World.GetComponent<WorldGenerator>().SetLeader(TheLeader);
		GameObject.Find("Main Camera").GetComponent<CameraAI>().leader = TheLeader;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
