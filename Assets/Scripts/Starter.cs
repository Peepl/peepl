using UnityEngine;
using System.Collections;

public class Starter : MonoBehaviour {

    public GameObject LeaderPrefab;
    public GameObject TheLeader = null;
	// Use this for initialization
	void Start () {
        TheLeader = Instantiate(LeaderPrefab, new Vector3(10.0f, 5.0f, 0.0f), Quaternion.identity) as GameObject;
        FindObjectOfType<SwarmAI>().Leader = TheLeader;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
