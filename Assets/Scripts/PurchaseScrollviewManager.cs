using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseScrollviewManager : MonoBehaviour {

	//this behavior is responsible for constructing the scrollview given a set of purchasableItems

	public PurchaseUIManager m_ui_manager;
	public RectTransform m_content_root_transform;

	public GameObject m_purchase_entry_prefab;
	public float m_entry_vertical_spacing;

	private List<PurchaseDisplayBehavior> m_purchase_elements;

	public void refreshScrollview(List<PurchasableItem> purchasable_items)
	{
		destroyExistingScrollview();
		
		m_purchase_elements = new List<PurchaseDisplayBehavior>();

		int item_count = purchasable_items.Count;

		//resize the content transform to accomodate the list
		Vector2 content_root_size = m_content_root_transform.sizeDelta;
		m_content_root_transform.sizeDelta = new Vector2(content_root_size.x, item_count * m_entry_vertical_spacing);

		//Sort the list 
		purchasable_items.Sort();

		//Construct each element at its proper position
		float vertical_spawn_cursor = -m_entry_vertical_spacing / 2;//start the "cursor" at the midpoint of the first entry
		for(int n = 0; n < item_count; n++)
		{
			spawnPurchaseElement(purchasable_items[n], vertical_spawn_cursor);
			vertical_spawn_cursor -= m_entry_vertical_spacing;
		}
	}

	public void refreshAvailability()
	{
		for(int n = 0; n < m_purchase_elements.Count; n++)
		{
			m_purchase_elements[n].refreshAvailability();
		}
	}

	private void destroyExistingScrollview()
	{
		if(m_purchase_elements == null)
		{
			//nothing to destroy
			return;
		}

		for(int n = 0; n < m_purchase_elements.Count; n++)
		{
			Destroy(m_purchase_elements[n]);
		}
		m_purchase_elements.Clear();
	}

	private void spawnPurchaseElement(PurchasableItem item_data, float spawn_cursor)
	{
		//In a perfect world these would be spawned from an object pool rather than raw instantiation
		GameObject element_go = Instantiate(m_purchase_entry_prefab);
		RectTransform element_transform = element_go.GetComponent<RectTransform>();
		element_transform.SetParent(m_content_root_transform);
		element_transform.anchoredPosition = new Vector2(0, spawn_cursor);

		PurchaseDisplayBehavior element_display_behavior = element_go.GetComponent<PurchaseDisplayBehavior>();
		element_display_behavior.constructDisplay(item_data, m_ui_manager);
		m_purchase_elements.Add(element_display_behavior);
	}

}
