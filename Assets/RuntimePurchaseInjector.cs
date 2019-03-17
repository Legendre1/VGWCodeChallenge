using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimePurchaseInjector : MonoBehaviour {
	
	public TextAsset[] client_defined_purchases;


	void Start () 
	{
		foreach(TextAsset t in client_defined_purchases)
		{
			PurchasableItem deserialized_purchase = JsonUtility.FromJson<PurchasableItem>(t.text);
			Debug.Log("Deserialized purchase named " + deserialized_purchase.purchase_key);
		}
	}
	

}
