using UnityEngine;
using System.Collections;

public class SwarmAI : MonoBehaviour {

    public GameObject CitizenPrefab;
    public GameObject Leader;

    private GameObject[] m_People;
	// Use this for initialization
	void Start () {
        m_People = new GameObject[100];
        int i = 0;
        for (int x = -5; x < 5; ++x)
        {
            for (int z = -5; z < 5; ++z)
            {
                GameObject tmp = Instantiate(CitizenPrefab, new Vector3(x, 5.0f, z), Quaternion.identity) as GameObject;
                tmp.GetComponent<PersonAI>().Leader = Leader;
                m_People[i] = tmp;
                ++i;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < m_People.Length; ++i)
        {
            for (int j = i + 1; j < m_People.Length; ++j)
            {
                Vector3 p1 = m_People[i].transform.position;
                Vector3 p2 = m_People[j].transform.position;
                float magnitude = 0.03f / (p2 - p1).magnitude;
                m_People[i].GetComponent<Rigidbody>().AddForce(magnitude * (p1 - p2) );
                m_People[j].GetComponent<Rigidbody>().AddForce(magnitude * (p2 - p1) );
            }
        }
	}
}
