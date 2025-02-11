//
//  DotPadSDK.swift
//  Unity-iPhone
//
//  Created by coy on 9/6/24.
//


import Foundation
import DotPadFrameworks
import UIKit
import Accessibility

@objc public class DotPadSDK:NSObject,DotPadFrameworks.SendDataProtocol {
    
    @objc public static let shared = DotPadSDK()
    let dotPadAPI: DotPadFrameworks.DotPadAPI = DotPadFrameworks.DotPadAPI()
    
    public var deviceName: String = ""
    public var dotPad: DotPad = DotPad(deviceType: DeviceType.DotPad320)
    
    var isGraphicDisplay: Bool = false
    var isBrailleDisplay: Bool = false
    
    public var discoveries = [BKDiscovery]() {
        didSet {
           // tableView.reloadData()
        }
    }
    
    public func setDeviceType(_ deviceType: DeviceType) {
      switch (deviceType) {
      case .DotPad320:
          print("[DotPadSDK] setDeviceType : \(deviceType)")
          dotPad = DotPad(deviceType: DeviceType.DotPad320)
      case .DotPad832:
          print("[DotPadSDK] setDeviceType : \(deviceType)")
      case .Default:
          print("[DotPadSDK] setDeviceType : Default")
      case .DotPad300:
          print("[DotPadSDK] setDeviceType : \(deviceType)")
          dotPad = DotPad(deviceType: DeviceType.DotPad300)
      @unknown default:
          print("[DotPadSDK] setDeviceType : \(deviceType)")
          dotPad = DotPad(deviceType: DeviceType.DotPad320)
      }
      
      dotPad = DotPad(deviceType: DeviceType.DotPad320)
  }
    
    private override init() {
        super.init()
    }
    
    @objc public func dotpadinit(){
        DotPadSDK.shared.dotPadAPI.dotPadCommunication.delegate_SDP = self
    }
    
    @objc public func scanForDotPad() {
        DotPadSDK.shared.dotPadAPI.dotPadCommunication.scan()
        
        /*if (DotPadSDK.shared.dotPadAPI.dotPadCommunication.isConnect() == false) {
         //LogMessage("Start Scan")
         DotPadSDK.shared.dotPadAPI.dotPadCommunication.scan()
         }
         else if (DotPadSDK.shared.dotPadAPI.dotPadCommunication.isConnect() == true) {
         //LogMessage("Disconnect")
         DotPadSDK.shared.dotPadAPI.dotPadCommunication.disconnect()
         }*/
 
    }

    @objc public func stopScanForDotPad(){
        DotPadSDK.shared.dotPadAPI.dotPadCommunication.stopScan()
    }
    
    @objc public func connectToDotPad(index: Int ) { //, indexPath : IndexPath
        if index < DotPadSDK.shared.dotPadAPI.dotPadCommunication.discoveries_filtered.count {
            let discovery = DotPadSDK.shared.dotPadAPI.dotPadCommunication.discoveries_filtered[index]
            DotPadSDK.shared.dotPadAPI.dotPadCommunication.connect(discovery.remotePeripheral)
            
            
       //     DotPadSDK.shared.dotPadAPI.dotPadCommunication.connect(discoveries[indexPath.row].remotePeripheral)
        }
       }
    
    @objc public func disconnectFromDotPad() {
        DotPadSDK.shared.dotPadAPI.dotPadCommunication.disconnect()
    }
    
    @objc public func displayGraphicData(data: String) {
        //DotPadSDK.shared.dotPadAPI.dotPadProcessData.displayGraphicData(data: data)
        
        if isGraphicDisplay {
         /*   let graphicDataStr =  "000000000000000000000000000000000000000000000000000000000000000000000000008888888888882446888888888888000000000000000000000000000000f0a622222244440ce84644222222e10f0000000000000000000000000000f0f088888888f70fa10488888888780f000000000000000000000000000010212222e2fff0100100e1a622221200000000000000000000000000000000000080e7310c0f000030fc000000000000000000000000000000000000000000f0f000f10f000000690f0000000000000000000000000000000000000000102133233333333312010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"*/
            /*"0000000000000000000000000000000000000000000000000000000000000000000000CCEE0E00000000000000000000000000000000000000000000000000000010FF0F00000000000000000000000000000000880800808800000000888888FF0F0000000000000000C0CC0000000000003303003033000080FE7F33F7FF0F00C0FE7FF7EF0C00F7FF77770000000088080080880000F0FF000000FF0F00FF1F0000F1FF00F0FF00000000000033030030330000F0FF8C00C8FF0F00F7CF0880FC7F00F0FF080000000000880800808800000073777777777707103377773301003077777700000000330300303300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"
            */
            DotPadSDK.shared.dotPadAPI.dotPadProcessData.displayGraphicData(data: data)
        
        }
    }
  
    
    @objc public func displayTextData(text: String) {
        let result =
        DotPadSDK.shared.dotPadAPI.dotPadProcessData.displayTextData(text: text)
    }
    
    
    @objc public func getDeviceInfo(_ sender: Any) {
       // LogMessage()
        DotPadSDK.shared.dotPadAPI.dotPadProcessData.requestDeviceName()
    }
    
