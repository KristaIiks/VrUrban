using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportProvider : TeleportationProvider
{
    public void Teleport(Transform _point)
    {
        QueueTeleportRequest(new TeleportRequest
        {
            destinationPosition = _point.position,
            destinationRotation = _point.rotation
        });
    }
}
