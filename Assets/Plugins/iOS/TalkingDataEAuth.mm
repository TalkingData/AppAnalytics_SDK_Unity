//
//  TalkingDataEAuth.mm
//  TalkingData
//
//  Created by liweiqiang on 15/12/2.
//  Copyright © 2015年 TendCloud. All rights reserved.
//

#include "TalkingDataEAuth.h"

// Converts C style string to NSString
static NSString *tdEAuthCreateNSString(const char *string) {
    if (string)
        return [NSString stringWithUTF8String:string];
    else
        return [NSString stringWithUTF8String:""];
}


@interface TalkingDataEAuthCallback : NSObject <TalkingDataEAuthDelegate>
@end

static TalkingDataEAuthCallback *eAuthCallback;

extern "C" {
    
    extern void UnitySendMessage(const char* obj, const char* method, const char* msg);
    
#pragma GCC diagnostic ignored "-Wmissing-prototypes"
    
    void tdEAuthInit(const char *appId, const char *secretId) {
        if (eAuthCallback == nil) {
            eAuthCallback = [[TalkingDataEAuthCallback alloc] init];
        }
        [TalkingDataEAuth initEAuth:tdEAuthCreateNSString(appId)
                           secretId:tdEAuthCreateNSString(secretId)];
    }
    
    void tdEAuthApplyAuthCode(const char *countryCode, const char *mobile, TDAuthCodeType type, const char *accountName, const char *smsId) {
        [TalkingDataEAuth applyAuthCode:tdEAuthCreateNSString(countryCode)
                                 mobile:tdEAuthCreateNSString(mobile)
                           authCodeType:type
                            accountName:tdEAuthCreateNSString(accountName)
                                  smsId:tdEAuthCreateNSString(smsId)
                               delegate:eAuthCallback];
    }
    
    void tdEAuthReapplyAuthCode(const char *countryCode, const char *mobile, TDAuthCodeType type, const char *accountName, const char *smsId, const char *requestId) {
        [TalkingDataEAuth reapplyAuthCode:tdEAuthCreateNSString(countryCode)
                                   mobile:tdEAuthCreateNSString(mobile)
                             authCodeType:type
                              accountName:tdEAuthCreateNSString(accountName)
                                    smsId:tdEAuthCreateNSString(smsId)
                                requestId:tdEAuthCreateNSString(requestId)
                                 delegate:eAuthCallback];
    }
    
    void tdEAuthIsVerifyAccount(const char *accountName) {
        [TalkingDataEAuth isVerifyAccount:tdEAuthCreateNSString(accountName)
                                 delegate:eAuthCallback];
    }
    
    void tdEAuthIsMobileMatchAccount(const char *accountName, const char *countryCode, const char *mobile) {
        [TalkingDataEAuth isMobileMatchAccount:tdEAuthCreateNSString(accountName)
                                   countryCode:tdEAuthCreateNSString(countryCode)
                                        mobile:tdEAuthCreateNSString(mobile)
                                      delegate:eAuthCallback];
    }
    
    void tdEAuthBind(const char *countryCode, const char *mobile, const char *authCode, const char *accountName) {
        [TalkingDataEAuth bindEAuth:tdEAuthCreateNSString(countryCode)
                             mobile:tdEAuthCreateNSString(mobile)
                           authCode:tdEAuthCreateNSString(authCode)
                        accountName:tdEAuthCreateNSString(accountName)
                           delegate:eAuthCallback];
    }
    
    void tdEAuthUnbind(const char *countryCode, const char *mobile, const char *accountName) {
        [TalkingDataEAuth unbindEAuth:tdEAuthCreateNSString(countryCode)
                               mobile:tdEAuthCreateNSString(mobile)
                          accountName:tdEAuthCreateNSString(accountName)
                             delegate:eAuthCallback];
    }
    
#pragma GCC diagnostic warning "-Wmissing-prototypes"
    
}


@implementation TalkingDataEAuthCallback

- (void)onRequestSuccess:(TDEAuthType)type requestId:(NSString *)requestId phoneNumber:(NSString *)phoneNumber phoneNumSeg:(NSArray *)phoneNumSeg {
    NSMutableDictionary *args = [NSMutableDictionary dictionary];
    [args setObject:[NSNumber numberWithInteger:type] forKey:@"type"];
    [args setObject:requestId ?: @"" forKey:@"requestId"];
    [args setObject:phoneNumber ?: @"" forKey:@"phoneNumber"];
    [args setObject:phoneNumSeg ?: @[] forKey:@"phoneNumSeg"];
    NSData *argData = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    NSString *argJson = [[NSString alloc] initWithData:argData encoding:NSUTF8StringEncoding];
    UnitySendMessage("TalkingDataEAuth", "OnRequestSuccess", [argJson UTF8String]);
}

- (void)onRequestFailed:(TDEAuthType)type errorCode:(NSInteger)errorCode errorMessage:(NSString *)errorMessage {
    NSMutableDictionary *args = [NSMutableDictionary dictionary];
    [args setObject:[NSNumber numberWithInteger:type] forKey:@"type"];
    [args setObject:[NSNumber numberWithInteger:errorCode] forKey:@"errorCode"];
    [args setObject:errorMessage ?: @"" forKey:@"errorMessage"];
    NSData *argData = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    NSString *argJson = [[NSString alloc] initWithData:argData encoding:NSUTF8StringEncoding];
    UnitySendMessage("TalkingDataEAuth", "OnRequestFailed", [argJson UTF8String]);
}

@end
