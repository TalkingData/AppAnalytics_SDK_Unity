using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Threading;

public enum TalkingDataAccountType {
	ANONYMOUS 	= 0,
	REGISTERED 	= 1,
	SINA_WEIBO 	= 2,
	QQ 			= 3,
	QQ_WEIBO 	= 4,
	ND91 		= 5,
	WEIXIN		= 6,
	TYPE1 		= 11,
	TYPE2 		= 12,
	TYPE3 		= 13,
	TYPE4 		= 14,
	TYPE5 		= 15,
	TYPE6 		= 16,
	TYPE7 		= 17,
	TYPE8 		= 18,
	TYPE9 		= 19,
	TYPE10 		= 20
}

public class TalkingDataPlugin {
	const string version = "4.0.0";

#if UNITY_IPHONE
	/* Interface to native implementation */
	
	[DllImport ("__Internal")]
	private static extern void tdSetSDKFramework(int tag);

	[DllImport ("__Internal")]
	private static extern void tdSetAntiCheatingEnabled(bool enable);
	
	[DllImport ("__Internal")]
	private static extern void tdSessionStarted(string appKey, string channelId);
	
	[DllImport ("__Internal")]
	private static extern void tdSetExceptionReportEnabled(bool enable);
	
	[DllImport ("__Internal")]
	private static extern void tdSetLatitude(double latitude, double longitude);
	
	[DllImport ("__Internal")] 
	private static extern void tdSetLogEnabled(bool enable);
	
	[DllImport ("__Internal")]
	private static extern void tdOnRegister(string accountId, int type, string name);
	
	[DllImport ("__Internal")]
	private static extern void tdOnLogin(string accountId, int type, string name);
	
	[DllImport ("__Internal")]
	private static extern void tdTrackEvent(string eventId);
	
	[DllImport ("__Internal")]
	private static extern void tdTrackEventLabel(string eventId, string eventLabel);
	
	[DllImport ("__Internal")]
	private static extern void tdTrackEventParameters(string eventId, string eventLabel, 
		string[] keys, string[] stringValues, double[] numberValues, int count);
	
	[DllImport ("__Internal")]
	private static extern void tdOnPlaceOrder(string account, string orderJson);
	
	[DllImport ("__Internal")]
	private static extern void tdOnOrderPaySucc(string account, string payType, string orderJson);
	
	[DllImport ("__Internal")]
	private static extern void tdOnViewItem(string itemId, string category, string name, int unitPrice);
	
	[DllImport ("__Internal")]
	private static extern void tdOnAddItemToShoppingCart(string item, string category, string name, int unitPrice, int amount);
	
	[DllImport ("__Internal")]
	private static extern void tdOnViewShoppingCart(string shoppingCartJson);
	
	[DllImport ("__Internal")]
	private static extern void tdTrackPageBegin(string pageName);
	
	[DllImport ("__Internal")]
	private static extern void tdTrackPageEnd(string pageName);
	
	[DllImport ("__Internal")]
	private static extern void tdSetDeviceToken(byte[] deviceToken, int length);
	
	[DllImport ("__Internal")]
	private static extern void tdHandlePushMessage(string message);
	
	private static bool hasTokenBeenObtained = false;
#endif
	
	/* Public interface for use inside C# / JS code */
	
	
	// provider 可以是‘wgs84’，‘gcj02’，‘bd09ll’ 等，请根据您选择的定位方式添加参数，错误的参数将导致跟定位功能相关的报表数据异常。
	public static void SetLocation(double latitude, double longitude, string provider)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdSetLatitude(latitude, longitude);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("setLocation", currActivity, latitude, longitude, provider);
#endif
		}
	}
	
	public static void SetLogEnabled(bool enable)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdSetLogEnabled(enable);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.SetStatic("LOG_ON", enable);
#endif
		}
	}
	
	public static void SetAntiCheatingEnabled(bool enable)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdSetAntiCheatingEnabled(enable);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("setAntiCheatingEnabled", currActivity, enable);
