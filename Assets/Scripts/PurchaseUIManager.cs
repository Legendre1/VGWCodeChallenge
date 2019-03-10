using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUIManager : MonoBehaviour {

	public Text m_currency_display;
	public PurchaseSystemManager m_purchase_manager;

	private int m_currency_owned;
	
	void Start () 
	{
		//ideally this would cal into some sort of local or server side userdata manager, for now Ill just call into Unity playerPrefs directly
		m_currency_owned = PlayerPrefs.GetInt("owned_currency");
		updateCurrencyDisplay();
	}
	

	#region Display Methods

	private void updateCurrencyDisplay()
	{
		m_currency_display.text = m_currency_owned.ToString();
	}

	#endregion 


	#region Button Methods

	public void debugFreeMoney()
	{
		m_currency_owned += 1000;
		writeCurrencyToUserdata();
		updateCurrencyDisplay();
	}

	public void debugUpdateScrollview()
	{
		List<PurchasableItem> purchase_list = m_purchase_manager.getAvailablePurchases();
	}

	#endregion

	#region Internal Utility Methods

	private void writeCurrencyToUserdata()
	{
		PlayerPrefs.SetInt("owned_currency", m_currency_owned);
	}

	#endregion
	
}
