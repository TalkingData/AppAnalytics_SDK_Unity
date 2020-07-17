using UnityEngine;


#if TDAA_STANDARD
public class TalkingDataShoppingCart
{
#if UNITY_ANDROID
    public AndroidJavaObject javaObj;
#endif

#if UNITY_IPHONE
    private string items = "";
#endif

    public static TalkingDataShoppingCart CreateShoppingCart()
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TalkingDataShoppingCart shoppingCart = new TalkingDataShoppingCart();
#if UNITY_ANDROID
            AndroidJavaClass javaClass = new AndroidJavaClass("com.tendcloud.tenddata.ShoppingCart");
            shoppingCart.javaObj = javaClass.CallStatic<AndroidJavaObject>("createShoppingCart");
#endif
            return shoppingCart;
        }
        return null;
    }

    public TalkingDataShoppingCart AddItem(string itemId, string category, string name, int unitPrice, int amount)
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
            string orderStr = "{\"items\":[" + items + "]}";
            return orderStr;
        }
        return null;
    }
#endif
}
#endif
