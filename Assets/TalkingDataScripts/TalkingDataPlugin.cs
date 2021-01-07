using UnityEngine;
using System.Collections.Generic;
#if UNITY_ANDROID
using System;
#endif
#if UNITY_IPHONE
using System.Runtime.InteropServices;
using System.Collections;
#endif


public enum TalkingDataProfileType
{
    ANONYMOUS = 0,
    REGISTERED = 1,
    SINA_WEIBO = 2,
    QQ = 3,
    QQ_WEIBO = 4,
    ND91 = 5,
    WEIXIN = 6,
    TYPE1 = 11,
    TYPE2 = 12,
    TYPE3 = 13,
    TYPE4 = 14,
    TYPE5 = 15,
    TYPE6 = 16,
    TYPE7 = 17,
    TYPE8 = 18,
    TYPE9 = 19,
    TYPE10 = 20
}


public static class TalkingDataPlugin
{
#if UNITY_ANDROID
    private static readonly string APP_ANALYTICS_CLASS = "com.tendcloud.tenddata.TCAgent";
    private static AndroidJavaClass appAnalyticsClass;
    private static AndroidJavaClass unityPlayerClass;
#endif

#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern string TDAAGetDeviceId();

    [DllImport("__Internal")]
    private static extern void TDAASetLogEnabled(bool enable);

    [DllImport("__Internal")]
    private static extern void TDAABackgroundSessionEnabled();

    [DllImport("__Internal")]
    private static extern void TDAASessionStarted(string appId, string channelId);

    [DllImport("__Internal")]
    private static extern void TDAASetExceptionReportEnabled(bool enable);

    [DllImport("__Internal")]
    private static extern void TDAASetLocation(double latitude, double longitude);

    [DllImport("__Internal")]
    private static extern void TDAAOnRegister(string profileId, int type, string name);

    [DllImport("__Internal")]
    private static extern void TDAAOnLogin(string profileId, int type, string name);

#if TDAA_STANDARD
    [DllImport("__Internal")]
    private static extern void TDAAOnViewItem(string itemId, string category, string name, int unitPrice);

    [DllImport("__Internal")]
    private static extern void TDAAOnAddItemToShoppingCart(string item, string category, string name, int unitPrice, int amount);

    [DllImport("__Internal")]
    private static extern void TDAAOnViewShoppingCart(string shoppingCartJson);

    [DllImport("__Internal")]
    private static extern void TDAAOnPlaceOrder(string profile, string orderJson);

    [DllImport("__Internal")]
    private static extern void TDAAOnOrderPaySucc(string profile, string payType, string orderJson);
#endif

#if TDAA_CUSTOM
    [DllImport("__Internal")]
    private static extern void TDAATrackEvent(string eventId);

    [DllImport("__Internal")]
    private static extern void TDAATrackEventLabel(string eventId, string eventLabel);

    [DllImport("__Internal")]
    private static extern void TDAATrackEventParameters(string eventId, string eventLabel, string parameters);

    [DllImport("__Internal")]
    private static extern void TDAATrackEventValue(string eventId, string eventLabel, string parameters, double eventValue);
#endif

#if TDAA_PAGE
    [DllImport("__Internal")]
    private static extern void TDAATrackPageBegin(string pageName);

    [DllImport("__Internal")]
    private static extern void TDAATrackPageEnd(string pageName);
#endif

#if TDAA_PUSH
    [DllImport("__Internal")]
    private static extern void TDAASetDeviceToken(byte[] deviceToken, int length);

    [DllImport("__Internal")]
    private static extern void TDAAHandlePushMessage(string message);

    private static bool hasTokenBeenObtained = false;
#endif
#endif

#if UNITY_ANDROID
    private static AndroidJavaObject GetCurrentActivity()
    {
        if (unityPlayerClass == null)
        {
            unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }
        AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        return activity;
    }
#endif

    private static string deviceId = null;
    public static string GetDeviceId()
    {
        if (deviceId == null && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass == null)
            {
                appAnalyticsClass = new AndroidJavaClass(APP_ANALYTICS_CLASS);
            }
            deviceId = appAnalyticsClass.CallStatic<string>("getDeviceId", GetCurrentActivity());
#endif
#if UNITY_IPHONE
            deviceId = TDAAGetDeviceId();
#endif
        }
        return deviceId;
    }

    private static string oaid = null;
    public static string GetOAID()
    {
        if (oaid == null && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass == null)
            {
                appAnalyticsClass = new AndroidJavaClass(APP_ANALYTICS_CLASS);
            }
            oaid = appAnalyticsClass.CallStatic<string>("getOAID", GetCurrentActivity());
#endif
        }
        return oaid;
    }

    public static void SetLogEnabled(bool enable)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass == null)
            {
                appAnalyticsClass = new AndroidJavaClass(APP_ANALYTICS_CLASS);
            }
            appAnalyticsClass.SetStatic("LOG_ON", enable);
#endif
#if UNITY_IPHONE
            TDAASetLogEnabled(enable);
