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

	private float specialDistribution = 0.4f;
	private float basicDistribution = 0.6f;

	private float killDistance = 800.0f;

	private float appearDistance = 500.0f;
	
	private int maxSize = 64;

	private int blockSize = 4;
	
	private Transform leaderTransform;

	private List<TileInfo> tileInfo = new List<TileInfo>();

	private List<Block> blocks = new List<Block>();

	private Dictionary<int, int> tileTypesInScene = new Dictionary<int, int>();

	private bool villageSpawned;

	private Vector3 villagePosition;
	
	public void SetLeader(GameObject leaderGameObject) {
		leaderTransform = leaderGameObject.transform;			
	}

	public Vector3 GetVillagePosition() {
		return villagePosition;
	}

	// Use this for initialization
	void Start () {
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

	public void Generate() {

		villageSpawned = false;
		villagePosition = new Vector3(0.0f, 0.0f, 0.0f);
		// cache tileinfos
		for (int i = 0; i < prefabs.Count; i++) {
			
			TileInfo info = prefabs[i].GetComponent<TileInfo>();
			if (info == null) {
				Debug.LogError("No tile info in prefab");
			}
			
			tileInfo.Add(prefabs[i].GetComponent<TileInfo>());
		}
		
		CreateBlocks();
	}
	
	void CreateBlocks() {
	
		int nblock = maxSize / blockSize;

		for (int i = 0; i < nblock; i++) {
			for (int j = 0; j < nblock; j++) {
				CreateBlock (i, j);			
			}
		}	
	}

	int GetRandomTileType() {
		int tileType = 0;
		var rnd = Random.value;
		if (rnd < specialDistribution || rnd < basicDistribution) {

			do {
				if(rnd < specialDistribution)
					tileType = Random.Range(1, 5);
				else
					tileType = Random.Range(5, tileInfo.Count);
			} while (tileType == 11);

			int count = 0;
							
			if (tileTypesInScene.TryGetValue(tileType, out count)) {

				/*if (count >= tileInfo[tileType].maxCount) {
					return 0;
				}*/

				tileTypesInScene[tileType] = count + 1;

			} else {
			
				tileTypesInScene.Add(tileType, 1);
			}
		}

		return tileType;
	}

	bool TestFit(int[] tiles, int size, int x, int z) {
	
		for (int tz = 0; tz < size; tz++) {
			for (int tx = 0; tx < size; tx++) {
				if (tiles[(x + tx) + (z + tz) * blockSize] != -1) {
					return false;
                }
            }
        }

		return true;
    
	}

	Vector3 GetTileWorldPos(int blockX, int blockZ, int tileX, int tileZ, int size) {

		float blockWorldSize = blockSize * tileSize;

		Vector3 worldPos = new Vector3(-(float)maxSize * tileSize / 2.0f + (float)blockX * blockWorldSize,
		                               0.0f,
		                               -(float)maxSize * tileSize / 2.0f + (float)blockZ * blockWorldSize);

		Vector3 localPos = new Vector3(
			-(float)blockSize * tileSize / 2.0f + (float)tileX * tileSize + (float)(size) * (float)tileSize / 2.0f,
			0.0f,
			-(float)blockSize * tileSize / 2.0f + (float)tileZ * tileSize + (float)(size) * (float)tileSize / 2.0f
			);
		
		return worldPos + localPos;

	}

	bool IsCenterTile(int blockX, int blockZ, int tileX, int tileZ, int size) {

		Vector3 pos = GetTileWorldPos(blockX, blockZ, tileX, tileZ, size);

		if (Vector3.Distance(pos, Vector3.zero) < 80) {
			return true;
		}

		return false;

	}
    
    void CreateBlock(int blockX, int blockZ) {

		int[] tiles = new int[blockSize * blockSize];

		for (int i = 0; i < tiles.Length; i++) {
			tiles[i] = -1;
		}

		Block block = new Block();
		block.tiles = new List<Tile>();
		block.enabled = false;

		blocks.Add(block);

		float blockWorldSize = blockSize * tileSize;

		block.worldPos = new Vector3(-(float)maxSize * tileSize / 2.0f + (float)blockX * blockWorldSize,
		                             0.0f,
		                             -(float)maxSize * tileSize / 2.0f + (float)blockZ * blockWorldSize);

		for (int z = 0; z < blockSize; z++) {
			for (int x = 0; x < blockSize; x++) {

				int tileType = 0;

				if (!IsCenterTile(blockX, blockZ, x, z, 1)) {
					tileType = GetRandomTileType();
				}
				else
				{
					if(!villageSpawned)
					{
						tileType = 11;
						villageSpawned = true;
					}
				}

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

					if (tileType == 11) {

						Debug.Log ("Village created");

						villagePosition = block.worldPos + tile.localPos;
					}

					tile.tileType = tileType;
				
					block.tiles.Add(tile);
				}
			}

		}
	
	}

	void UpdateBlocks() {
	
		for (int i = 0; i < blocks.Count; i++) {
		
			if (Vector3.Distance(blocks[i].worldPos, leaderTransform.position) < appearDistance) {
				EnableBlock(blocks[i]);
			}

		}

		for (int i = 0; i < blocks.Count; i++) {

			if (Vector3.Distance(blocks[i].worldPos, leaderTransform.position) > killDistance) {
				DisableBlock(blocks[i]);			
			}
		
		}
	
	}

	void EnableBlock(Block block) {

		if (!block.enabled) {

			block.enabled = true;

			for (int i = 0; i < block.tiles.Count; i++) {

				Quaternion rot = Quaternion.identity;

				/*if (block.tiles[i].tileType > 0) {
					rot = Quaternion.Euler(-90, -180, 0);
				}*/

				block.tiles[i].gameObject = 
					Instantiate(prefabs[block.tiles[i].tileType], 
					            block.worldPos + block.tiles[i].localPos, rot) as GameObject;
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
