using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUIManager : MonoBehaviour {

	public Text m_currency_display;
	public PurchaseSystemManager m_purchase_manager;
	public PurchaseScrollviewManager m_scrollview_manager;
	public ModalSystemManager m_modal_manager;

	//these would ideally be set by the backend or some external data definition
	public string m_purchase_confirmation_text;//shown when an item is purchased directly with currency
	public string m_free_item_confirmation_text;//shown when an item is awarded for free
	public string m_purchase_failure_text;//shown when a purchase fails

	
	#region Mono Methods

	void Start () 
	{
		updateCurrencyDisplay();
		refreshScrollview();
	}
	
	#endregion

	#region Purchase Reaction

	public void showSuccessfulPurchaseResults(PurchasableItem purchased_item)
	{
		//called when an item is purchased succesfully
		//show a little UI teling the user the purchase went through
		string concatenated_purchase_confirmation = string.Format(m_purchase_confirmation_text, purchased_item.visible_name);
		showInformativeModal(concatenated_purchase_confirmation);
		//trigger any animations the purchase specifies
		triggerPurchaseAnimations(purchased_item);
		refreshPurchaseAvailability();
		updateCurrencyDisplay();
	}

	public void showFreeItemRewardedResults(PurchasableItem awarded_item)
	{
		//called when an item is awarded for free, as a result of a random reward from another purchase
		string concatenated_purchase_confirmation = string.Format(m_free_item_confirmation_text, awarded_item.visible_name);
		showInformativeModal(concatenated_purchase_confirmation);
		//trigger any animations the purchase specifies
		triggerPurchaseAnimations(awarded_item);
	}

	public void showFailedPurchaseResults()
	{
		showInformativeModal(m_purchase_failure_text);
	}

	#endregion

	#region Display Methods

	private void refreshScrollview()
	{
		List<PurchasableItem> purchase_list = m_purchase_manager.getAvailablePurchases();
		m_scrollview_manager.refreshScrollview(purchase_list);
	}

	private void refreshPurchaseAvailability()
	{
		m_scrollview_manager.refreshAvailability();
	}

	private void updateCurrencyDisplay()
	{
		m_currency_display.text = m_purchase_manager.CurrencyOwned.ToString();
	}

	private void triggerPurchaseAnimations(PurchasableItem item_data)
	{
		List<PurchasableItem.AnimationTriggerOnPurchase> animations_on_purchase = item_data.animation_triggers;

		for(int n = 0; n < animations_on_purchase.Count; n++)
		{
			PurchasableItem.AnimationTriggerOnPurchase animation = animations_on_purchase[n];
			AnimationTriggerManager.TriggerAnimation(animation.animator_name, animation.animator_trigger_param);
		}
		
	}

	#endregion 


	#region Button Methods

	public void debugFreeMoney()
	{
		m_purchase_manager.debugAddCurrency(1000);
		refreshPurchaseAvailability();
		updateCurrencyDisplay();
	}

	public void debugUpdateScrollview()
	{
		refreshScrollview();
	}

	#endregion

	#region Information Modal Subsystem

	public void showInformativeModal(string modal_content)
	{
		//this shows a single button modal display for purchase results info etc
		m_modal_manager.showSingleButtonModal(modal_content);
	}

	#endregion

	#region Static Methods

	private static PurchaseUIManager s_ui_manager;

	public static void RefreshAllUI()
	{
		if(s_ui_manager == null)
		{
			s_ui_manager = FindObjectOfType<PurchaseUIManager>();
		}

		s_ui_manager.refreshScrollview();
	}

	#endregion
	
}
