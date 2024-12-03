using MARM.Enums;

namespace MARM.Services;

public interface ITargetConnectStateManager
{
    event Action<TargetConnectState>? MainTargetConnectStateChanged;
    TargetConnectState MainTargetConnectState { get; }

    event Action<TargetConnectState>? SubTargetConnectStateChanged;
    TargetConnectState SubTargetConnectState { get; }
}
