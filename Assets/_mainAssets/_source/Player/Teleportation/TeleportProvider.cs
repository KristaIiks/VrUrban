using UnityEngine;

public class TeleportProvider : UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider
{
    public void Teleport(Transform _point)
    {
        QueueTeleportRequest(new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest
        {
            destinationPosition = _point.position,
            destinationRotation = _point.rotation
        });
    }
}
