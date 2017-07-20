using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Threading;


public class TalkingDataOrder {
	
#if UNITY_ANDROID
	public AndroidJavaObject javaObj;
#endif
	
#if UNITY_IPHONE
	private string orderId = null;
	private int total;
	private string currencyType = null;
	private string items = "";
#endif
	
	/* Public interface for use inside C# code */
	
	public static TalkingDataOrder CreateOrder(string orderId, int total, string currencyType)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TalkingDataOrder order = new TalkingDataOrder();
#if UNITY_ANDROID
			AndroidJavaClass javaClass = new AndroidJavaClass("com.tendcloud.tenddata.Order");
			order.javaObj = javaClass.CallStatic<AndroidJavaObject>("createOrder", orderId, total, currencyType);
#endif
#if UNITY_IPHONE
			order.orderId = orderId;
			order.total = total;
			order.currencyType = currencyType;
#endif
			return order;
		}
		
		return null;
	}
	
	public TalkingDataOrder() {
	}
	
	public TalkingDataOrder AddItem(string itemId, string category, string name, int unitPrice, int amount)
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
			string orderStr = "{\"orderId\":\"" + this.orderId + "\",\"total\":" + this.total + ",\"currencyType\":\"" + this.currencyType + "\",\"items\":[" + items + "]}";
			return orderStr;
		}
		
		return null;
	}
#endif
}
