using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    public GameObject leader;
    public float Distance = 10.0f;

    // Use this for initialization
	void Start () {
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.nearClipPlane = 15.0f;
        camera.farClipPlane = 120.0f;
        transform.position = new Vector3(0.0f, Distance, Distance);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(leader.transform.position.x, leader.transform.position.y + Distance, leader.transform.position.z - Distance);
        transform.LookAt(leader.transform.position);

        float stormSeverity = GameObject.Find("GameManager").GetComponent<SandStorm>().Severity;
        DesertShader storm = GetComponent<DesertShader>();
        storm.perlinStrength = 0.5f * stormSeverity;
        storm.speedX = 0.2f * stormSeverity;
        storm.speedY = 0.2f * stormSeverity;
        storm.fogStrength = stormSeverity;
	
	}
}
