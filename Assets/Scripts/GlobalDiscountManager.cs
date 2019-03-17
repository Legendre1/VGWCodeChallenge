using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalDiscountManager : MonoBehaviour {

	//Singleton Implementation so I can take advantage of Monobehavior Update method for monitoring expirations

	private class ActiveDiscount
	{
		public float discount_amount;
		public DateTime expiration_time;
	}

	private static List<ActiveDiscount> s_active_discounts;
	private static float s_calculated_discount_factor;
	private static GlobalDiscountManager s_instance;
	private static OnDiscountsUpdated s_on_discounts_updated;

	public static void SubscribeToDiscountUpdates(OnDiscountsUpdated callback)
	{
		s_on_discounts_updated += callback;
	}

	public static float GetDiscountFactor()
	{
		if(s_calculated_discount_factor == 0.0f)
		{
			//discount factor has never been calculated, initialize its value
			//could also be handled via script execution order, but this is more foolproof
			CalculateDiscountFactor();
		}

		return s_calculated_discount_factor;
	}

	private static void CalculateDiscountFactor()
	{
		//calculates a normalized float to apply to (multiply) all prices in game
		float price_discount_factor = 1.0f;
		if(s_active_discounts == null)
		{
			//no discounts have ever been applied, set full price factor
			s_calculated_discount_factor = price_discount_factor;
			return;
		}

		//apply discounts cumulatively (i.e. 2 10% discounts means 19% off)
		foreach(ActiveDiscount discount in s_active_discounts)
		{
			price_discount_factor *= (1.0f - discount.discount_amount);
		}

		//Dont let the discount factor go below the minimum specified in the instance
		s_calculated_discount_factor = Mathf.Clamp(price_discount_factor, s_instance.minimum_discount_factor, 1.0f);
	}

	public static void ApplyDiscount(float[] float_params)
	{
		float discount_amount = float_params[0];
		float duration_in_minutes = float_params[1];
		
		//ideally these discounts would be serialized out to userdata but thats way too much work for now
		if(s_active_discounts == null)
		{
			s_active_discounts = new List<ActiveDiscount>();
		}

		ActiveDiscount new_discount = new ActiveDiscount();
		new_discount.discount_amount = discount_amount;
		
		TimeSpan duration = TimeSpan.FromMinutes(duration_in_minutes);
		DateTime expiration_time = DateTime.Now.Add(duration);
		new_discount.expiration_time = expiration_time;

		s_active_discounts.Add(new_discount);

		CalculateDiscountFactor();
		s_on_discounts_updated();
	}

	//Nonstatic definitions

	public float minimum_discount_factor; //some minimum factor by which prices can be brought down to (i.e. 20%) to prevent abuse

	public delegate void OnDiscountsUpdated();

	void Start()
	{
		s_instance = this;
		CalculateDiscountFactor();
	}

	void Update()
	{
		DateTime now = DateTime.Now;

		if(s_active_discounts != null)
		{
			for(int n = s_active_discounts.Count - 1; n >= 0; n--)
			{
				ActiveDiscount discount = s_active_discounts[n];
				if(now.CompareTo(discount.expiration_time) > 0)
				{
					Debug.Log("Discount has expired");
					s_active_discounts.RemoveAt(n);
					CalculateDiscountFactor();
					s_on_discounts_updated();
				}
			}
		}
	}
}
