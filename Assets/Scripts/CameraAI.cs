using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    public GameObject leader;
    public float Distance = 10.0f;
	public float Height = 15.0f;


    // Use this for initialization
	void Start () {
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.nearClipPlane = 15.0f;
        camera.farClipPlane = 140.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(leader.transform.position.x+Distance, leader.transform.position.y + Height, leader.transform.position.z - Distance);
        transform.LookAt(leader.transform.position);
        float stormSeverity = GameObject.Find("GameManager").GetComponent<SandStorm>().Severity;
		float day = GameObject.Find("GameManager").GetComponent<SandStorm>().dayVal;
        DesertShader storm = GetComponent<DesertShader>();
        if(stormSeverity < 0)
		{
			//victory case
			storm.perlinStrength = 0f;
			storm.speedX = 0f;
			storm.speedY = 0f;
			//storm.desertColor = new Color(39f/255f* stormSeverity,19f/255f* stormSeverity,0);
			storm.desertColor = new Color(208f/255f* stormSeverity*(day+0.2f),189f/255f* stormSeverity*(day+0.2f),89f/255f* stormSeverity*(day+0.2f));
			storm.fogStrength = -stormSeverity;
            storm.day = 1;
        }
        else
        {
            storm.perlinStrength = 1.0f + 2.5f * stormSeverity * stormSeverity;
	        storm.speedX = 1.4f * stormSeverity+0.24f;
	        storm.speedY = 1.3f * stormSeverity+0.24f;
			//storm.desertColor = new Color(39f/255f* stormSeverity,19f/255f* stormSeverity,0);
			storm.desertColor = new Color(208f/255f* stormSeverity*(day+0.2f),189f/255f* stormSeverity*(day+0.2f),89f/255f* stormSeverity*(day+0.2f));
	        storm.fogStrength = 0.8f + 3.0f * stormSeverity * stormSeverity;
			storm.day = day;
		}
    }
}
