using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmAI : MonoBehaviour {

    public GameObject CitizenPrefab;
	public GameObject BloodPrefab;

    public int People = 25;
    public float Morale = 100;
    public GameColor TribeColor;

    private List<GameObject> m_People;
    public float m_MoraleLossSpeed = 0.004f;
    public GameObject Leader;

	private GUIController guiController;

	private bool ended = false;

	// Use this for initialization
	void Start () {
		ended = false;
		guiController = GameObject.FindObjectOfType<GUIController>();
    }

    public int GetPopulation()
    {
        return m_People.Count * 10;
    }

    public void InitSwarm(GameObject leader)
    {
        TribeColor = GameColor.Blue;
        Leader = leader;

        Debug.Log("initing swarm");
        m_People = new List<GameObject>();
        for (int i = 0; i < People; ++i)
        {
            AddPerson(leader);
        }
	}

    public void Bless()
    {
        m_MoraleLossSpeed *= 0.75f;
    }

    public void AddPerson(GameObject leader)
    {
        Vector3 offset = PersonAI.GetRandomOffset();
        offset.y = 10.0f;
        GameObject tmp = Instantiate(CitizenPrefab, offset, Quaternion.identity) as GameObject;
        tmp.transform.parent = this.transform;
        tmp.GetComponent<PersonAI>().Leader = leader;
        m_People.Add(tmp);
    }

    public void KillPerson(GameObject p )
    {
        GameObject blood = Instantiate(BloodPrefab, p.transform.position, p.transform.rotation) as GameObject;
        m_People.Remove(p);
        Destroy(p);
        GameObject.Find("GameManager").GetComponent<GUIController>().PopulationChanged();
    }

    public void StormDamage(float severity)
    {
        //if ( Random.Range(0.0f, 1.0f) < severity && m_People.Count > 0 )
        if (severity > 0.3f && Random.Range(0.0f, 40.0f) < severity * severity)
        {
            int randIndex = Random.Range(0, m_People.Count);
            GameObject p = m_People[randIndex];
            if ( !p.GetComponent<PersonAI>().IsSheltered)
            {
                Debug.Log("Storm kill!");

				guiController.SandStormKill();

                KillPerson(p);
            }
        }
    }

    public void KillInRadius(Vector3 center, float radius, float chance)
    {
        for (int i = 0; i < m_People.Count; ++i)
        {
            GameObject p = m_People[i];
            if ((p.transform.position - center).magnitude <= radius && Random.Range(0.0f, 1.0f) < chance)
            {
				//Add blood
                KillPerson(p);
                i--;
            }
        }
    }
	void gameover(){
		Application.LoadLevel("gameover");
	}

	void FixedUpdate () {
        Morale -= m_MoraleLossSpeed;
        if (Morale < 0.0f || GetPopulation() == 0)
		{
			Morale = 0.0f;
			if(!ended)
			{
				GameObject.Find("GameManager").GetComponent<SandStorm>().EndOfWorldStorm();
				Invoke("gameover", 1.5f);
			}
			ended = true;
		}

        for (int i = 0; i < m_People.Count; ++i)
        {
            if ( (m_People[i].transform.position - Leader.transform.position).magnitude > 100.0f )
            {
                KillPerson(m_People[i]);
                --i;
            }
        }

        for (int i = 0; i < m_People.Count-1; ++i)
        {
            for (int j = i + 1; j < m_People.Count; ++j)
            {
                Vector3 p1 = m_People[i].transform.position;
                Vector3 p2 = m_People[j].transform.position;
                Vector3 v = p2 - p1;
                v.y = 0.0f;
                float mag = v.magnitude;
                float multiplier;
                if (mag < 15.0f)
                {
                    mag = Mathf.Max(1.0f, mag);
                    multiplier = 30.0f / (mag * mag);
                }
                else if (mag < 70.0f)
                {
                    multiplier = -0.0005f * mag;
                }
                else
                {
                    multiplier = 0.0f;
                }
                m_People[i].GetComponent<Rigidbody>().AddForce(multiplier * -v);
                m_People[j].GetComponent<Rigidbody>().AddForce(multiplier * v);
            }
        }
	}
}
