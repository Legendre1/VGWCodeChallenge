using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PurchasableItem :  IComparable {

	//mandatory vars (these must be defined for all purchasable items)
	public string visible_name;//Item name seen by the user
	public string description;//Item description, user visible
	public string purchase_key;//unique name used to correlate with store backend etc. not user visible
	public float order_priority;//lower numbers show in the list first. negative values allowed, but typicaly reserved for promotions etc
	public int currency_cost;//the cost, in game currency, to purchase this item


	//optional vars (these may or may not be defined for a given purchase)
	public List<AnimationTriggerOnPurchase> animation_triggers;
	public List<FreeItemOnPurchase> possible_free_items;
	public List<GlobalDiscountOnPurchase> global_discounts;




	//Subclasses (these could be defined in their own files, but for brevity I wil define them here)

	[System.Serializable]
	public class AnimationTriggerOnPurchase
	{
		public string animator_name;//name of the scene object to be animated
		public string animator_trigger_param;//name of the animator (trigger) parameter to invoke
	}

	[System.Serializable]
	public class FreeItemOnPurchase
	{
		public float chance_for_free_item;//normalized chance to recieve a free item;
		public string free_item_purchase_key;//(unique) purchase key of item that might be won
	}

	[System.Serializable]
	public class GlobalDiscountOnPurchase
	{
		public float global_discount;//normalized discount applied to al items
		public int discount_duration_minutes;//duration in minutes after which the global discount expires
	}


	//IComparable Implementation
	public int CompareTo(object other)
	{
		return this.order_priority.CompareTo(((PurchasableItem)other).order_priority);
	}

}
