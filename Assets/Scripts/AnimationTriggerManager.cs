using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationTriggerManager  {


	public static void TriggerAnimation(string[] string_params)
	{
		GameObject go = GameObject.Find(string_params[0]);
		if(go == null)
		{
			Debug.LogError("Gameobject " + string_params[0] + " not found");
			return;
		}

		Animator animator = go.GetComponent<Animator>();
		if(animator == null)
		{
			Debug.LogError("Animator not found on object " + string_params[0]);
			return;
		}

		animator.SetTrigger(string_params[1]);
	}
}