#endif
        }
    }

    public static void BackgroundSessionEnabled()
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_IPHONE
            TDAABackgroundSessionEnabled();
#endif
        }
    }

    public static void SessionStarted(string appId, string channelId)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.Log("TalkingData App Analytics Unity SDK.");
#if UNITY_ANDROID
            using (AndroidJavaClass dz = new AndroidJavaClass("com.tendcloud.tenddata.dz"))
            {
                dz.SetStatic("a", 2);
            }
            if (appAnalyticsClass == null)
            {
                appAnalyticsClass = new AndroidJavaClass(APP_ANALYTICS_CLASS);
            }
            AndroidJavaObject activity = GetCurrentActivity();
            appAnalyticsClass.CallStatic("init", activity, appId, channelId);
            appAnalyticsClass.CallStatic("onResume", activity);
#endif
#if UNITY_IPHONE
            TDAASessionStarted(appId, channelId);
#endif
        }
    }

    public static void SessionStoped()
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onPause", GetCurrentActivity());
                appAnalyticsClass = null;
                unityPlayerClass = null;
            }
#endif
        }
    }

    public static void SetExceptionReportEnabled(bool enable)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("setReportUncaughtExceptions", enable);
            }
#endif
#if UNITY_IPHONE
            TDAASetExceptionReportEnabled(enable);
#endif
        }
    }

    public static void SetLocation(double latitude, double longitude)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_IPHONE
            TDAASetLocation(latitude, longitude);
#endif
        }
    }

    public static void OnRegister(string profileId, TalkingDataProfileType type, string name)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDProfile$ProfileType");
                AndroidJavaObject typeObj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
                appAnalyticsClass.CallStatic("onRegister", profileId, typeObj, name);
                enumClass.Dispose();
            }
#endif
#if UNITY_IPHONE
            TDAAOnRegister(profileId, (int)type, name);
#endif
        }
    }

    public static void OnLogin(string profileId, TalkingDataProfileType type, string name)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDProfile$ProfileType");
                AndroidJavaObject typeObj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
                appAnalyticsClass.CallStatic("onLogin", profileId, typeObj, name);
                enumClass.Dispose();
            }
#endif
#if UNITY_IPHONE
            TDAAOnLogin(profileId, (int)type, name);
#endif
        }
    }

#if TDAA_STANDARD
    public static void OnViewItem(string itemId, string category, string name, int unitPrice)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onViewItem", itemId, category, name, unitPrice);
            }
#endif
#if UNITY_IPHONE
            TDAAOnViewItem(itemId, category, name, unitPrice);
#endif
        }
    }

    public static void OnAddItemToShoppingCart(string itemId, string category, string name, int unitPrice, int amount)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onAddItemToShoppingCart", itemId, category, name, unitPrice, amount);
            }
#endif
#if UNITY_IPHONE
            TDAAOnAddItemToShoppingCart(itemId, category, name, unitPrice, amount);
#endif
        }
    }

    public static void OnViewShoppingCart(TalkingDataShoppingCart shoppingCart)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onViewShoppingCart", shoppingCart.javaObj);
            }
#endif
#if UNITY_IPHONE
            TDAAOnViewShoppingCart(shoppingCart.ToString());
#endif
        }
    }

    public static void OnPlaceOrder(string profileId, TalkingDataOrder order)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onPlaceOrder", profileId, order.javaObj);
            }
#endif
#if UNITY_IPHONE
            TDAAOnPlaceOrder(profileId, order.ToString());
#endif
        }
    }

    public static void OnOrderPaySucc(string profileId, string payType, TalkingDataOrder order)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onOrderPaySucc", profileId, payType, order.javaObj);
            }
#endif
#if UNITY_IPHONE
            TDAAOnOrderPaySucc(profileId, payType, order.ToString());
#endif
        }
    }
#endif

#if TDAA_CUSTOM
    public static void TrackEvent(string eventId)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onEvent", GetCurrentActivity(), eventId);
            }
#endif
#if UNITY_IPHONE
            TDAATrackEvent(eventId);
#endif
        }
    }

    public static void TrackEventWithLabel(string eventId, string eventLabel)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onEvent", GetCurrentActivity(), eventId, eventLabel);
            }
#endif
#if UNITY_IPHONE
            TDAATrackEventLabel(eventId, eventLabel);
