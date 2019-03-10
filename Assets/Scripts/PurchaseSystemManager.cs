using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseSystemManager : MonoBehaviour {

	#region Member Vars
	//for this code test, the list of possible purchases is simple a public array that can be modified from the Unity Editor
	//In a real application, this would be procedurally modified based on server data etc
	public PurchasableItem[] m_available_purchases;

	private int m_currency_owned;

	#endregion

	#region Public Properties

	public int CurrencyOwned
	{
		get
		{
			return m_currency_owned;
		}
	}

	#endregion

	#region Startup
	void Start()
	{
		s_purchase_system_manager = this;
		pullCurrencyFromBackend();
	}
	#endregion

	#region Purchase Data Access

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
			//make sure there isnt already a purchase item with this unique id

			purchase_list.Add(purchase);

		}

		return purchase_list;		
	}

	#endregion

	#region Purchase Handling

	public bool canAffordPurchase(PurchasableItem item_data)
	{
		int cost = item_data.currency_cost;

		if(m_currency_owned >= cost)
		{
			return true;
		}
		return false;
	}

	public void processPurchase(PurchasableItem item_purchased)
	{
		//this handles all backend aspects of the purchase, including the awarding of bonuses, application of global discounts [System.Serializable]
		
	}

	#endregion

	#region Currency

	public void debugAddCurrency(int amount)
	{
		Debug.Log("Adding free currency via debug: " + amount);
		m_currency_owned += amount;
		pushCurrencyToBackend();
	}

	private void pullCurrencyFromBackend()
	{
		//ideally this would call into some sort of local or server side userdata manager, for now Ill just call into Unity playerPrefs directly
		m_currency_owned = PlayerPrefs.GetInt("owned_currency");
	}

	private void pushCurrencyToBackend()
	{
		//ideally this would call into some sort of local or server side userdata manager, for now Ill just call into Unity playerPrefs directly
		PlayerPrefs.SetInt("owned_currency", m_currency_owned);
	}

	#endregion

	#region Singleton Access

	private static PurchaseSystemManager s_purchase_system_manager;

	public static PurchaseSystemManager GetInstance()
	{
		return s_purchase_system_manager;
	}

	#endregion
}
