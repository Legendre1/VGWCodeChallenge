using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimePurchaseInjector : MonoBehaviour {
	
	public TextAsset[] client_defined_purchases;

	public PurchaseSystemManager m_purchase_manager;

	void Start () 
	{
		foreach(TextAsset t in client_defined_purchases)
		{
			PurchasableItem deserialized_purchase = JsonUtility.FromJson<PurchasableItem>(t.text);
			Debug.Log("Deserialized purchase named " + deserialized_purchase.purchase_key);
			m_purchase_manager.addOrModifyPurchase(deserialized_purchase);
		}
	}
	

}
