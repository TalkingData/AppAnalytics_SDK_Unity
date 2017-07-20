//
//  TalkingData.mm
//  TalkingData
//
//  Created by Biao Hou on 12-6-21.
//  Copyright (c) 2012年 TendCloud. All rights reserved.
//

#import "TalkingData.h"

// Converts C style string to NSString
static NSString *tdCreateNSString(const char *string) {
	if (string)
		return [NSString stringWithUTF8String:string];
	else
		return [NSString stringWithUTF8String:""];
}

extern "C" {

#pragma GCC diagnostic ignored "-Wmissing-prototypes"
    
    void tdSetSDKFramework(int tag) {
        [TalkingData setSDKFramework:tag];
    }
    
    void tdSessionStarted(const char *appKey, const char *channelId) {
        [TalkingData sessionStarted:tdCreateNSString(appKey)
                      withChannelId:tdCreateNSString(channelId)];
    }
    
    void tdSetAntiCheatingEnabled(bool enable) {
        [TalkingData setAntiCheatingEnabled:enable];
    }
    
    void tdSetExceptionReportEnabled(bool enable) {
        [TalkingData setExceptionReportEnabled:enable];
    }
    
    void tdSetLatitude(double latitude, double longitude) {
        [TalkingData setLatitude:latitude longitude:longitude];
    }
    
    void tdSetLogEnabled(bool enable) {
        [TalkingData setLogEnabled:enable];
    }
    
    void tdOnRegister(const char *accountId, int type, const char *name) {
        [TalkingData onRegister:tdCreateNSString(accountId)
                           type:(TDAccountType)type
                           name:tdCreateNSString(name)];
    }
    
    void tdOnLogin(const char *accountId, int type, const char *name) {
        [TalkingData onLogin:tdCreateNSString(accountId)
                        type:(TDAccountType)type
                        name:tdCreateNSString(name)];
    }
    
    void tdTrackEvent(const char *eventId) {
        [TalkingData trackEvent:tdCreateNSString(eventId)];
    }
    
    void tdTrackEventLabel(const char *eventId, const char *eventLabel) {
        [TalkingData trackEvent:tdCreateNSString(eventId)
                          label:tdCreateNSString(eventLabel)];
    }
    
    void tdTrackEventParameters(const char *eventId, const char *eventLabel,
                                 const char *keys[], const char *stringValues[],
                                 double numberValues[], int count) {
        NSMutableDictionary *dic = [NSMutableDictionary dictionary];
        for (int i = 0; i < count; i++) {
            if (keys[i] != NULL) {
                if (stringValues[i] != NULL) {
                    [dic setObject:tdCreateNSString(stringValues[i]) 
                            forKey:tdCreateNSString(keys[i])];
                } else {
                    [dic setObject:[NSNumber numberWithDouble:numberValues[i]] 
                            forKey:tdCreateNSString(keys[i])];
                }
            }
        }
        
        [TalkingData trackEvent:tdCreateNSString(eventId) 
                          label:tdCreateNSString(eventLabel) 
                     parameters:dic];        
    }
    
    void tdOnPlaceOrder(const char *accountId, const char *orderJson) {
        NSString *orderStr = tdCreateNSString(orderJson);
        NSData *orderData = [orderStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *orderDic = [NSJSONSerialization JSONObjectWithData:orderData options:0 error:nil];
        TalkingDataOrder *order = [TalkingDataOrder createOrder:orderDic[@"orderId"]
                                                          total:[orderDic[@"total"] intValue]
                                                   currencyType:orderDic[@"currencyType"]];
        NSArray *items = orderDic[@"items"];
        for (NSDictionary *item in items) {
            [order addItem:item[@"itemId"]
                  category:item[@"category"]
                      name:item[@"name"]
                 unitPrice:[item[@"unitPrice"] intValue]
                    amount:[item[@"amount"] intValue]];
        }
        [TalkingData onPlaceOrder:tdCreateNSString(accountId)
                            order:order];
    }
    
    void tdOnOrderPaySucc(const char *accountId, const char *payType, const char *orderJson) {
        NSString *orderStr = tdCreateNSString(orderJson);
        NSData *orderData = [orderStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *orderDic = [NSJSONSerialization JSONObjectWithData:orderData options:0 error:nil];
        TalkingDataOrder *order = [TalkingDataOrder createOrder:orderDic[@"orderId"]
                                                          total:[orderDic[@"total"] intValue]
                                                   currencyType:orderDic[@"currencyType"]];
        NSArray *items = orderDic[@"items"];
        for (NSDictionary *item in items) {
            [order addItem:item[@"itemId"]
                  category:item[@"category"]
                      name:item[@"name"]
                 unitPrice:[item[@"unitPrice"] intValue]
                    amount:[item[@"amount"] intValue]];
        }
        [TalkingData onOrderPaySucc:tdCreateNSString(accountId)
                            payType:tdCreateNSString(payType)
                              order:order];
    }
    
    void tdOnViewItem(const char *itemId, const char *category, const char *name, int unitPrice) {
        [TalkingData onViewItem:tdCreateNSString(itemId)
                       category:tdCreateNSString(category)
                           name:tdCreateNSString(name)
                      unitPrice:unitPrice];
    }
    
    void tdOnAddItemToShoppingCart(const char *itemId, const char *category, const char *name, int unitPrice, int amount) {
        [TalkingData onAddItemToShoppingCart:tdCreateNSString(itemId)
                                    category:tdCreateNSString(category)
                                        name:tdCreateNSString(name)
                                   unitPrice:unitPrice
                                      amount:amount];
    }
    
    void tdOnViewShoppingCart(const char *shoppingCartJson) {
        NSString *shoppingCartStr = tdCreateNSString(shoppingCartJson);
        NSData *shoppingCartData = [shoppingCartStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *shoppingCartDic = [NSJSONSerialization JSONObjectWithData:shoppingCartData options:0 error:nil];
        TalkingDataShoppingCart *shoppingCart = [TalkingDataShoppingCart createShoppingCart];
        NSArray *items = shoppingCartDic[@"items"];
        for (NSDictionary *item in items) {
            [shoppingCart addItem:item[@"itemId"]
                         category:item[@"category"]
                             name:item[@"name"]
                        unitPrice:[item[@"unitPrice"] intValue]
                           amount:[item[@"amount"] intValue]];
        }
        [TalkingData onViewShoppingCart:shoppingCart];
    }
    
    void tdTrackPageBegin(const char *pageName) {
        [TalkingData trackPageBegin:tdCreateNSString(pageName)];
    }
    
    void tdTrackPageEnd(const char *pageName) {
        [TalkingData trackPageEnd:tdCreateNSString(pageName)];
    }
    
    void tdSetDeviceToken(const char *deviceToken) {
        NSString *token = tdCreateNSString(deviceToken);
        [TalkingData setDeviceToken:(id)token];
    }
    
    void tdHandlePushMessage(const char *message) {
        NSString *val = tdCreateNSString(message);
        NSDictionary *dic = [NSDictionary dictionaryWithObject:val forKey:@"sign"];
        [TalkingData handlePushMessage:dic];
    }
    
#pragma GCC diagnostic warning "-Wmissing-prototypes"
    
}

