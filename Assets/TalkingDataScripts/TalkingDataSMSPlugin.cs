using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Threading;

public class TalkingDataSMSPlugin : MonoBehaviour {

	public delegate void SuccDelegate(string requestId);
	public delegate void FailedDelegate(int errorCode, string errorMessage);

	public SuccDelegate   _applySucc;
	public FailedDelegate _applyFailed;
	public SuccDelegate   _verifySucc;
	public FailedDelegate _verifyFailed;

	static TalkingDataSMSPlugin _tdSmsPlugin;

#if UNITY_IPHONE
	/* Interface to native implementation */
	
	[DllImport ("__Internal")]
	private static extern void tdSmsInit(string appKey, string secretId);
	
	[DllImport ("__Internal")]
	private static extern void tdSmsApplyAuthCode(string countryCode, string mobile);
	
	[DllImport ("__Internal")]
	private static extern void tdSmsReapplyAuthCode(string countryCode, string mobile, string requestId);
	
	[DllImport ("__Internal")] 
	private static extern void tdSmsVerifyAuthCode(string countryCode, string mobile, string authCode);

#endif

	/* Public interface for use inside C# / JS code */

	public static void Init(string appKey, string secretId)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			if (_tdSmsPlugin == null)
			{
				_tdSmsPlugin = GameObject.FindObjectOfType<TalkingDataSMSPlugin>();
				if (_tdSmsPlugin == null)
				{
					_tdSmsPlugin = new GameObject("TalkingDataSMSPlugin").AddComponent<TalkingDataSMSPlugin>();
				}
			}
#if UNITY_IPHONE
			tdSmsInit(appKey, secretId);
#endif
#if UNITY_ANDROID
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass tdSmsClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataUnityPlugin");
			Debug.Log("android sms start");
			tdSmsClass.CallStatic("init", currActivity, appKey, secretId);
#endif
		}
	}
	
	public static void ApplyAuthCode(string countryCode, string mobile, SuccDelegate succMethod, FailedDelegate failedMethod)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			_tdSmsPlugin._applySucc = succMethod;
			_tdSmsPlugin._applyFailed = failedMethod;
#if UNITY_IPHONE
			tdSmsApplyAuthCode(countryCode, mobile);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdSmsClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataUnityPlugin");
			tdSmsClass.CallStatic("applyAuthCode", countryCode, mobile);
#endif
		}
	}
	
	public static void ReapplyAuthCode(string countryCode, string mobile, string requestId, SuccDelegate succMethod, FailedDelegate failedMethod)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			_tdSmsPlugin._applySucc = succMethod;
			_tdSmsPlugin._applyFailed = failedMethod;
#if UNITY_IPHONE
			tdSmsReapplyAuthCode(countryCode, mobile, requestId);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdSmsClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataUnityPlugin");
			tdSmsClass.CallStatic("reapplyAuthCode", countryCode, mobile, requestId);
#endif
		}
	}
	
	public static void VerifyAuthCode(string countryCode, string mobile, string authCode, SuccDelegate succMethod, FailedDelegate failedMethod)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			_tdSmsPlugin._verifySucc = succMethod;
			_tdSmsPlugin._verifyFailed = failedMethod;
#if UNITY_IPHONE
			tdSmsVerifyAuthCode(countryCode, mobile, authCode);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdSmsClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataUnityPlugin");
			tdSmsClass.CallStatic("verifyAuthCode", countryCode, mobile, authCode);
#endif
		}
	}
	
	void OnApplySucc(string requestId)
	{
		_applySucc.Invoke(requestId);
	}
	
	void OnApplyFailed(string args)
	{
		string msg = args.Replace("[", "").Replace("]", "");
		string[] msgArr = msg.Split(',');
		int errorCode = int.Parse(msgArr[0]);
		string errorMessage = msgArr[1];
		errorMessage = errorMessage.Replace ("\"", "").Replace ("\"", "");
		_applyFailed.Invoke(errorCode, errorMessage);
	}
	
	void OnVerifySucc(string requestId)
	{
		_verifySucc.Invoke(requestId);
	}
	
	void OnVerifyFailed(string args)
	{
		string msg = args.Replace("[", "").Replace("]", "");
		string[] msgArr = msg.Split(',');
		int errorCode = int.Parse(msgArr[0]);
		string errorMessage = msgArr [1];
		errorMessage = errorMessage.Replace ("\"", "").Replace ("\"", "");
		_verifyFailed.Invoke(errorCode, errorMessage);
	}
}
