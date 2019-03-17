using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimePurchaseInjector : MonoBehaviour {
	
	public TextAsset[] client_defined_purchases;
	public TextAsset[] purchases_to_add_at_runtime;
	public TextAsset[] purchases_which_modify_existing;

	public string[] purchases_to_delete_at_runtime;

	public PurchaseSystemManager m_purchase_manager;


	void Start () 
	{
		foreach(TextAsset t in client_defined_purchases)
		{
			PurchasableItem deserialized_purchase = JsonUtility.FromJson<PurchasableItem>(t.text);
			m_purchase_manager.addOrModifyPurchase(deserialized_purchase);
		}
	}

	//These methods alow runtime modification of the purchases. I am relying on localy stored text assets to do this, but the 
	//strings holding JSon serialized PurchasableObjects could come from anywhere

	public void addRuntimePurchases()
	{
		foreach(TextAsset t in purchases_to_add_at_runtime)
		{
			PurchasableItem deserialized_purchase = JsonUtility.FromJson<PurchasableItem>(t.text);
			m_purchase_manager.addOrModifyPurchase(deserialized_purchase);
		}
		PurchaseUIManager.RefreshAllUI();
	}

	public void addModifyingPurchases()
	{
		foreach(TextAsset t in purchases_which_modify_existing)
		{
			PurchasableItem deserialized_purchase = JsonUtility.FromJson<PurchasableItem>(t.text);
			m_purchase_manager.addOrModifyPurchase(deserialized_purchase);
		}
		PurchaseUIManager.RefreshAllUI();
	}

	public void deletePurchases()
	{
		foreach(string s in purchases_to_delete_at_runtime)
		{
			m_purchase_manager.deletePurchase(s);
		}
		PurchaseUIManager.RefreshAllUI();
	}
	

}