#endif
        }
    }

    public static void TrackEventWithParameters(string eventId, string eventLabel, Dictionary<string, object> parameters)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                if (parameters != null && parameters.Count > 0)
                {
                    int count = parameters.Count;
                    AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap", count);
                    IntPtr method_Put = AndroidJNIHelper.GetMethodID(map.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
                    object[] args = new object[2];
                    foreach (KeyValuePair<string, object> kvp in parameters)
                    {
                        args[0] = new AndroidJavaObject("java.lang.String", kvp.Key);
                        args[1] = typeof(string).IsInstanceOfType(kvp.Value)
                            ? new AndroidJavaObject("java.lang.String", kvp.Value)
                            : new AndroidJavaObject("java.lang.Double", "" + kvp.Value);
                        AndroidJNI.CallObjectMethod(map.GetRawObject(), method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                    appAnalyticsClass.CallStatic("onEvent", GetCurrentActivity(), eventId, eventLabel, map);
                    map.Dispose();
                }
                else
                {
                    appAnalyticsClass.CallStatic("onEvent", GetCurrentActivity(), eventId, eventLabel, null);
                }
            }
#endif
#if UNITY_IPHONE
            if (parameters != null && parameters.Count > 0)
            {
                string parameterStr = "{";
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    if (kvp.Value is string)
                    {
                        parameterStr += "\"" + kvp.Key + "\":\"" + kvp.Value + "\",";
                    }
                    else
                    {
                        try
                        {
                            double tmp = System.Convert.ToDouble(kvp.Value);
                            parameterStr += "\"" + kvp.Key + "\":" + tmp + ",";
                        }
                        catch (System.Exception)
                        {
                        }
                    }
                }
                parameterStr = parameterStr.TrimEnd(',');
                parameterStr += "}";
                TDAATrackEventParameters(eventId, eventLabel, parameterStr);
            }
            else
            {
                TDAATrackEventParameters(eventId, eventLabel, null);
            }
#endif
        }
    }

    public static void TrackEventWithValue(string eventId, string eventLabel, Dictionary<string, object> parameters, double eventValue)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                if (parameters != null && parameters.Count > 0)
                {
                    int count = parameters.Count;
                    AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap", count);
                    IntPtr method_Put = AndroidJNIHelper.GetMethodID(map.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
                    object[] args = new object[2];
                    foreach (KeyValuePair<string, object> kvp in parameters)
                    {
                        args[0] = new AndroidJavaObject("java.lang.String", kvp.Key);
                        args[1] = typeof(string).IsInstanceOfType(kvp.Value)
                            ? new AndroidJavaObject("java.lang.String", kvp.Value)
                            : new AndroidJavaObject("java.lang.Double", "" + kvp.Value);
                        AndroidJNI.CallObjectMethod(map.GetRawObject(), method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                    appAnalyticsClass.CallStatic("onEvent", GetCurrentActivity(), eventId, eventLabel, map, eventValue);
                    map.Dispose();
                }
                else
                {
                    appAnalyticsClass.CallStatic("onEvent", GetCurrentActivity(), eventId, eventLabel, null, eventValue);
                }
            }
#endif
#if UNITY_IPHONE
            if (parameters != null && parameters.Count > 0)
            {
                string parameterStr = "{";
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    if (kvp.Value is string)
                    {
                        parameterStr += "\"" + kvp.Key + "\":\"" + kvp.Value + "\",";
                    }
                    else
                    {
                        try
                        {
                            double tmp = System.Convert.ToDouble(kvp.Value);
                            parameterStr += "\"" + kvp.Key + "\":" + tmp + ",";
                        }
                        catch (System.Exception)
                        {
                        }
                    }
                }
                parameterStr = parameterStr.TrimEnd(',');
                parameterStr += "}";
                TDAATrackEventValue(eventId, eventLabel, parameterStr, eventValue);
            }
            else
            {
                TDAATrackEventValue(eventId, eventLabel, null, eventValue);
            }
#endif
        }
    }
#endif

#if TDAA_PAGE
    public static void TrackPageBegin(string pageName)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onPageStart", GetCurrentActivity(), pageName);
            }
#endif
#if UNITY_IPHONE
            TDAATrackPageBegin(pageName);
#endif
        }
    }

    public static void TrackPageEnd(string pageName)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (appAnalyticsClass != null)
            {
                appAnalyticsClass.CallStatic("onPageEnd", GetCurrentActivity(), pageName);
            }
#endif
#if UNITY_IPHONE
            TDAATrackPageEnd(pageName);
#endif
        }
    }
#endif

#if TDAA_PUSH
    public static void SetDeviceToken()
    {
#if UNITY_IPHONE
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            if (!hasTokenBeenObtained)
            {
                byte[] deviceToken = UnityEngine.iOS.NotificationServices.deviceToken;
                if (deviceToken != null)
                {
                    TDAASetDeviceToken(deviceToken, deviceToken.Length);
                    hasTokenBeenObtained = true;
                }
            }
        }
#endif
    }

    public static void HandlePushMessage()
    {
#if UNITY_IPHONE
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            UnityEngine.iOS.RemoteNotification[] notifications = UnityEngine.iOS.NotificationServices.remoteNotifications;
            if (notifications != null)
            {
                UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
                foreach (UnityEngine.iOS.RemoteNotification rn in notifications)
                {
                    foreach (DictionaryEntry de in rn.userInfo)
                    {
                        if (de.Key.ToString().Equals("sign"))
                        {
                            string sign = de.Value.ToString();
                            TDAAHandlePushMessage(sign);
                        }
                    }
                }
            }
        }
#endif
    }
#endif
}
