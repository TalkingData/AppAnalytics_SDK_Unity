using UnityEngine;


#if TDAA_STANDARD
public class TalkingDataOrder
{
#if UNITY_ANDROID
    public AndroidJavaObject javaObj;
#endif

#if UNITY_IPHONE
    private string orderId;
    private int total;
    private string currencyType;
    private string items = "";
#endif

    public static TalkingDataOrder CreateOrder(string orderId, int total, string currencyType)
    {
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

    public TalkingDataOrder AddItem(string itemId, string category, string name, int unitPrice, int amount)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (javaObj != null)
            {
                javaObj.Call<AndroidJavaObject>("addItem", itemId, category, name, unitPrice, amount);
            }
#endif
#if UNITY_IPHONE
            string item = "{\"itemId\":\"" + itemId + "\",\"category\":\"" + category + "\",\"name\":\"" + name + "\",\"unitPrice\":" + unitPrice + ",\"amount\":" + amount + "}";
            if (items.Length > 0)
            {
                items += ",";
            }
            items += item;
#endif
        }
        return this;
    }

#if UNITY_IPHONE
    public override string ToString()
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            string orderStr = "{\"orderId\":\"" + orderId + "\",\"total\":" + total + ",\"currencyType\":\"" + currencyType + "\",\"items\":[" + items + "]}";
            return orderStr;
        }
        return null;
    }
#endif
}
#endif
