﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class TalkingDataDemoScript : MonoBehaviour {
	
	const int left = 90;
	const int height = 50;
	const int top = 120;
	int width = Screen.width - left * 2;
	const int step = 60;
	const string account = "user01";
	string request_id = null;

	void OnGUI() {
		
		int i = 0;
		GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "Demo Menu");

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnRegister")) {
			TalkingDataPlugin.OnRegister ("user01", TalkingDataAccountType.ANONYMOUS, "abc");
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnLogin")) {
			TalkingDataPlugin.OnLogin ("user01", TalkingDataAccountType.TYPE1, "abc");
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnPlaceOrder")) {
			TalkingDataOrder order = TalkingDataOrder.CreateOrder ("order01", 2466400, "CNY");
			order.AddItem ("A1660", "手机", "iPhone 7", 538800, 2);
			order.AddItem ("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
			TalkingDataPlugin.OnPlaceOrder ("user01", order);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnOrderPaySucc")) {
			TalkingDataOrder order = TalkingDataOrder.CreateOrder ("order01", 2466400, "CNY");
			order.AddItem ("A1660", "手机", "iPhone 7", 538800, 2);
			order.AddItem ("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
			TalkingDataPlugin.OnOrderPaySucc ("user01", "AliPay", order);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnViewItem")) {
			TalkingDataPlugin.OnViewItem ("A1660", "手机", "iPhone 7", 538800);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnAddItemToShoppingCart")) {
			TalkingDataPlugin.OnAddItemToShoppingCart ("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "OnViewShoppingCart")) {
			TalkingDataShoppingCart shoppingCart = TalkingDataShoppingCart.CreateShoppingCart ();
			if (shoppingCart != null) {
				shoppingCart.AddItem ("A1660", "手机", "iPhone 7", 538800, 2);
				shoppingCart.AddItem ("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
				TalkingDataPlugin.OnViewShoppingCart (shoppingCart);
			}
		}
		
		if (GUI.Button(new Rect(left, top + step * i++, width, height), "TrackPageBegin")) {
			TalkingDataPlugin.TrackPageBegin("page_name");
		}
		
		if (GUI.Button(new Rect(left, top + step * i++, width, height), "TrackPageEnd")) {
			TalkingDataPlugin.TrackPageEnd("page_name");
		}
		
		if (GUI.Button(new Rect(left, top + step * i++, width, height), "TrackEvent")) {
			TalkingDataPlugin.TrackEvent("action_id");
		}
		
		if (GUI.Button(new Rect(left, top + step * i++, width, height), "TrackEventWithLabel")) {
			TalkingDataPlugin.TrackEventWithLabel("action_id", "action_label");
		}
		
		if (GUI.Button(new Rect(left, top + step * i++, width, height), "TrackEventWithParameters")) {
			Dictionary<string, object> dic = new Dictionary<string, object>();
			dic.Add("StartApp"+"StartAppTime", "startAppMac"+"#"+"02/01/2013 09:52:24");
			dic.Add("IntValue", 1);
			TalkingDataPlugin.TrackEventWithParameters("action_id", "action_label", dic);
		}
	}
	
	void Start () {
		Debug.Log("start...!!!!!!!!!!");
		TalkingDataPlugin.SetLogEnabled(true);
		TalkingDataPlugin.SetExceptionReportEnabled(true);
		TalkingDataPlugin.SessionStarted("E7538D90715219B3A2272A3E07E69C57", "your_channel_id");
#if UNITY_IPHONE
#if UNITY_5 || UNITY_5_6_OR_NEWER
		UnityEngine.iOS.NotificationServices.RegisterForNotifications(
			UnityEngine.iOS.NotificationType.Alert |
			UnityEngine.iOS.NotificationType.Badge |
			UnityEngine.iOS.NotificationType.Sound);
#else
		UnityEngine.iOS.NotificationServices.RegisterForNotifications(
			UnityEngine.iOS.NotificationType.Alert |
			UnityEngine.iOS.NotificationType.Badge |
			UnityEngine.iOS.NotificationType.Sound);
#endif
#endif
	}
	
	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
#if UNITY_IPHONE
		TalkingDataPlugin.SetDeviceToken();
		TalkingDataPlugin.HandlePushMessage();
#endif
	}
	
	void OnDestroy (){
		TalkingDataPlugin.SessionStoped();
		Debug.Log("onDestroy");
	}
	
	void Awake () {
		Debug.Log("Awake");
	}
	
	void OnEnable () {
		Debug.Log("OnEnable");
	}
	
	void OnDisable () {
		Debug.Log("OnDisable");
	}
}
