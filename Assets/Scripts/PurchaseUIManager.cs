using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUIManager : MonoBehaviour {

	public Text m_currency_display;
	public PurchaseSystemManager m_purchase_manager;
	public PurchaseScrollviewManager m_scrollview_manager;

	private int m_currency_owned;
	
	#region Mono Methods

	void Start () 
	{
		//ideally this would cal into some sort of local or server side userdata manager, for now Ill just call into Unity playerPrefs directly
		m_currency_owned = PlayerPrefs.GetInt("owned_currency");
		updateCurrencyDisplay();
		constructPurchaseScrollview();
	}
	
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

	#region Display Methods

	private void constructPurchaseScrollview()
	{
		List<PurchasableItem> purchase_list = m_purchase_manager.getAvailablePurchases();
		m_scrollview_manager.constructScrollview(purchase_list);
	}

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
		constructPurchaseScrollview();
	}

	#endregion

	#region Internal Utility Methods

	private void writeCurrencyToUserdata()
	{
		PlayerPrefs.SetInt("owned_currency", m_currency_owned);
	}

	#endregion
	
}
