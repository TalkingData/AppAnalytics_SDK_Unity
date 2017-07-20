using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Threading;


public class TalkingDataShoppingCart {
	
#if UNITY_ANDROID
	public AndroidJavaObject javaObj;
#endif
	
#if UNITY_IPHONE
	private string items = "";
#endif
	
	private static TalkingDataShoppingCart shoppingCart = null;
	
	/* Public interface for use inside C# code */
	
	public static TalkingDataShoppingCart CreateShoppingCart()
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			shoppingCart = new TalkingDataShoppingCart();
#if UNITY_ANDROID
			AndroidJavaClass javaClass = new AndroidJavaClass("com.tendcloud.tenddata.ShoppingCart");
			shoppingCart.javaObj = javaClass.CallStatic<AndroidJavaObject>("createShoppingCart");
#endif
			return shoppingCart;
		}
		
		return null;
	}
	
	public TalkingDataShoppingCart() {
	}
	
	public TalkingDataShoppingCart AddItem(string itemId, string category, string name, int unitPrice, int amount)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_ANDROID
			if (this.javaObj != null)
			{
				this.javaObj.Call<AndroidJavaObject>("addItem", itemId, category, name, unitPrice, amount);
			}
#endif
#if UNITY_IPHONE
			string item = "{\"itemId\":\"" + itemId + "\",\"category\":\"" + category + "\",\"name\":\"" + name + "\",\"unitPrice\":" + unitPrice + ",\"amount\":" + amount + "}";
			if (this.items.Length > 0)
			{
				this.items += ",";
			}
			this.items += item;
#endif
		}
		
		return this;
	}
	
#if UNITY_IPHONE
	public override string ToString()
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			string orderStr = "{\"items\":[" + items + "]}";
			return orderStr;
		}
		
		return null;
	}
#endif
}
