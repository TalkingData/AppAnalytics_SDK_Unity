﻿using UnityEngine;
using System.Collections.Generic;

public class TDAADemoScript : MonoBehaviour
{
    private const int top = 100;
    private const int left = 80;
    private const int height = 50;
    private readonly int width = Screen.width - (left * 2);
    private const int step = 60;
    private string deviceId;
    private string oaid;

    private void OnGUI()
    {
        int i = 0;
        GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "Demo Menu");

        GUI.Label(new Rect(left, top + (step * i++), width, height), deviceId);
        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "getDeviceId"))
        {
            deviceId = TalkingDataPlugin.GetDeviceId();
        }

        GUI.Label(new Rect(left, top + (step * i++), width, height), oaid);
        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "getOAID"))
        {
            oaid = TalkingDataPlugin.GetOAID();
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "SetLocation"))
        {
            TalkingDataPlugin.SetLocation(39.94, 116.43);
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "OnRegister"))
        {
            TalkingDataPlugin.OnRegister("user01", TalkingDataAccountType.ANONYMOUS, "abc");
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "OnLogin"))
        {
            TalkingDataPlugin.OnLogin("user01", TalkingDataAccountType.TYPE1, "abc");
        }

#if TDAA_STANDARD
        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "OnViewItem"))
        {
            TalkingDataPlugin.OnViewItem("A1660", "手机", "iPhone 7", 538800);
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "OnAddItemToShoppingCart"))
        {
            TalkingDataPlugin.OnAddItemToShoppingCart("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
        }

        if (GUI.Button(new Rect(left, top + step * i++, width, height), "OnViewShoppingCart"))
        {
            TalkingDataShoppingCart shoppingCart = TalkingDataShoppingCart.CreateShoppingCart();
            if (shoppingCart != null)
            {
                shoppingCart.AddItem("A1660", "手机", "iPhone 7", 538800, 2);
                shoppingCart.AddItem("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
                TalkingDataPlugin.OnViewShoppingCart(shoppingCart);
            }
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "OnPlaceOrder"))
        {
            TalkingDataOrder order = TalkingDataOrder.CreateOrder("order01", 2466400, "CNY");
            order.AddItem("A1660", "手机", "iPhone 7", 538800, 2);
            order.AddItem("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
            TalkingDataPlugin.OnPlaceOrder("user01", order);
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "OnOrderPaySucc"))
        {
            TalkingDataOrder order = TalkingDataOrder.CreateOrder("order01", 2466400, "CNY");
            order.AddItem("A1660", "手机", "iPhone 7", 538800, 2);
            order.AddItem("MLH12CH", "电脑", "MacBook Pro", 1388800, 1);
            TalkingDataPlugin.OnOrderPaySucc("user01", "AliPay", order);
        }
#endif

#if TDAA_CUSTOM
        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "TrackEvent"))
        {
            TalkingDataPlugin.TrackEvent("action_id");
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "TrackEventWithLabel"))
        {
            TalkingDataPlugin.TrackEventWithLabel("action_id", "action_label");
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "TrackEventWithParameters"))
        {
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "StringValue", "Pi" },
                { "NumberValue", 3.14 }
            };
            TalkingDataPlugin.TrackEventWithParameters("action_id", "action_label", dic);
        }
#endif

#if TDAA_PAGE
        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "TrackPageBegin"))
        {
            TalkingDataPlugin.TrackPageBegin("page_name");
        }

        if (GUI.Button(new Rect(left, top + (step * i++), width, height), "TrackPageEnd"))
        {
            TalkingDataPlugin.TrackPageEnd("page_name");
        }
#endif
    }

    private void Start()
    {
        Debug.Log("Start");
        //TalkingDataPlugin.SetLogEnabled(false);
        TalkingDataPlugin.BackgroundSessionEnabled();
        TalkingDataPlugin.SessionStarted("your_app_id", "your_channel_id");
        TalkingDataPlugin.SetExceptionReportEnabled(true);
#if TDAA_PUSH
#if UNITY_IPHONE
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(
            UnityEngine.iOS.NotificationType.Alert |
            UnityEngine.iOS.NotificationType.Badge |
            UnityEngine.iOS.NotificationType.Sound);
#endif
#endif
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
#if TDAA_PUSH
        TalkingDataPlugin.SetDeviceToken();
        TalkingDataPlugin.HandlePushMessage();
#endif
    }

    private void OnDestroy()
    {
        Debug.Log("onDestroy");
        TalkingDataPlugin.SessionStoped();
    }

    private void Awake()
    {
        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
}