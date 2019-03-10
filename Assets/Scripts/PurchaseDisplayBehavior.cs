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

	public void constructDisplay(PurchasableItem item_data, PurchaseUIManager ui_manager)
	{
		m_ui_manager = ui_manager;
		m_item_data = item_data;

		m_title_text.text = item_data.visible_name;
		m_description_text.text = item_data.description;

		setCostText();
	}

	private void setCostText()
	{
		int currency_owned = m_ui_manager.CurrencyOwned;

		if(currency_owned >= m_item_data.currency_cost)
		{
			m_cost_text.text = string.Format(m_cost_string, m_item_data.currency_cost.ToString());
		}
		else
		{
			m_cost_text.text = m_insufficient_funds_string;
		}
	}
}
