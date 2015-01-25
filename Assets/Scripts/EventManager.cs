using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    public enum GameColor { White, Blue, Black, Red, Green };
    public enum EventType { Temple, Obelisk, Orb, Cavern, Oasis };

    public GameColor TribeColor;

	// Use this for initialization
	void Start () {
        TribeColor = GameColor.White;
	}

	

	// Update is called once per frame
	void Update () {
	
	}
}
