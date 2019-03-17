using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ScriptObjSerializer : MonoBehaviour {

	public PurchasableItem script_object;
	public string data_location;

	public TextAsset deserialization_source;

	public void serializeToTextFile()
	{
		string serialized_purchase = JsonUtility.ToJson(script_object);
		Debug.Log("Output: " + serialized_purchase);

		string path = "Assets/Resources/" + data_location + "/" + script_object.purchase_key + ".txt";

        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(serialized_purchase);
        writer.Close();

		AssetDatabase.Refresh();
	}

	public void deSerializeFromTextFile()
	{
		PurchasableItem deserialized_purchase = JsonUtility.FromJson<PurchasableItem>(deserialization_source.text);
		script_object = deserialized_purchase;
	}

}