#endif
		}
	}

	/* Public interface for use inside C# / JS code */
	public static void SessionStarted(string appKey, string channelId)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			Debug.Log("TalkingData App Analytics Unity3d SDK version is " + version);
#if UNITY_IPHONE
			Debug.Log("ios start");
			tdSetSDKFramework(2);
			tdSessionStarted(appKey, channelId);
#endif
#if UNITY_ANDROID
			AndroidJavaClass dz = new AndroidJavaClass("com.tendcloud.tenddata.dz");
			dz.SetStatic<int>("a", 2);
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			Debug.Log("android start");
			tCAgent.CallStatic("init", currActivity, appKey, channelId);
			tCAgent.CallStatic("onResume", currActivity);
#endif
		}
	}
	
	public static void SessionStoped()
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_ANDROID
			Debug.Log("android stop");
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("onPause", currActivity);
#endif
		}
	}
	
	public static void SetExceptionReportEnabled(bool enable)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdSetExceptionReportEnabled(enable);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("setReportUncaughtExceptions", enable);
#endif
		}
	}
	private static string oaid = null;
	public static string GetOAID() {
		//if the platform is real device
		if (oaid == null && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			oaid = tCAgent.CallStatic<string>("getOAID", activity);
#endif
		}
		return oaid;
	}
	
	public static void OnRegister(string accountId, TalkingDataAccountType type, string name)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdOnRegister(accountId, (int)type, name);
#endif
#if UNITY_ANDROID
			AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDAccount$AccountType");
			AndroidJavaObject typeObj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onRegister", accountId, typeObj, name);
#endif
		}
	}
	
	public static void OnLogin(string accountId, TalkingDataAccountType type, string name)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdOnLogin(accountId, (int)type, name);
#endif
#if UNITY_ANDROID
			AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDAccount$AccountType");
			AndroidJavaObject typeObj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onLogin", accountId, typeObj, name);
#endif
		}
	}
	
	public static void TrackEvent(string eventId)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdTrackEvent(eventId);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("onEvent", currActivity, eventId);
#endif
		}
	}
	
	public static void TrackEventWithLabel(string eventId, string eventLabel)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdTrackEventLabel(eventId, eventLabel);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("onEvent", currActivity, eventId, eventLabel);
#endif
		}
	}
	
	public static void TrackEventWithParameters(string eventId, string eventLabel, 
		Dictionary<string, object> parameters)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) 
		{
#if UNITY_IPHONE
			if (parameters != null && parameters.Count > 0) 
			{
				int count = parameters.Count;
				string []keys = new string[count];
				string []stringValues = new string[count];
				double []numberValues = new double[count];
				int index = 0;
				foreach (KeyValuePair<string, object> kvp in parameters)
				{
					if (kvp.Value is string) 
					{
						keys[index] = kvp.Key;
						stringValues[index] = (string)kvp.Value;
					}
					else
					{
						try
						{
							double tmp = System.Convert.ToDouble(kvp.Value);
							numberValues[index] = tmp;
							keys[index] = kvp.Key;
						}
						catch(System.Exception)
						{
							count--;
							continue;
						}
					}
					index++;
				}
				tdTrackEventParameters(eventId, eventLabel, keys, stringValues, numberValues, count);
			}
			else
			{
				tdTrackEventLabel(eventId, eventLabel);
			}
#endif
#if UNITY_ANDROID
			if (parameters != null && parameters.Count > 0) 
			{
				int count = parameters.Count;
				AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap", count);
				
				IntPtr method_Put = AndroidJNIHelper.GetMethodID(map.GetRawClass(), 
					"put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				
				object[] args = new object[2];
				foreach (KeyValuePair<string, object> kvp in parameters) {
					args[0] = new AndroidJavaObject("java.lang.String", kvp.Key);
					if (typeof(System.String).IsInstanceOfType(kvp.Value)) {
						args[1] = new AndroidJavaObject("java.lang.String", kvp.Value);
					} else {
						args[1] = new AndroidJavaObject("java.lang.Double", ""+kvp.Value);
					}
					AndroidJNI.CallObjectMethod(map.GetRawObject(), method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
				}
				AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
				AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
				tCAgent.CallStatic("onEvent", currActivity, eventId, eventLabel, map);
			}
			else
			{
				AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
				AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
				tCAgent.CallStatic("onEvent", currActivity, eventId, eventLabel);
			}
#endif
		}
	}
	
	public static void OnPlaceOrder(string accountId, TalkingDataOrder order)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdOnPlaceOrder(accountId, order.ToString());
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onPlaceOrder", accountId, order.javaObj);
#endif
		}
	}
	
	public static void OnOrderPaySucc(string accountId, string payType, TalkingDataOrder order)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdOnOrderPaySucc(accountId, payType, order.ToString());
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onOrderPaySucc", accountId, payType, order.javaObj);
#endif
		}
	}
	
	public static void OnViewItem(string itemId, string category, string name, int unitPrice)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdOnViewItem(itemId, category, name, unitPrice);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onViewItem", itemId, category, name, unitPrice);
