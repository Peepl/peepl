using UnityEngine;
using System.Collections;

public enum EventType { Temple, Obelisk, Orb, Cavern, Oasis };
//public enum GameColor { White = 0, Blue = 1, Black = 2, Red = 3, Green = 4 };
public enum GameColor { Blue, Red };

public struct Event
{
    EventType type;
    GameColor color;
}

public class EventManager : MonoBehaviour {

    public static bool IsFriendly(GameColor c1, GameColor c2)
    {
        //int diff = Mathf.Abs(c1 - c2) % 4;
        //return diff <= 1;
        return c1 == c2;
    }

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	
	}
}
