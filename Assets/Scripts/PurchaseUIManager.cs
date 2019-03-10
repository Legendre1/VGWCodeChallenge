using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUIManager : MonoBehaviour {

	public Text m_currency_display;
	public PurchaseSystemManager m_purchase_manager;
	public PurchaseScrollviewManager m_scrollview_manager;

	
	
	#region Mono Methods

	void Start () 
	{
		updateCurrencyDisplay();
		constructPurchaseScrollview();
	}
	
	#endregion

	

	#region Display Methods

	private void constructPurchaseScrollview()
	{
		List<PurchasableItem> purchase_list = m_purchase_manager.getAvailablePurchases();
		m_scrollview_manager.constructScrollview(purchase_list);
	}

	private void updateCurrencyDisplay()
	{
		m_currency_display.text = m_purchase_manager.CurrencyOwned.ToString();
	}

	#endregion 


	#region Button Methods

	public void debugFreeMoney()
	{
		m_purchase_manager.debugAddCurrency(1000);
		updateCurrencyDisplay();
	}

	public void debugUpdateScrollview()
	{
		constructPurchaseScrollview();
	}

	#endregion
	
}
