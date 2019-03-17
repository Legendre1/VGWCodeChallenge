using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
public class MethodCallUtility : MonoBehaviour {

	public static void CallOnPurchaseMethods(List<PurchasableItem.MethodCallsOnPurchase> on_purchase_methods)
	{
		foreach(PurchasableItem.MethodCallsOnPurchase method_call in on_purchase_methods)
		{
			
			string class_name = method_call.class_name;

			Type class_type = Type.GetType(class_name);
			if(class_type == null)
			{
				Debug.LogError("Class not found, aborting");
				return;
			}

			string method_name = method_call.method_name;			

			MethodInfo method_info = null;

			if(method_call.static_method)
			{
				method_info = class_type.GetMethod(method_name);
				if(method_info == null)
				{
					Debug.LogError("Static method " + method_name + " not found on class " + class_name + " not found, aborting");
					return;
				}

				ParameterInfo[] param_info = method_info.GetParameters();

				object[] method_parameters = ConstructMethodParameters(method_call, param_info);
				
				method_info.Invoke(null, method_parameters);
			}
			else
			{
				method_info = class_type.GetMethod(method_name);
				if(method_info == null)
				{
					Debug.LogError("Method " + method_name + " not found on class " + class_name + " not found, aborting");
					return;
				}

				ParameterInfo[] param_info = method_info.GetParameters();

				object[] method_parameters = ConstructMethodParameters(method_call, param_info);
				
				var class_instance = FindObjectOfType(class_type);

				method_info.Invoke(class_instance, method_parameters);
			}
		}
	}

	public static object[] ConstructMethodParameters(PurchasableItem.MethodCallsOnPurchase method_call_info, ParameterInfo[] param_info)
	{
		object[] method_parameters = new object[param_info.Length];

		for(int n = 0; n < param_info.Length; n++)
		{
			Type param_type = param_info[n].ParameterType;
			
			if(param_type == typeof(String))
			{
				method_parameters[n] = method_call_info.string_parameters[0];
			}
			else if(param_type == typeof(int))
			{
				method_parameters[n] = method_call_info.int_parameters[0];
			}
			else if(param_type == typeof(float))
			{
				method_parameters[n] = method_call_info.float_parameters[0];
			}
			else if(param_type == typeof(String[]))
			{
				method_parameters[n] = method_call_info.string_parameters;
			}
			else if(param_type == typeof(int[]))
			{
				method_parameters[n] = method_call_info.int_parameters;
			}
			else if(param_type == typeof(float[]))
			{
				method_parameters[n] = method_call_info.float_parameters;
			}
		}

		return method_parameters;
	}
}
