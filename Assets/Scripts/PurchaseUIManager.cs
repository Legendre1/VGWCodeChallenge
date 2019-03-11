using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUIManager : MonoBehaviour {

	public Text m_currency_display;
	public PurchaseSystemManager m_purchase_manager;
	public PurchaseScrollviewManager m_scrollview_manager;
	public ModalSystemManager m_modal_manager;

	public string m_purchase_confirmation_text;//this would ideally be set by the backend or some external data definition
	
	#region Mono Methods

	void Start () 
	{
		updateCurrencyDisplay();
		constructPurchaseScrollview();
	}
	
	#endregion

	#region Purchase Reaction

	public void showPurchaseResults(PurchasableItem purchased_item)
	{
		//called when an item is purchased succesfuly, show a little UI teling the user the purchase went through
		string concatenated_purchase_confirmation = string.Format(m_purchase_confirmation_text, purchased_item.visible_name);
		showInformativeModal(concatenated_purchase_confirmation);

		updateCurrencyDisplay();
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

	#region Information Modal Subsystem

	public void showInformativeModal(string modal_content)
	{
		//this shows a single button modal display for purchase results info etc
		m_modal_manager.showSingleButtonModal(modal_content);
	}

	#endregion
	
}
