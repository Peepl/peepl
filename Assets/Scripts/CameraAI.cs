using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    public GameObject leader;
    public float Distance = 10.0f;

    // Use this for initialization
	void Start () {
        transform.position = new Vector3(0.0f, Distance, Distance);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(leader.transform.position.x, leader.transform.position.y + Distance, leader.transform.position.z - Distance);
        transform.LookAt(leader.transform.position); 
	
	}
}
