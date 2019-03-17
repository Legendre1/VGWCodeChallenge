using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeItemAwarder : MonoBehaviour {

	private const string FREE_ITEM_AWARDED = "You won a free {0}";
	public static void ChanceToAwardFreeItem(string item_key, float chance_to_award)
	{
		Debug.Log("Chance to award free");
		float randy = UnityEngine.Random.Range(0.0f, 1.0f);
		if(randy < chance_to_award)
		{
			PurchasableItem free_item = PurchaseSystemManager.GetInstance().getPurchasableItemByKey(item_key);
			Debug.Log("Awarding " + free_item.visible_name);

			string free_item_notification = string.Format(FREE_ITEM_AWARDED, free_item.visible_name);
			ModalSystemManager.GetInstance().showSingleButtonModal(free_item_notification);
			
			//call any on_purchase methods specified
			if(free_item.on_purchase_methods != null)
			{
				MethodCallUtility.CallOnPurchaseMethods(free_item.on_purchase_methods);
			}
		}
	}
}
