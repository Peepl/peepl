using UnityEngine;
using System.Collections;

public class SandStorm : MonoBehaviour {

    public float Severity;

    public long Length = 600000000; // 60 s
    private long Starttime;

	// Use this for initialization
	void Start () {
        Starttime = System.DateTime.Now.Ticks - 5 * Length;
	}
	
	// Update is called once per frame
	void Update () {
	    if ( System.DateTime.Now.Ticks - Starttime > 7 * Length )
        {
            Debug.Log("starting storm");
            Starttime = System.DateTime.Now.Ticks;
        }

        else if ( System.DateTime.Now.Ticks - Starttime < Length )
        {
            float phase = ( System.DateTime.Now.Ticks - Starttime ) / (float)Length;
            Severity = Mathf.Sin(phase * 3.14159f);
            GameObject.Find("Swarm").GetComponent<SwarmAI>().StormDamage(Severity);
        }
        else
        {
            Severity = 0.0f;
        }

	}
}
