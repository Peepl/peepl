using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    public Transform leader;

    // Use this for initialization
	void Start () {
        transform.position = new Vector3(-40.0f, 40.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(leader.position.x - 40.0f, leader.position.y + 40.0f, leader.position.z);
        transform.LookAt(leader.transform.position); 
	
	}
}
