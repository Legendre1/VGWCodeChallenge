using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseScrollviewManager : MonoBehaviour {

	//this behavior is responsible for constructing the scrollview given a set of purchasableItems

	public PurchaseUIManager m_ui_manager;
	public Transform m_content_root_transform;

	public void constructScrollview(List<PurchasableItem> purchasable_items)
	{
		Debug.Log("Constructing scrollview");


	}

	private void destroyExistingScrollview()
	{

	}
	
}
