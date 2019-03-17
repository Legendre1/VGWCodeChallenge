using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PurchaseSystemManager : MonoBehaviour {

	#region Member Vars
	
	//this data is injected externally using the purchase modification methods below
	private Dictionary<string, PurchasableItem> m_purchase_dictionary;

	private int m_currency_owned;

	public delegate void AwardedPurchaseCallback(PurchasableItem awarded_item);

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
		m_purchase_dictionary = new Dictionary<string, PurchasableItem>();
		s_purchase_system_manager = this;
		pullCurrencyFromBackend();
	}
	#endregion

	#region Purchase Data Access

	public void addOrModifyPurchase(PurchasableItem purchase_data)
	{
		//this can either add new or overwrite existing purchase

		//check for a purchase with this key. if it exists, overwrite
		PurchasableItem existing_purchase = getPurchasableItemByKey(purchase_data.purchase_key);
		if(existing_purchase != null)
		{
			m_purchase_dictionary[purchase_data.purchase_key] = purchase_data;
		}
		else
		{
			//purchase with tis key does not exist, add it to the dictionary as a new item
			m_purchase_dictionary.Add(purchase_data.purchase_key, purchase_data);
		}

	}

	public void deletePurchase(string purchase_key)
	{
		PurchasableItem existing_purchase = getPurchasableItemByKey(purchase_key);
		if(existing_purchase != null)
		{
			m_purchase_dictionary.Remove(purchase_key);
		}
	}

	public List<PurchasableItem> getAvailablePurchases()
	{
		//rather than looking directly at the m_available_purchases array, this method returns a nice clean list, and does a little error checking
		//to avoid handing off bad data to the UI system or whatever else asks for it
		
		List<PurchasableItem> purchase_list = new List<PurchasableItem>();

		foreach(KeyValuePair<string, PurchasableItem> pair in m_purchase_dictionary)
		{
			PurchasableItem purchase = pair.Value;
			bool purchase_entry_is_acceptable = true;

			//if the purchase data is null for some reason, just skip it
			if(purchase == null)
			{
				Debug.Log("Null entry, skipping");
				purchase_entry_is_acceptable = false;
			}

			
			if(purchase_entry_is_acceptable)
			{
				string unique_purchase_key = purchase.purchase_key;
				//make sure there isnt already a purchase item with this unique id
				foreach(PurchasableItem compare_purchase in purchase_list)
				{
					if(compare_purchase.purchase_key == unique_purchase_key)
					{
						Debug.LogError("Detected a duplicate entry, skipping");
						purchase_entry_is_acceptable = false;
					}
				}

				
			}
			

			if(purchase_entry_is_acceptable)
			{
				purchase_list.Add(purchase);
			}
		}

		return purchase_list;		
	}

	#endregion

	#region Purchase Handling

	public bool canAffordPurchase(PurchasableItem item_data)
	{
		int cost = Mathf.RoundToInt(item_data.currency_cost * GlobalDiscountManager.GetDiscountFactor());

		if(m_currency_owned >= cost)
		{
			return true;
		}
		return false;
	}

	public bool processPurchase(PurchasableItem item_purchased, AwardedPurchaseCallback purchase_callback, AwardedPurchaseCallback awarded_callback)
	{
		//this handles all backend aspects of the purchase, including the awarding of bonuses, application of global discounts 
		//returns true if purchase is successful, otherwise false
		if(!canAffordPurchase(item_purchased))
		{
			Debug.LogError("Not enough currency to buy this");
			return false;
		}

		//charge the user for the purchase
		m_currency_owned -= Mathf.RoundToInt(item_purchased.currency_cost * GlobalDiscountManager.GetDiscountFactor());
		pushCurrencyToBackend();

		//give any payouts to the user (discounts etc)
		awardPayouts(item_purchased);

		//invoke the callback for this purchase, if any
		if(purchase_callback != null)
		{
			purchase_callback(item_purchased);
		}

		//roll for and award and secondary items this purchase has a chance of granting
		awardSecondaryItems(item_purchased, awarded_callback);

		return true;
		
	}

	private void awardSecondaryItems(PurchasableItem item_data, AwardedPurchaseCallback awarded_calback)
	{
		List<PurchasableItem.FreeItemOnPurchase> potential_free_items = item_data.possible_free_items;

		for(int n = 0; n < potential_free_items.Count; n++)
		{
			PurchasableItem.FreeItemOnPurchase maybe_free_item = potential_free_items[n];

			float randy = Random.Range(0.0f, 1.0f);
			if(maybe_free_item.chance_for_free_item >= randy)
			{
				//successful roll, find the item
				PurchasableItem free_item = getPurchasableItemByKey(maybe_free_item.free_item_purchase_key);

				//if it exists, award it and notify the user
				if(free_item != null)
				{
					awarded_calback(free_item);
					awardPayouts(free_item);
				}

				//could call this method again if we want the possibility for recursive secondary items. ill avoid for now
				//awardSecondaryItems(free_item)
			}
		}
	}

	private void awardPayouts(PurchasableItem item_data)
	{
		//apply any global discounts this purchase grants
		foreach(PurchasableItem.GlobalDiscountOnPurchase discount_on_purchase in item_data.global_discounts)
		{
			GlobalDiscountManager.ApplyDiscount(discount_on_purchase.global_discount, discount_on_purchase.discount_duration_minutes);
		}

		//if the purchase has some other kind of payout, like a premium currency, it would be handled here
	}

	//New implementation which relies on dictionary storage rather than a list. Complexity = O(1)
	private PurchasableItem getPurchasableItemByKey(string item_key)
	{
		PurchasableItem result = null;

		if(m_purchase_dictionary.TryGetValue(item_key, out result))
		{
			return result;
		}
		else
		{
			return null;
		}
	}

	//this var doesnt do anything, I am keeping it around so I can write the Linq version of the getPurchasableItem method as requested
	private PurchasableItem[] m_available_purchases;
	
	//As requested, Linq implementation. You asked me not to use a local variable, I'm hoping this is what you were getting at
	public PurchasableItem getPurchasableItemByKeyUsingLinq(string item_key)
	{
		return ( from purchase in m_available_purchases
                            where purchase.purchase_key == item_key
                            select purchase).First();
	} 

	//Original implementation. Complexity is O(n)
	// private PurchasableItem getPurchasableItemByKey(string item_key)
	// {
	// 	foreach(PurchasableItem item in m_available_purchases)
	// 	{
	// 		if(item.purchase_key == item_key)
	// 		{
	// 			return item;
	// 		}
	// 	}

	// 	Debug.LogError("Item not found with key " + item_key);
	// 	return null;
	// }

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
