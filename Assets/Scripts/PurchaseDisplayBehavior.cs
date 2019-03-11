using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseDisplayBehavior : MonoBehaviour {

	public Text m_title_text;
	public Text m_description_text;
	public Text m_cost_text;


	public string m_cost_string; 
	public string m_insufficient_funds_string;

	private PurchaseUIManager m_ui_manager;
	private PurchasableItem m_item_data;
	private PurchaseSystemManager m_purchase_system;


	public void constructDisplay(PurchasableItem item_data, PurchaseUIManager ui_manager)
	{
		m_ui_manager = ui_manager;
		m_item_data = item_data;

		m_title_text.text = item_data.visible_name;
		m_description_text.text = item_data.description;

		setCostText();
	}

	public void refreshAvailability()
	{
		setCostText();
	}

	public void onButtonPressed()
	{
		getPurchaseSystemManager();

		PurchaseSystemManager.AwardedPurchaseCallback paid_purchase_calback = purchaseCallback;
		PurchaseSystemManager.AwardedPurchaseCallback awarded_item_calback = freeItemCallback;

		if(!m_purchase_system.processPurchase(m_item_data, paid_purchase_calback, awarded_item_calback))
		{
			//purchase failed for some reason, show a modal informing the user
			m_ui_manager.showFailedPurchaseResults();
		}
	}

	private void purchaseCallback(PurchasableItem item_data)
	{
		m_ui_manager.showSuccessfulPurchaseResults(item_data);
	}

	private void freeItemCallback(PurchasableItem item_data)
	{
		m_ui_manager.showFreeItemRewardedResults(item_data);
	}

	private void setCostText()
	{
		getPurchaseSystemManager();

		bool can_afford = m_purchase_system.canAffordPurchase(m_item_data);

		if(can_afford)
		{
			float discounted_cost = m_item_data.currency_cost * GlobalDiscountManager.GetDiscountFactor();
			m_cost_text.text = string.Format(m_cost_string, discounted_cost.ToString());
		}
		else
		{
			m_cost_text.text = m_insufficient_funds_string;
		}
	}

	private void getPurchaseSystemManager()
	{
		if(m_purchase_system == null)
		{
			m_purchase_system = PurchaseSystemManager.GetInstance();
		}
	}
}
