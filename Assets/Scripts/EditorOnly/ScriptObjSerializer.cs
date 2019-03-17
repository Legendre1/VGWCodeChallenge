using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScriptObjSerializer : MonoBehaviour {

	public PurchasableItem script_object;
	public string data_location;

	public void serializeToTextFile()
	{
		string serialized_purchase = JsonUtility.ToJson(script_object);
		Debug.Log("Output: " + serialized_purchase);

		string path = "Assets/Resources/" + data_location + "/" + script_object.purchase_key + ".txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(serialized_purchase);
        writer.Close();
	}
}
