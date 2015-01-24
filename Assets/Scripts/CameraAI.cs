using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    public GameObject leader;
    public float Distance = 10.0f;


    // Use this for initialization
	void Start () {
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.nearClipPlane = 15.0f;
        camera.farClipPlane = 140.0f;
        transform.position = new Vector3(0.0f, Distance, Distance);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(leader.transform.position.x-Distance, leader.transform.position.y + Distance, leader.transform.position.z - Distance);
        transform.LookAt(leader.transform.position);

        float stormSeverity = GameObject.Find("GameManager").GetComponent<SandStorm>().Severity;
        DesertShader storm = GetComponent<DesertShader>();
        storm.perlinStrength = 0.5f * stormSeverity+0.3f;
        storm.speedX = 0.2f * stormSeverity+0.02f;
        storm.speedY = 0.2f * stormSeverity+0.02f;
		storm.desertColor = new Color(39,19,0) * stormSeverity;
        storm.fogStrength = Mathf.Min (1.0f,stormSeverity+0.4f);
	
	}
}
