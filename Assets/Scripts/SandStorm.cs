using UnityEngine;
using System.Collections;

public class SandStorm : MonoBehaviour {

	public AudioClip storm;
	public AudioSource stormAS;

    public float Severity;
	public float dayVal=1f;

    public long Length = 230000000; // 60 s
    private long Starttime;

	public long dayLength = 250000000;
	public long nightLength = 200000000;
	private bool day = true;
	private long dayStart;
	private long transition = 150000000;
    
	private long last =0;

	private bool ending = false;
	private bool victory;

	private bool forceChange = false;

	// Use this for initialization
	void Start () {
		ending = false;
		victory = false;
        Starttime = System.DateTime.Now.Ticks - 1 * Length;
		dayStart = System.DateTime.Now.Ticks;
		dayVal = 1f;
		last = dayStart;
	}
	public void VillageFound(){
		Starttime = System.DateTime.Now.Ticks;
		dayVal = 1;
		this.day = true;
		ending = true;
		victory = true;
	}
	public void EndOfWorldStorm(){
		ending = true;
	}

	public void ForceDay()
	{
		forceChange = true;
		day = true;
	//	dayVal = 1;
		dayStart = System.DateTime.Now.Ticks;
	}
	public void ForceNight()
	{
		forceChange = true;
        day = false;
	//	dayVal = 0;
		dayStart = System.DateTime.Now.Ticks;
	}
	
	// Update is called once per frame
	void Update () {

		if(ending)
		{
			stormAS.Play();
			Severity = Mathf.Max( Mathf.Min(Severity+0.2f*(victory?-1:1), 5.0f), -10f);

		}

		else
		{

		    if ( System.DateTime.Now.Ticks - Starttime > 4 * Length )
	        {
	            Debug.Log("starting storm");
	            Starttime = System.DateTime.Now.Ticks;
				stormAS.Play();
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

			if(forceChange)
			{
				if(day && dayVal < 1)
				{
					dayVal += 0.04f;
				}
				else if(!day && dayVal > 0)
				{
					dayVal -= 0.04f;
				}
				if(day && dayVal > 1)
				{
					dayVal = 1;
					forceChange = false;
                    dayStart = System.DateTime.Now.Ticks;
                }
                else if(!day && dayVal < 0)
                {
                    dayVal = 0;
                    forceChange = false;
                    dayStart = System.DateTime.Now.Ticks;
				}
			}
			else
			{

				//Day is over
		        if(System.DateTime.Now.Ticks > dayStart + dayLength && !day)
				{
					dayVal = (float)(System.DateTime.Now.Ticks - (dayStart + dayLength) )/(float)transition;
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
					if(dayVal <= 0.0f)
					{
						dayVal = 0.0f;
						day = false;
		                dayStart = System.DateTime.Now.Ticks;
		            }
		        }
				if(dayVal > 1f)
					dayVal = 1f;
				else if(dayVal <0f)
					dayVal = 0f;
				//Debug.Log ("angle " + angle+ " --- " + ( ( (float)System.DateTime.Now.Ticks))/((float) (dayLength+transition+nightLength+transition)));
		        
			}
			long tickd = System.DateTime.Now.Ticks % (dayLength+transition+nightLength+transition)*4;
			float angle = Mathf.Cos( (float)(( ( (double)tickd))/((double) (dayLength+transition+nightLength+transition)*4)*Mathf.PI*2));
            float angle2 = Mathf.Sin( (float)(( ( (double)tickd))/((double) (dayLength+transition+nightLength+transition)*4)*Mathf.PI*2));
            GameObject.Find("mainlight").GetComponent<Light>().intensity = dayVal*0.3f+0.2f;
			GameObject.Find("mainlight").GetComponent<Light>().shadowStrength = dayVal*0.8f+0.2f;
	        GameObject.Find("mainlight").transform.rotation = Quaternion.Euler(new Vector3(46f+angle*15f, 212f+angle*20f, 46f+(angle2)*15f));
		}
    }
}
