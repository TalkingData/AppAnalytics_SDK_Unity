#import "TalkingData.h"

//#define TDAA_STANDARD   // 标准化事件
//#define TDAA_CUSTOM     // 自定义事件
//#define TDAA_PAGE       // 页面统计
//#define TDAA_PUSH       // 推送营销

// Converts C style string to NSString
static NSString *TDAACreateNSString(const char *string) {
    return string ? [NSString stringWithUTF8String:string] : nil;
}

static char *tdaaDeviceId = NULL;

extern "C" {
#pragma GCC diagnostic ignored "-Wmissing-prototypes"

const char *TDAAGetDeviceId() {
    if (!tdaaDeviceId) {
        NSString *deviceId = [TalkingData getDeviceID];
        tdaaDeviceId = (char *)calloc(deviceId.length + 1, sizeof(char));
        strcpy(tdaaDeviceId, deviceId.UTF8String);
    }
    return tdaaDeviceId;
}

void TDAASetLogEnabled(bool enable) {
    [TalkingData setLogEnabled:enable];
}

void TDAABackgroundSessionEnabled() {
     [TalkingData backgroundSessionEnabled];
}

void TDAASessionStarted(const char *appId, const char *channelId) {
    if ([TalkingData respondsToSelector:@selector(setFrameworkTag:)]) {
        [TalkingData performSelector:@selector(setFrameworkTag:) withObject:@2];
    }
    [TalkingData sessionStarted:TDAACreateNSString(appId) withChannelId:TDAACreateNSString(channelId)];
}

void TDAASetExceptionReportEnabled(bool enable) {
    [TalkingData setExceptionReportEnabled:enable];
}

void TDAASetLocation(double latitude, double longitude) {
    [TalkingData setLatitude:latitude longitude:longitude];
}

void TDAAOnRegister(const char *profileId, int type, const char *name) {
    [TalkingData onRegister:TDAACreateNSString(profileId) type:(TDProfileType)type name:TDAACreateNSString(name)];
}

void TDAAOnLogin(const char *profileId, int type, const char *name) {
    [TalkingData onLogin:TDAACreateNSString(profileId) type:(TDProfileType)type name:TDAACreateNSString(name)];
}

#ifdef TDAA_STANDARD
void TDAAOnViewItem(const char *itemId, const char *category, const char *name, int unitPrice) {
    [TalkingData onViewItem:TDAACreateNSString(itemId)
                   category:TDAACreateNSString(category)
                       name:TDAACreateNSString(name)
                  unitPrice:unitPrice];
}

void TDAAOnAddItemToShoppingCart(const char *itemId, const char *category, const char *name, int unitPrice, int amount) {
    [TalkingData onAddItemToShoppingCart:TDAACreateNSString(itemId)
                                category:TDAACreateNSString(category)
                                    name:TDAACreateNSString(name)
                               unitPrice:unitPrice
                                  amount:amount];
}

void TDAAOnViewShoppingCart(const char *shoppingCartJson) {
    NSString *shoppingCartStr = TDAACreateNSString(shoppingCartJson);
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

void TDAAOnPlaceOrder(const char *orderId, int amount, const char *currencyType) {
    [TalkingData onPlaceOrder:TDAACreateNSString(orderId) amount:amount currencyType:TDAACreateNSString(currencyType)];
}

void TDAAOnOrderPaySucc(const char *orderId, int amount, const char *currencyType, const char *paymentType) {
    [TalkingData onOrderPaySucc:TDAACreateNSString(orderId) amount:amount currencyType:TDAACreateNSString(currencyType) paymentType:TDAACreateNSString(paymentType)];
}

void TDAAOnCancelOrder(const char *orderId, int amount, const char *currencyType) {
    [TalkingData onCancelOrder:TDAACreateNSString(orderId) amount:amount currencyType:TDAACreateNSString(currencyType)];
}
#endif

#ifdef TDAA_CUSTOM
void TDAATrackEvent(const char *eventId) {
    [TalkingData trackEvent:TDAACreateNSString(eventId)];
}

void TDAATrackEventLabel(const char *eventId, const char *eventLabel) {
    [TalkingData trackEvent:TDAACreateNSString(eventId) label:TDAACreateNSString(eventLabel)];
}

void TDAATrackEventParameters(const char *eventId, const char *eventLabel, const char *parameters) {
    NSString *parameterStr = TDAACreateNSString(parameters);
    NSDictionary *parameterDic = nil;
    if (parameterStr) {
        NSData *parameterData = [parameterStr dataUsingEncoding:NSUTF8StringEncoding];
        parameterDic = [NSJSONSerialization JSONObjectWithData:parameterData options:0 error:nil];
    }
    [TalkingData trackEvent:TDAACreateNSString(eventId) label:TDAACreateNSString(eventLabel) parameters:parameterDic];
}

void TDAATrackEventValue(const char *eventId, const char *eventLabel, const char *parameters, double eventValue) {
    NSString *parameterStr = TDAACreateNSString(parameters);
    NSDictionary *parameterDic = nil;
    if (parameterStr) {
        NSData *parameterData = [parameterStr dataUsingEncoding:NSUTF8StringEncoding];
        parameterDic = [NSJSONSerialization JSONObjectWithData:parameterData options:0 error:nil];
    }
    [TalkingData trackEvent:TDAACreateNSString(eventId) label:TDAACreateNSString(eventLabel) parameters:parameterDic value:eventValue];
}
#endif

#ifdef TDAA_PAGE
void TDAATrackPageBegin(const char *pageName) {
    [TalkingData trackPageBegin:TDAACreateNSString(pageName)];
}

void TDAATrackPageEnd(const char *pageName) {
    [TalkingData trackPageEnd:TDAACreateNSString(pageName)];
}
#endif

#ifdef TDAA_PUSH
void TDAASetDeviceToken(const void *deviceToken, int length) {
    NSData *tokenData = [NSData dataWithBytes:deviceToken length:length];
    [TalkingData setDeviceToken:tokenData];
}

void TDAAHandlePushMessage(const char *message) {
    NSString *val = TDAACreateNSString(message);
    NSDictionary *dic = [NSDictionary dictionaryWithObject:val forKey:@"sign"];
    [TalkingData handlePushMessage:dic];
}
#endif

#pragma GCC diagnostic warning "-Wmissing-prototypes"
}
