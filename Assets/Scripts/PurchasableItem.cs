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
	public List<MethodCallsOnPurchase> on_purchase_methods;



	//Subclasses (these could be defined in their own files, but for brevity I wil define them here)

	[System.Serializable]
	public class MethodCallsOnPurchase
	{
		public string class_name;//name of the class to call in to
		public string method_name;//name of the method to call
		public bool static_method;//if true, cal a static method, if false, call a (presumably singleton) instance method

		//Method parameters. Could break it down even more than this if we want more versatility, but this should handle 90% of practical use cases, I think
		public string[] string_parameters;
		public int[] int_parameters;
		public float[] float_parameters;

	}

	


	//IComparable Implementation
	public int CompareTo(object other)
	{
		return this.order_priority.CompareTo(((PurchasableItem)other).order_priority);
	}

}
