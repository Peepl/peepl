using UnityEngine;
using System.Collections;

public class SwarmAI : MonoBehaviour {

    public GameObject CitizenPrefab;

    public int People = 75;

    private GameObject[] m_People;
	// Use this for initialization
	void Start () {
    }

    public void InitSwarm(GameObject leader)
    {
        Debug.Log("initing swarm");
        m_People = new GameObject[200];
        for (int i = 0; i < People; ++i)
        {
            GameObject tmp = Instantiate(CitizenPrefab, PersonAI.GetRandomOffset() + new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity) as GameObject;
            tmp.GetComponent<PersonAI>().Leader = leader;
            m_People[i] = tmp;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < People; ++i)
        {
            for (int j = i + 1; j < People; ++j)
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