     public func sendDataFunc(_ dataCode: DotPadFrameworks.DotPadCommunication.DataCodes, _ dataStr: String) {
        
        switch dataCode {
        case DotPadFrameworks.DotPadCommunication.DataCodes.Discovery_List:
           // padInfoTextField.text = ""
            discoveries = DotPadSDK.shared.dotPadAPI.dotPadCommunication.discoveries_filtered
            print("스캔된 장치 목록: \(dataStr)")
            print("스캔된 장치 목록: \(dataCode)")
            print("스캔된 장치 목록: \(discoveries)")
            /* let discoveriesString = discoveries.map { "\($0)" }.joined(separator: ", ")
                     
                     // Unity의 특정 GameObject에 데이터 전송
                     UnityFramework().sendMessageToGO(withName: "DotPadManager", functionName: "ReceiveDeviceList", message: "스캔된 장치 목록: \(discoveriesString)")*/
          // 인덱스 번호와 discoveries 정보를 함께 생성 (유효한 장치 이름만 포함)
                let discoveriesString = discoveries.enumerated().compactMap { (index, discovery) -> String? in
                    // kCBAdvDataLocalName이 있는 경우에만 인덱스와 함께 문자열을 생성
                    if let localName = discovery.advertisementData["kCBAdvDataLocalName"] as? String {
                        return  "\(index),\(localName)"
                    }
                    return nil // 유효하지 않은 경우 nil 반환
                }.joined(separator: ", ")

                // Unity의 특정 GameObject에 데이터 전송
                UnityFramework().sendMessageToGO(withName: "DotPadManager", functionName: "ReceiveDeviceList", message: "\(discoveriesString)")
                
        case DotPadFrameworks.DotPadCommunication.DataCodes.Connected:
         //   BLEButton.setTitle("Disconnect", for: .normal)
            do { sleep(3) }
            getDeviceInfo(self)
            print("디바이스 연결됨: \(dataStr)")
            print("디바이스 연결됨: \(dataCode)")
        case DotPadFrameworks.DotPadCommunication.DataCodes.Disconnected:
          //  padInfoTextField.text = ""
            discoveries.removeAll()
          //  BLEButton.setTitle("Start Scan", for: .normal)
        case DotPadFrameworks.DotPadCommunication.DataCodes.DeviceName:
            print("디바이스 이름: \(dataStr)")
            print("디바이스 이름: \(dataCode)")
            let deviceName = dataStr
            if deviceName.contains("KM2-20") {
                isGraphicDisplay = false
                isBrailleDisplay = true
            } else if deviceName.contains("DotPad320") {
                isGraphicDisplay = true
                isBrailleDisplay = true
            }else if deviceName.contains("300") {
                isGraphicDisplay = true
                isBrailleDisplay = false
                UnityFramework().sendMessageToGO(withName: "DotPadManager", functionName: "ConnectDotInfo", message: "\(1)")
            }
         //   padInfoTextField.text = dataStr
        case DotPadFrameworks.DotPadCommunication.DataCodes.KeyFunction1:
            //LogMessage("Key Function1 from framework")
            let dotDataBoolean: Array<Array<Bool>> = DotPadSDK.shared.dotPadAPI.dotPadProcessData.getDotDataBooleanCurrentDTMSItem()
            //LogMessage(dotDataBoolean)
            let itemInfo: (Int, Int) = DotPadSDK.shared.dotPadAPI.dotPadProcessData.getDTMSItemIdxInfo()
           // LogMessage(itemInfo)
        case DotPadFrameworks.DotPadCommunication.DataCodes.KeyFunction2:
            //LogMessage("Key Function2 from framework")
            let dotDataBoolean: Array<Array<Bool>> = DotPadSDK.shared.dotPadAPI.dotPadProcessData.getDotDataBooleanCurrentDTMSItem()
            //LogMessage(dotDataBoolean)
            let itemInfo: (Int, Int) = DotPadSDK.shared.dotPadAPI.dotPadProcessData.getDTMSItemIdxInfo()
            //LogMessage(itemInfo)
        default:
            break
        }
    }
    
   
    
    private func charPairToByte(_ str: String) -> UInt8{
        var byte:UInt8 = 0
        
        for c in str {
            var number:UInt8 = 0
            byte = byte << 4
            number = UInt8(UInt(String(c), radix: 16) ?? 0 )
            byte = byte | number
        }
        return byte
    }
    
    private func hexStringToByteArray(_ str: String) -> Array<UInt8> {
        var hexString = ""
        
        for character in str {
            if (character != " ") {
                hexString.append(character)
            }
        }
        
        hexString = hexString.uppercased()
        
        var bytes = [UInt8]()
        var stringLength = hexString.count
        
        if (stringLength % 2 != 0) {
            stringLength -= 1;
        }
        
        for i in (0..<stringLength/2) {
            let sub = hexString.substring(with: (hexString.index(hexString.startIndex, offsetBy: i * 2) ..< hexString.index(hexString.startIndex, offsetBy: (i * 2) + 2)))
            let byte:UInt8 = charPairToByte(sub)
            
            bytes.append(byte)
        }
        return bytes
    }
    @objc public func sendImageToSwift(data: String){//base64Image: String) {
        // Base64 인코딩된 이미지를 처리하는 로직
        /*if let imageData = Data(base64Encoded: base64Image) {
            if let image = UIImage(data: imageData) {
                // UIImage를 다시 Data로 변환하여 Base64 문자열로 변환
                if let pngData = image.pngData() {
                    let graphicDataStr = pngData.base64EncodedString()
                    
                    // DotPadSDK에 이미지 데이터 전송 (Base64 String 형태로)
                    DotPadSDK.shared.dotPadAPI.dotPadProcessData.displayGraphicData(data: graphicDataStr)
                } else {
                    print("PNG 데이터로 변환할 수 없습니다.")
                }
            } else {
                print("UIImage로 변환할 수 없습니다.")
            }
        } else {
            print("Base64 문자열을 Data로 변환할 수 없습니다.")
        }*/
        if isGraphicDisplay {
               // 받은 데이터를 사용하여 DotPadSDK API를 호출
               DotPadSDK.shared.dotPadAPI.dotPadProcessData.displayGraphicData(data: data)
           }
    }

}


