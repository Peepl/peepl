using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmAI : MonoBehaviour {

    public GameObject CitizenPrefab;

    public int People = 25;
    public int Morale = 100;
    public GameColor TribeColor;

    private List<GameObject> m_People;
	// Use this for initialization
	void Start () {
    }

    public void InitSwarm(GameObject leader)
    {
        TribeColor = GameColor.White;

        Debug.Log("initing swarm");
        m_People = new List<GameObject>();
        for (int i = 0; i < People; ++i)
        {
            AddPerson(leader);
        }
	}

    public void AddPerson(GameObject leader)
    {
        GameObject tmp = Instantiate(CitizenPrefab, PersonAI.GetRandomOffset(), Quaternion.identity) as GameObject;
        tmp.GetComponent<PersonAI>().Leader = leader;
        m_People.Add(tmp);
    }

    public void StormDamage(float severity)
    {
        if ( Random.Range(0.0f, 1.0f) < severity && m_People.Count > 0 )
        //if (Random.Range(0.0f, 50.0f) < severity)
        {
            int randIndex = Random.Range(0, m_People.Count);
            GameObject p = m_People[randIndex];
            if ( !p.GetComponent<PersonAI>().IsSheltered)
            {
                Debug.Log("Storm kill!");
                m_People.Remove(p);
                Destroy(p);
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
                m_People.Remove(p);
                Destroy(p);
                i--;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        for (int i = 0; i < m_People.Count-1; ++i)
        {
            for (int j = i + 1; j < m_People.Count; ++j)
            {
                Vector3 p1 = m_People[i].transform.position;
                Vector3 p2 = m_People[j].transform.position;
                Vector3 v = p2 - p1;
                float mag = v.magnitude;
                float multiplier;
                if (mag < 8.0f)
                {
                    mag = Mathf.Max(1.0f, mag);
                    multiplier = 0.1f / (mag * mag);
                }
                else
                {
                    multiplier = -0.001f * mag;
                }
                m_People[i].GetComponent<Rigidbody>().AddForce(multiplier * -v);
                m_People[j].GetComponent<Rigidbody>().AddForce(multiplier * v);
            }
        }
	}
}
