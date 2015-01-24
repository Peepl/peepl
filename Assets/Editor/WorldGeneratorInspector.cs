using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WorldGenerator))] 
public class WorldGeneratorInspector : Editor {

	public override void OnInspectorGUI() {

		DrawDefaultInspector();
		
		WorldGenerator worldGenerator = (WorldGenerator)target;

		if (GUILayout.Button("Test"))
		{
			worldGenerator.Test();
		}
	}
}
