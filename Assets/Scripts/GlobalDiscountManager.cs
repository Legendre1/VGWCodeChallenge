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

	public static float GetDiscountFactor()
	{
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

		s_calculated_discount_factor = price_discount_factor;
	}

	public static void ApplyDiscount(float discount_amount, float duration_in_minutes)
	{
		//idealy these discounts would be serialized out to userdata but thats way too much work for now
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
	}

	//Nonstatic definitions

	public float minimum_discount_factor; //some minimum factor by which prices can be brought down to (i.e. 20%) to prevent abuse
	public float test_val;

	void Start()
	{
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
					s_calculated_discount_factor = Mathf.Clamp(s_calculated_discount_factor, minimum_discount_factor, 1.0f);
				}
			}
		}
		test_val = s_calculated_discount_factor;
	}
}
