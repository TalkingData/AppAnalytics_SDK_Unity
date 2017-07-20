//
//  TalkingDataSMS.mm
//  TalkingData
//
//  Created by liweiqiang on 15/12/2.
//  Copyright © 2015年 TendCloud. All rights reserved.
//

#include "TalkingDataSMS.h"

// Converts C style string to NSString
static NSString *tdSmsCreateNSString(const char *string) {
    if (string)
        return [NSString stringWithUTF8String:string];
    else
        return [NSString stringWithUTF8String:""];
}


@interface TalkingDataSMSCallback : NSObject <TalkingDataSMSDelegate>
@end

static TalkingDataSMSCallback *smsCallback;

extern "C" {
    
    extern void UnitySendMessage(const char* obj, const char* method, const char* msg);
    
#pragma GCC diagnostic ignored "-Wmissing-prototypes"
    
    void tdSmsInit(const char *appKey, const char *secretId) {
        if (smsCallback == nil) {
            smsCallback = [[TalkingDataSMSCallback alloc] init];
        }
        [TalkingDataSMS init:tdSmsCreateNSString(appKey)
                withSecretId:tdSmsCreateNSString(secretId)];
    }
    
    void tdSmsApplyAuthCode(const char *countryCode, const char *mobile) {
        [TalkingDataSMS applyAuthCode:tdSmsCreateNSString(countryCode)
                               mobile:tdSmsCreateNSString(mobile)
                             delegate:smsCallback];
    }
    
    void tdSmsReapplyAuthCode(const char *countryCode, const char *mobile, const char *requestId) {
        [TalkingDataSMS reapplyAuthCode:tdSmsCreateNSString(countryCode)
                                 mobile:tdSmsCreateNSString(mobile)
                              requestId:tdSmsCreateNSString(requestId)
                               delegate:smsCallback];
    }
    
    void tdSmsVerifyAuthCode(const char *countryCode, const char *mobile, const char *authCode) {
        [TalkingDataSMS verifyAuthCode:tdSmsCreateNSString(countryCode)
                                mobile:tdSmsCreateNSString(mobile)
                              authCode:tdSmsCreateNSString(authCode)
                              delegate:smsCallback];
    }
    
#pragma GCC diagnostic warning "-Wmissing-prototypes"
    
}


@implementation TalkingDataSMSCallback

- (void)onApplySucc:(NSString *)requestId {
    UnitySendMessage("TalkingDataSMSPlugin", "OnApplySucc", [requestId UTF8String]);
}

- (void)onApplyFailed:(int)errorCode errorMessage:(NSString *)errorMessage {
    NSArray *args = [NSArray arrayWithObjects:[NSNumber numberWithInt:errorCode], errorMessage, nil];
    NSData *argData = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    NSString *argJson = [[NSString alloc] initWithData:argData encoding:NSUTF8StringEncoding];
    UnitySendMessage("TalkingDataSMSPlugin", "OnApplyFailed", [argJson UTF8String]);
}

- (void)onVerifySucc:(NSString *)requestId {
    UnitySendMessage("TalkingDataSMSPlugin", "OnVerifySucc", [requestId UTF8String]);
}

- (void)onVerifyFailed:(int)errorCode errorMessage:(NSString *)errorMessage {
    NSArray *args = [NSArray arrayWithObjects:[NSNumber numberWithInt:errorCode], errorMessage, nil];
    NSData *argData = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    NSString *argJson = [[NSString alloc] initWithData:argData encoding:NSUTF8StringEncoding];
    UnitySendMessage("TalkingDataSMSPlugin", "OnVerifyFailed", [argJson UTF8String]);
}

@end
