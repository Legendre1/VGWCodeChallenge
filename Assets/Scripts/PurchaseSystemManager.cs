using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseSystemManager : MonoBehaviour {

	//for this code test, the list of possible purchases is simple a public array that can be modified from the Unity Editor
	//In a real application, this would be procedurally modified based on server data etc
	public PurchasableItem[] m_available_purchases;

	public List<PurchasableItem> getAvailablePurchases()
	{
		//rather than looking directly at the m_available_purchases array, this method returns a nice clean list, and does a little error checking
		//to avoid handing off bad data to the UI system or whatever else asks for it
		
		List<PurchasableItem> purchase_list = new List<PurchasableItem>();

		foreach(PurchasableItem purchase in m_available_purchases)
		{
			//if the purchase data is nul for some reason, just skip it
			if(purchase == null)
			{
				continue;
			}

			
			string unique_purchase_key = purchase.purchase_key;

		}

		return purchase_list;		
	}

}