#endif
		}
	}
	
	public static void OnAddItemToShoppingCart(string itemId, string category, string name, int unitPrice, int amount)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdOnAddItemToShoppingCart(itemId, category, name, unitPrice, amount);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onAddItemToShoppingCart", itemId, category, name, unitPrice, amount);
#endif
		}
	}
	
	public static void OnViewShoppingCart(TalkingDataShoppingCart shoppingCart)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			Debug.Log ("ShoppingCart:" + shoppingCart);
#if UNITY_IPHONE
			tdOnViewShoppingCart(shoppingCart.ToString());
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			tCAgent.CallStatic("onViewShoppingCart", shoppingCart.javaObj);
#endif
		}
	}
	
	public static void TrackPageBegin(string pageName)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdTrackPageBegin(pageName);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("onPageStart", currActivity, pageName);
#endif
		}
	}
	
	public static void TrackPageEnd(string pageName)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdTrackPageEnd(pageName);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tCAgent = new AndroidJavaClass("com.tendcloud.tenddata.TCAgent");
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			tCAgent.CallStatic("onPageEnd", currActivity, pageName);
#endif
		}
	}
	
#if UNITY_IPHONE
#if UNITY_5 || UNITY_5_6_OR_NEWER
	public static void SetDeviceToken() {
		if (!hasTokenBeenObtained) {
			byte[] deviceToken = UnityEngine.iOS.NotificationServices.deviceToken;
			if(deviceToken != null) {
				tdSetDeviceToken(deviceToken, deviceToken.Length);
				hasTokenBeenObtained = true;
			}
		}
	}
	
	public static void HandlePushMessage() {
		UnityEngine.iOS.RemoteNotification[] notifications = UnityEngine.iOS.NotificationServices.remoteNotifications;
		if (notifications != null) {
			UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
			foreach (UnityEngine.iOS.RemoteNotification rn in notifications) {
				foreach (DictionaryEntry de in rn.userInfo) {
					if (de.Key.ToString().Equals("sign")) {
						string sign = de.Value.ToString();
						tdHandlePushMessage(sign);
					}
				}
			}
		}
	}
#else
	public static void SetDeviceToken() {
		if (!hasTokenBeenObtained) {
			byte[] deviceToken = UnityEngine.iOS.NotificationServices.deviceToken;
			if(deviceToken != null) {
                tdSetDeviceToken(deviceToken, deviceToken.Length);
				hasTokenBeenObtained = true;
			}
		}
	}

	public static void HandlePushMessage() {
		UnityEngine.iOS.RemoteNotification[] notifications = UnityEngine.iOS.NotificationServices.remoteNotifications;
		if (notifications != null) {
			UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
			foreach (UnityEngine.iOS.RemoteNotification rn in notifications) {
				foreach (DictionaryEntry de in rn.userInfo) {
					if (de.Key.ToString().Equals("sign")) {
						string sign = de.Value.ToString();
						tdHandlePushMessage(sign);
					}
				}
			}
		}
	}
#endif
#endif
}
