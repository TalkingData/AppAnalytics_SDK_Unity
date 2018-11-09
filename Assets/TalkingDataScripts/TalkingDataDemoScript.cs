using UnityEngine;
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
	const string mobile = "13800138000";
	const string account = "user01";
	const string smsId = "";
	string request_id = null;

	void OnEAuthSuccess(TalkingDataEAuthType type, string requestId, string phoneNumber, TalkingDataEAuthPhoneNumSeg[] phoneNumSeg) {
		Debug.Log ("Unity Demo OnEAuthSuccess" + " requestId:" + requestId + " phoneNumber:" + phoneNumber);
		foreach (var seg in phoneNumSeg) {
			Debug.Log ("phoneNumSeg:" + seg.begin + "-" + seg.end);
		}
		request_id = requestId;
		switch (type) {
		case TalkingDataEAuthType.TDEAuthTypeApplyCode:
			Debug.Log ("申请认证码成功");
			break;
		case TalkingDataEAuthType.TDEAuthTypeChecker:
			Debug.Log ("此账号已认证");
			break;
		case TalkingDataEAuthType.TDEAuthTypePhoneMatch:
			Debug.Log ("此账号与手机号已绑定");
			break;
		case TalkingDataEAuthType.TDEAuthTypeBind:
			Debug.Log ("认证成功");
			break;
		case TalkingDataEAuthType.TDEAuthTypeUnBind:
			Debug.Log ("已成功解除绑定");
			break;
		}
	}
	
	void OnEAuthFailed(TalkingDataEAuthType type, int errorCode, string errorMessage) {
		Debug.Log ("Unity Demo OnEAuthFailed" + " type:" + type + " errorCode:" + errorCode + " errorMessage:" + errorMessage);
	}

	void OnGUI() {
		
		int i = 0;
		GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "Demo Menu");
		
		if (GUI.Button (new Rect (left, top + step * i++, width, height), "Apply Auth Code")) {
			TalkingDataEAuth.ApplyAuthCode ("86", mobile, TalkingDataAuthCodeType.smsAuth, account, smsId);
		}
		
		if (GUI.Button (new Rect (left, top + step * i++, width, height), "Reapply Auth Code")) {
			TalkingDataEAuth.ReapplyAuthCode ("86", mobile, TalkingDataAuthCodeType.voiceCallAuth, account, smsId, request_id);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "Is Verify Account")) {
			TalkingDataEAuth.IsVerifyAccount (account);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "Is Mobile Match Account")) {
			TalkingDataEAuth.IsMobileMatchAccount (account, "86", mobile);
		}
		
		if (GUI.Button (new Rect (left, top + step * i++, width, height), "Bind")) {
			TalkingDataEAuth.Bind ("86", mobile, "001178", account);
		}

		if (GUI.Button (new Rect (left, top + step * i++, width, height), "Unbind")) {
			TalkingDataEAuth.Unbind ("86", mobile, account);
		}

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
		TalkingDataEAuth.SuccessDelegate successMethod = new TalkingDataEAuth.SuccessDelegate (this.OnEAuthSuccess);
		TalkingDataEAuth.FailedDelegate failedMethod = new TalkingDataEAuth.FailedDelegate (this.OnEAuthFailed);
		TalkingDataEAuth.Init("506610b4d5a142809cca181e32d70e21", "7c1573fbd1cc33d336f01838dc2d606e", successMethod, failedMethod);
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
