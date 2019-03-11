using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationTriggerManager  {

	public static void TriggerAnimation(string gameobject_name, string animator_trigger)
	{
		GameObject go = GameObject.Find(gameobject_name);
		if(go == null)
		{
			Debug.LogError("Gameobject " + gameobject_name + " not found");
			return;
		}

		Animator animator = go.GetComponent<Animator>();
		if(animator == null)
		{
			Debug.LogError("Animator not found on object " + gameobject_name);
			return;
		}

		animator.SetTrigger(animator_trigger);
	}
}
