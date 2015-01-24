using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//[ExecuteInEditMode]
public class WorldGenerator : MonoBehaviour {

	private class Tile {
		public GameObject gameObject;
		public int tileType;
		public Vector3 localPos;
	}

	private class Block {
		public List<Tile> tiles;
		public Vector3 worldPos;
		public bool enabled;
	}

	public float tileSize;
	
	public List<GameObject> prefabs;

	public int seed = 1234;

	private int maxTiles = 128;
	
	private float killDistance = 400.0f;

	private float appearDistance = 300.0f;
	
	private int maxSize = 16;

	private int blockSize = 4;
	
	private Transform leaderTransform;

	private List<TileInfo> tileInfo = new List<TileInfo>();

	private List<Block> blocks = new List<Block>();
	
	public void Init(GameObject leaderGameObject) {
		leaderTransform = leaderGameObject.transform;			
	}

	// Use this for initialization
	void Start () {

		// cache tileinfos
		for (int i = 0; i < prefabs.Count; i++) {
			tileInfo.Add(prefabs[i].GetComponent<TileInfo>());
		}

		CreateBlocks();
	}
	
	// Update is called once per frame
	void Update () {

		UpdateBlocks();
	}

	void Reset() {

		tileSize = 50.0f;
	}

	public void Test() {

	}
	
	void CreateBlocks() {
	
		int nblock = maxSize / blockSize;

		for (int i = 0; i < nblock; i++) {
			for (int j = 0; j < nblock; j++) {
				CreateBlock (i, j);			
			}
		}	
	}

	TileInfo GetRandomTile() {
	
		int tileType = Random.Range(0, tileInfo.Count);
		
		TileInfo info = tileInfo[tileType];
    	
		return info;
	}

	bool TestFit(int[] tiles, int size, int x, int z) {
	
		bool canAdd = true;
		
		for (int tz = 0; tz < size; tz++) {
			for (int tx = 0; tx < size; tx++) {
				if (tiles[(x + tx) + (z + tz) * blockSize] != -1) {
					Debug.Log("CANT ADD");
					return false;
                }
            }
        }

		return true;
    
	}
    
    void CreateBlock(int blockX, int blockZ) {

		int[] tiles = new int[blockSize * blockSize];

		for (int i = 0; i < tiles.Length; i++) {
			tiles[i] = -1;
		}

<<<<<<< HEAD
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
=======
		Block block = new Block();
		block.tiles = new List<Tile>();
		block.enabled = false;

		blocks.Add(block);

		float blockWorldSize = blockSize * tileSize;
>>>>>>> anssi-branch

		block.worldPos = new Vector3(-(float)maxSize * tileSize / 2.0f + (float)blockX * blockWorldSize,
		                             0.0f,
		                             -(float)maxSize * tileSize / 2.0f + (float)blockZ * blockWorldSize);

		for (int z = 0; z < blockSize; z++) {
			for (int x = 0; x < blockSize; x++) {

				int tileType = Random.Range(0, tileInfo.Count);

				TileInfo info = tileInfo[tileType];
                
				// can't add big tile on the edge, so change to small

				if ((z + info.size) > (blockSize) ||
				    (x + info.size) > (blockSize)) {

					info = tileInfo[0];
					tileType = 0;
					//continue;
				}

				// check if this block fits

				bool canAdd = true;
                
				if (!TestFit (tiles, info.size, x, z)) {
				
					// not fitting, test again with small

					tileType = 0;
					info = tileInfo[0];
                    
					if (!TestFit(tiles, info.size, x, z)) {

						// totally blockked
						canAdd = false;
					}
					
				}

				if (canAdd) {
				
					// it fit new tile...

					Tile tile = new Tile();

					tile.localPos = new Vector3(
						-(float)blockSize * tileSize / 2.0f + (float)x * tileSize + (float)(info.size) * (float)tileSize / 2.0f,
						0.0f,
						-(float)blockSize * tileSize / 2.0f + (float)z * tileSize + (float)(info.size) * (float)tileSize / 2.0f
							);

					for (int tz = 0; tz < info.size; tz++) {
						for (int tx = 0; tx < info.size; tx++) {
							tiles[(x + tx) + (z + tz) * blockSize] = 0;
						}
					}

					tile.tileType = tileType;
				
					block.tiles.Add(tile);
				}
			}

		}
	
	}

	void UpdateBlocks() {
	
		for (int i = 0; i < blocks.Count; i++) {
		
<<<<<<< HEAD
			if (currentTiles[i].frameIndex != frameIndex) {
                /*
				if (Application.isEditor) {
					DestroyImmediate(currentTiles[i].gameObject);
				} else {*/
					Destroy(currentTiles[i].gameObject);
				/*}*/
			} 
=======
			//Debug.Log ("pos " + blocks[i].worldPos.ToString());

			if (Vector3.Distance(blocks[i].worldPos, leaderTransform.position) < appearDistance) {
				EnableBlock(blocks[i]);
			}

		}

		for (int i = 0; i < blocks.Count; i++) {

			if (Vector3.Distance(blocks[i].worldPos, leaderTransform.position) > killDistance) {
				DisableBlock(blocks[i]);			
			}
		
>>>>>>> anssi-branch
		}
	
	}

	void EnableBlock(Block block) {

		if (!block.enabled) {

			block.enabled = true;

			for (int i = 0; i < block.tiles.Count; i++) {
				block.tiles[i].gameObject = 
					Instantiate(prefabs[block.tiles[i].tileType], 
					            block.worldPos + block.tiles[i].localPos, Quaternion.identity) as GameObject;
			}
		}

	}

	void DisableBlock(Block block) {

		if (block.enabled) {

			block.enabled = false;
			
			for (int i = 0; i < block.tiles.Count; i++) {
				Destroy(block.tiles[i].gameObject);
	        }
		}

	}
	
}
