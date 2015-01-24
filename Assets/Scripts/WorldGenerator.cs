using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//[ExecuteInEditMode]
public class WorldGenerator : MonoBehaviour {

	private class Tile {
		public GameObject gameObject;
		public int frameIndex;
		public int key;
	}

	public int generationRange;

	public float tileSize;
	
	public List<GameObject> prefabs;

	public int seed = 1234;
	
	private List<Tile> currentTiles = new List<Tile>();
	private List<Tile> newTiles = new List<Tile>();

	private int frameIndex = 0;

	private int maxSize = 1024;

	private int[] mapData;

	private Transform leaderTransform;
	
	public void Init(GameObject leaderGameObject) {
		leaderTransform = leaderGameObject.transform;			
	}

	// Use this for initialization
	void Start () {

		GenerateMapData();
	}
	
	// Update is called once per frame
	void Update () {

		GenerateTiles();
	}

	void Reset() {

		generationRange = 5;
		tileSize = 50.0f;
	}

	public void Test() {

		GenerateTiles();
	}

	static Tile FindTile(List<Tile> tiles, int key) {

		for (int i = 0; i < tiles.Count; i++) {
			if (tiles[i].key == key) {
				return tiles[i];
			}
		}

		return null;
	}

	void GenerateMapData() {
	
		mapData = new int[maxSize * maxSize];

		Random.seed = seed;

		for (int i = 0; i < maxSize * maxSize; i++) {
			mapData[i] = Random.Range(0, prefabs.Count);
		}
	
	}

	void GenerateTiles() {

		if (leaderTransform == null) {
			Debug.LogError("No leaderTransform set");
			return;
		}

		if (prefabs == null || prefabs.Count == 0) {
			Debug.LogError("No prefabs");
		}
        
		if (mapData == null) {
			GenerateMapData();
		}
        /*
		if (Application.isEditor) {

			var children = new List<GameObject>();
			foreach (Transform child in transform) children.Add(child.gameObject);
			children.ForEach(child => DestroyImmediate(child.gameObject));
		
			currentTiles.Clear();
		}*/
        
		frameIndex = frameIndex + 1;

		//Debug.Log ("FrameIndex " + frameIndex);
		        
		int px = (int)Mathf.Floor(leaderTransform.position.x / tileSize);
		int pz = (int)Mathf.Floor(leaderTransform.position.z / tileSize);
        
        newTiles.Clear();

		for (int z = 0; z < generationRange*2+1; z++) {
			for (int x = 0; x < generationRange*2+1; x++) {

				int key = (px + maxSize / 2 + x) + (pz + maxSize / 2 + z) * maxSize;

				Tile tile = FindTile(currentTiles, key);

				if (tile == null) {

					float fx = -tileSize * (float)generationRange + (float)(x + px) * tileSize;
					float fz = -tileSize * (float)generationRange + (float)(z + pz) * tileSize;

					int prefabIndex = mapData[key];

					if (prefabs[prefabIndex] == null) {
					
						Debug.LogError("No prefab set for index: " + prefabIndex);
					
					} else {

						GameObject go = (GameObject)Instantiate(prefabs[prefabIndex], new Vector3(fx, 0.0f, fz), Quaternion.identity);
	                    go.transform.SetParent(transform);

						tile = new Tile();
						tile.key = key;
						tile.gameObject = go;
						tile.frameIndex = frameIndex;

						newTiles.Add (tile);
					
					}
                    
				} else {

					tile.frameIndex = frameIndex;

					newTiles.Add (tile);
				}
			}
		}

		for (int i = 0; i < currentTiles.Count; i++) {
		
			if (currentTiles[i].frameIndex != frameIndex) {
                /*
				if (Application.isEditor) {
					DestroyImmediate(currentTiles[i].gameObject);
				} else {*/
					Destroy(currentTiles[i].gameObject);
				/*}*/
			} 
		}

		currentTiles.Clear();
		for (int i = 0; i < newTiles.Count; i++) {
			currentTiles.Add(newTiles[i]);
		}

                
    }
    

}
