using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Threading;

public enum TalkingDataEAuthType {
	TDEAuthTypeApplyCode	= 0,
	TDEAuthTypeChecker		= 1,
	TDEAuthTypePhoneMatch	= 2,
	TDEAuthTypeBind			= 3,
	TDEAuthTypeUnBind		= 4
}

public enum TalkingDataAuthCodeType {
	smsAuth					= 0,
	voiceCallAuth			= 1
}

public class TalkingDataEAuth : MonoBehaviour {

	public delegate void SuccessDelegate(TalkingDataEAuthType type, string requestId, string phoneNumber, TalkingDataEAuthPhoneNumSeg[] phoneNumSeg);
	public delegate void FailedDelegate(TalkingDataEAuthType type, int errorCode, string errorMessage);

	public SuccessDelegate  _successDelegate;
	public FailedDelegate   _failedDelegate;

	static TalkingDataEAuth _tdEAuth;

#if UNITY_IPHONE
	/* Interface to native implementation */
	
	[DllImport ("__Internal")]
	private static extern void tdEAuthInit(string appId, string secretId);
	
	[DllImport ("__Internal")]
	private static extern void tdEAuthApplyAuthCode(string countryCode, string mobile, TalkingDataAuthCodeType type, string accountName);
	
	[DllImport ("__Internal")]
	private static extern void tdEAuthReapplyAuthCode(string countryCode, string mobile, TalkingDataAuthCodeType type, string accountName, string requestId);
	
	[DllImport ("__Internal")]
	private static extern void tdEAuthIsVerifyAccount(string accountName);

	[DllImport ("__Internal")]
	private static extern void tdEAuthIsMobileMatchAccount(string accountName, string countryCode, string mobile);

	[DllImport ("__Internal")] 
	private static extern void tdEAuthBind(string countryCode, string mobile, string authCode, string accountName);

	[DllImport ("__Internal")]
	private static extern void tdEAuthUnbind(string countryCode, string mobile, string accountName);

#endif

	/* Public interface for use inside C# / JS code */

	public static void Init(string appId, string secretId, SuccessDelegate successMethod, FailedDelegate failedMethod)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			if (_tdEAuth == null)
			{
				_tdEAuth = GameObject.FindObjectOfType<TalkingDataEAuth>();
				if (_tdEAuth == null)
				{
					_tdEAuth = new GameObject("TalkingDataEAuth").AddComponent<TalkingDataEAuth>();
				}
			}
			_tdEAuth._successDelegate = successMethod;
			_tdEAuth._failedDelegate = failedMethod;
#if UNITY_IPHONE
			tdEAuthInit(appId, secretId);
#endif
#if UNITY_ANDROID
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			Debug.Log("android eAuth start");
			tdEAuthClass.CallStatic("init", currActivity, appId, secretId);
#endif
		}
	}
	
	public static void ApplyAuthCode(string countryCode, string mobile, TalkingDataAuthCodeType type, string accountName)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdEAuthApplyAuthCode(countryCode, mobile, type, accountName);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TalkingDataEAuth$TDAuthCodeType");
			AndroidJavaObject typeObj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
			tdEAuthClass.CallStatic("applyAuthCode", countryCode, mobile, accountName, typeObj);
#endif
		}
	}
	
	public static void ReapplyAuthCode(string countryCode, string mobile, TalkingDataAuthCodeType type, string accountName, string requestId)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdEAuthReapplyAuthCode(countryCode, mobile, type, accountName, requestId);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TalkingDataEAuth$TDAuthCodeType");
			AndroidJavaObject typeObj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
			tdEAuthClass.CallStatic("reapplyAuthCode", countryCode, mobile, requestId, accountName, typeObj);
#endif
		}
	}
	
	public static void IsVerifyAccount(string accountName)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdEAuthIsVerifyAccount(accountName);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			tdEAuthClass.CallStatic("isVerifyAccount", accountName);
#endif
		}
	}

	public static void IsMobileMatchAccount(string accountName, string countryCode, string mobile)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdEAuthIsMobileMatchAccount(accountName, countryCode, mobile);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			tdEAuthClass.CallStatic("isMobileMatchAccount", countryCode, mobile, accountName);
#endif
		}
	}

	public static void Bind(string countryCode, string mobile, string authCode, string accountName)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdEAuthBind(countryCode, mobile, authCode, accountName);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			tdEAuthClass.CallStatic("bind", countryCode, mobile, authCode, accountName);
#endif
		}
	}

	public static void Unbind(string countryCode, string mobile, string accountName)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
#if UNITY_IPHONE
			tdEAuthUnbind(countryCode, mobile, accountName);
#endif
#if UNITY_ANDROID
			AndroidJavaClass tdEAuthClass = new AndroidJavaClass("com.tendcloud.tenddata.unityplugin.TalkingDataEAuthPlugin");
			tdEAuthClass.CallStatic("unbind", countryCode, mobile, accountName);
#endif
		}
	}
	
	void OnRequestSuccess(string args)
	{
		Debug.Log ("Unity Plugin onRequestSuccess:" + args);
		TalkingDataEAuthSuccessArgs succArgs = JsonUtility.FromJson<TalkingDataEAuthSuccessArgs> (args);
		string requestId = succArgs.requestId != null ? succArgs.requestId : "";
		string phoneNumber = succArgs.phoneNumber != null ? succArgs.phoneNumber : "";
		TalkingDataEAuthPhoneNumSeg[] phoneNumSeg = succArgs.phoneNumSeg != null ? succArgs.phoneNumSeg : new TalkingDataEAuthPhoneNumSeg[0];
		_successDelegate.Invoke(succArgs.type, requestId, phoneNumber, phoneNumSeg);
	}

	void OnRequestFailed(string args)
	{
		Debug.Log ("Unity Plugin onRequestFailed:" + args);
		TalkingDataEAuthFailedArgs failedArgs = JsonUtility.FromJson<TalkingDataEAuthFailedArgs> (args);
		_failedDelegate.Invoke(failedArgs.type, failedArgs.errorCode, failedArgs.errorMessage);
	}
}

[Serializable]
public class TalkingDataEAuthPhoneNumSeg
{
	public string begin;
	public string end;
}

[Serializable]
public class TalkingDataEAuthSuccessArgs
{
	public TalkingDataEAuthType type;
	public string requestId;
	public string phoneNumber;
	public TalkingDataEAuthPhoneNumSeg[] phoneNumSeg;
}

[Serializable]
public class TalkingDataEAuthFailedArgs
{
	public TalkingDataEAuthType type;
	public int errorCode;
	public string errorMessage;
}
