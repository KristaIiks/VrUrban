using UnityEngine.XR.Interaction.Toolkit;

namespace StudySystem
{
    public class TeleportArea : TeleportationArea, IStudy
    {
        public void StartStudy()
        {
            teleporting.AddListener(FinishStudy);
        }

        private void FinishStudy(TeleportingEventArgs args)
        {
            teleporting.RemoveListener(FinishStudy);
            StudyManager.Instance.StartNextStage();
        }
    }
}
