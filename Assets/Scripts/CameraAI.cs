﻿using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    public GameObject leader;
    public float Distance = 10.0f;
	public float Height = 15.0f;

	private float dayNightCycle = 0f;

    // Use this for initialization
	void Start () {
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.nearClipPlane = 15.0f;
        camera.farClipPlane = 140.0f;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(leader.transform.position.x-Distance, leader.transform.position.y + Height, leader.transform.position.z - Distance);
        transform.LookAt(leader.transform.position);

        float stormSeverity = GameObject.Find("GameManager").GetComponent<SandStorm>().Severity;
        DesertShader storm = GetComponent<DesertShader>();
        storm.perlinStrength = 0.5f * stormSeverity+1.8f;
        storm.speedX = 0.2f * stormSeverity+0.1f;
        storm.speedY = 0.2f * stormSeverity+0.1f;
		storm.desertColor = new Color(39f/255f* stormSeverity,19f/255f* stormSeverity,0);
        storm.fogStrength = Mathf.Min (1.0f,stormSeverity+0.4f);
		storm.day = Mathf.Abs (Mathf.Sin(dayNightCycle));
		dayNightCycle+=0.005f;
	}
}
