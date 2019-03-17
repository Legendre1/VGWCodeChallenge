using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor (typeof(ScriptObjSerializer))]
public class SOSerializerEditor : Editor {

	public override void OnInspectorGUI()
	{

		ScriptObjSerializer myBehavior = (ScriptObjSerializer)target;

		DrawDefaultInspector ();

		if(GUILayout.Button("Serialize"))
		{
			myBehavior.serializeToTextFile();
		}


	}
}
