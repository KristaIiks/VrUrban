using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportProvider : TeleportationProvider
{
    public void Teleport(Vector3 _point)
    {
        QueueTeleportRequest(new TeleportRequest
        {
            destinationPosition = _point
        });
    }
}
