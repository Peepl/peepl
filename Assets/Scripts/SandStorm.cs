using UnityEngine;
using System.Collections;

public class SandStorm : MonoBehaviour {

    public float Severity;
	public float dayVal=1f;

    public long Length = 200000000; // 60 s
    private long Starttime;

	public long dayLength = 100000000;
	public long nightLength = 50000000;
	private bool day = true;
	private long dayStart;
	private long transition = 50000000;
    
	// Use this for initialization
	void Start () {
        Starttime = System.DateTime.Now.Ticks - 5 * Length;
		dayStart = System.DateTime.Now.Ticks;
		dayVal = 1f;
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

		//Day is over
        if(System.DateTime.Now.Ticks > dayStart + dayLength && !day)
		{
			dayVal = (float)(System.DateTime.Now.Ticks - (dayStart + dayLength) )/(float)transition;
			Debug.Log  (dayVal + " -- " + (System.DateTime.Now.Ticks - (dayStart + dayLength)));
            if(dayVal >= 1.0f)
			{
				dayVal = 1.0f;
				day = true;
				dayStart = System.DateTime.Now.Ticks;
			}
		}
		//Day is over
		else if(System.DateTime.Now.Ticks > dayStart + nightLength && day)
		{
			dayVal = 1.0f-(float)(System.DateTime.Now.Ticks - (dayStart + dayLength) )/(float)transition;
			Debug.Log  (dayVal + " -- " + (System.DateTime.Now.Ticks - (dayStart + dayLength)));
			if(dayVal <= 0.0f)
			{
				dayVal = 0.0f;
				day = false;
                dayStart = System.DateTime.Now.Ticks;
            }
        }
        
    }
}
