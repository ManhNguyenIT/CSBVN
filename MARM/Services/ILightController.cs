namespace MARM.Services;

public interface ILightController
{
    event Action<int, bool> LightStateChanged;
    bool Light1 { get; }
    bool Light2 { get; }
    bool Light3 { get; }
    bool Light4 { get; }

    Task TurnOnLight(int index);
    Task TurnOffLight(int index);
}
//{"action":"","areaCode":"","berthCode":"","callCode":"100750AA100000","callTyp":"","clientCode":"","cooX":98031.0,"cooY":97000.0,"ctnrCode":"18E359D58A01F8N","ctnrTyp":"1","currentCallCode":"098031AA097000","currentPositionCode":"098031AA097000","data":{},"dstBinCode":"20000402201013","eqpCode":"","indBind":"","layer":"","mapCode":"AA","mapDataCode":"098031AA097000","mapShortName":"","materialLot":"","materialType":"","method":"end","orgCode":"","podCode":"","podDir":"","podNum":"","podTyp":"","relatedArea":"","reqCode":"18E35A123581F0X","reqTime":"2024-03-13 10:25:36","roadWayCode":"","robotCode":"1159","seq":"","stgBinCode":"20000402201013","subTaskNum":"","taskCode":"CTUOUT18E359FBABE1F9Y","taskTyp":"","tokenCode":"","username":"","wbCode":"100750AA100000","whCode":""}
