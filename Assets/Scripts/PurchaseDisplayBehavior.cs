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

	public void onButtonPressed()
	{

	}

	private void setCostText()
	{
		getPurchaseSystemManager();

		bool can_afford = m_purchase_system.canAffordPurchase(m_item_data);

		if(can_afford)
		{
			m_cost_text.text = string.Format(m_cost_string, m_item_data.currency_cost.ToString());
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
