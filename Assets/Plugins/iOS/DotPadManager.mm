//
//  DotPadManager.m
//  Unity-iPhone
//
//  Created by coy on 9/6/24.
//

#import <Foundation/Foundation.h>
#import "DotPadFrameworks/DotPadFrameworks-swift.h"
#import "UnityAppController+UnityInterface.h"
#import "Unity-iPhone-Bridging-Header.h"
#import <UnityFramework/UnityFramework-Swift.h>
#import <CoreBluetooth/CoreBluetooth.h>
//#import <AudioToolbox/AudioToolbox.h>

extern "C" {


/*void iOSHapticFeedback() {
    if (@available(iOS 10.0, *)) {
        UIImpactFeedbackGenerator *generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
        [generator prepare];
        [generator impactOccurred];
    } else {
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate); // iOS 9 이하에서 기본 진동
    }
}*/

void _dotpadinit(){
    [[DotPadSDK shared] dotpadinit];
}
void _connectToDotPad(int index) {
    [[DotPadSDK shared] connectToDotPadWithIndex:index];
}

void _disconnectFromDotPad() {
    [[DotPadSDK shared] disconnectFromDotPad];
}

void _displayGraphicData(const char* data) {
    NSString *graphicData = [NSString stringWithUTF8String:data];
    [[DotPadSDK shared] displayGraphicDataWithData:graphicData];
}

void _displayTextData(const char* text) {
    NSString *textData = [NSString stringWithUTF8String:text];
    [[DotPadSDK shared] displayTextDataWithText:textData];
}

void _scanForDotPad() {
    [[DotPadSDK shared] scanForDotPad];
}

void _stopScanForDotPad(){
    [[DotPadSDK shared] stopScanForDotPad];
}

void _sendImageToSwift(const char* data){//const char* base64Image) {
    // NSString *imageString = [NSString stringWithUTF8String:base64Image];
     // Swift 메서드 호출
   // [[DotPadSDK shared] sendImageToSwiftWithBase64Image:imageString];
    NSString *graphicData = [NSString stringWithUTF8String:data];
      [[DotPadSDK shared] displayGraphicDataWithData:graphicData];
 }

void _checkBluetoothAndOpenSettingsIfDisabled() {
    /*  CBCentralManager *bluetoothManager = [[CBCentralManager alloc] init];
     CBManagerState bluetoothState = [bluetoothManager state];
     
     if (bluetoothState == CBManagerStatePoweredOn) {
     return true; // 블루투스가 활성화 상태면 true 반환
     } else {
     // 블루투스가 비활성화 상태이면 설정 화면으로 이동
     NSURL *url = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
     if ([[UIApplication sharedApplication] canOpenURL:url]) {
     [[UIApplication sharedApplication] openURL:url
     options:@{}
     completionHandler:nil];
     }
     return false; // 블루투스가 비활성화 상태이면 false 반환
     }*/
    NSURL *url = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
    if ([[UIApplication sharedApplication] canOpenURL:url]) {
        [[UIApplication sharedApplication] openURL:url options:@{} completionHandler:nil];
    }
}

}
