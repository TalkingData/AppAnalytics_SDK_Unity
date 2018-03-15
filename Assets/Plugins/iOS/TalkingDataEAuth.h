//
//  TalkingDataEAuth.h
//  TalkingDataSDK
//
//  Created by Robin on 7/13/16.
//  Copyright © 2016 TendCloud. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, TDEAuthType) {
    TDEAuthTypeApplyCode = 0,   // 申请认证码
    TDEAuthTypeChecker,         // 检查账号是否已认证
    TDEAuthTypePhoneMatch,      // 检查账号与手机号是否匹配
    TDEAuthTypeBind,            // 账号认证绑定
    TDEAuthTypeUnBind           // 账号认证解绑定
};

typedef NS_ENUM(NSInteger, TDAuthCodeType) {
    TDAuthCodeTypeSMS = 0,      // 短信认证
    TDAuthCodeTypeVoice         // 语音认证
};


// Delegate回调是在您的主线程中
@protocol TalkingDataEAuthDelegate <NSObject>

- (void)onRequestSuccess:(TDEAuthType)type requestId:(NSString *)requestId phoneNumber:(NSString *)phoneNumber phoneNumSeg:(NSArray *)phoneNumSeg;
- (void)onRequestFailed:(TDEAuthType)type errorCode:(NSInteger)errorCode errorMessage:(NSString *)errorMessage;

@optional
- (void)onRequestSuccess:(TDEAuthType)type;

@end


@interface TalkingDataEAuth : NSObject

/**
 * 易认证初始化
 *
 * @param appID
 *            TalkingData 分配的 AppID
 * @param secretID
 *            TalkingData 分配的 SecretID
 */
+ (void)initEAuth:(NSString *)appID secretId:(NSString *)secretID;

/**
 *  设置日志输出状态
 *
 *	@param enable
 *             日志输出状态
 */
+ (void)setLogEnabled:(BOOL)enable;

/**
 *  获取SDK所使用的DeviceID
 *
 *  @return DeviceID
 */
+ (NSString *)getDeviceId;

/**
 * 申请认证码
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            申请验证码的手机号
 * @param type
 *            获取认证码的方式
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)applyAuthCode:(NSString *)countryCode
               mobile:(NSString *)mobile
         authCodeType:(TDAuthCodeType)type
          accountName:(NSString *)acctName
             delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 重新发送认证码
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            申请验证码的手机号
 * @param type
 *            获取认证码的方式
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)reapplyAuthCode:(NSString *)countryCode
                 mobile:(NSString *)mobile
           authCodeType:(TDAuthCodeType)type
            accountName:(NSString *)acctName
              requestId:(NSString *)requestId
               delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 申请短信认证码
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            申请验证码的手机号
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)applyAuthCode:(NSString *)countryCode
               mobile:(NSString *)mobile
          accountName:(NSString *)acctName
             delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 检查账号是否已认证
 *
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)isVerifyAccount:(NSString *)acctName
               delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 检查手机号和账号是否匹配
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            申请验证码的手机号
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)isMobileMatchAccount:(NSString *)acctName
                 countryCode:(NSString *)countryCode
                      mobile:(NSString *)mobile
                    delegate:(id<TalkingDataEAuthDelegate>)delegate;


/**
 * 进行实名认证绑定
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            验证手机号
 * @param authCode
 *            短信认证码
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            绑定请求的异步回调接口
 */
+ (void)bindEAuth:(NSString *)countryCode
           mobile:(NSString *)mobile
         authCode:(NSString *)authCode
      accountName:(NSString *)acctName
         delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 解除实名认证绑定
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            验证手机号
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            解绑请求的异步回调接口
 */
+ (void)unbindEAuth:(NSString *)countryCode
             mobile:(NSString *)mobile
        accountName:(NSString *)acctName
           delegate:(id<TalkingDataEAuthDelegate>)delegate;

@end
