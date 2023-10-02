using UnityEngine;
using UnityEngine.InputSystem;
using StudySystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Player.Hands
{
    public class HandControls : MonoBehaviour, IStudy
    {
        [SerializeField] private GameObject _interactHand;
        [SerializeField] private GameObject _teleportHand;

        [SerializeField] private InputActionReference _teleportAction;

        private void Awake()
        {
            _teleportAction.action.performed += StartTeleport;
            _teleportAction.action.canceled += FinishTeleport;
        }

        private void StartTeleport(InputAction.CallbackContext cnt)
        {
            _interactHand.SetActive(false);
            _teleportHand.SetActive(true);
        }

        private void FinishTeleport(InputAction.CallbackContext cnt) => Invoke("Deactivate", .01f);
        private void Deactivate()
        {
            _interactHand.SetActive(true);
            _teleportHand.SetActive(false);
        }

        public void StartStudy()
        {
            _teleportHand.GetComponent<XRRayInteractor>().selectEntered.AddListener(StopStudy);
        }

        private void StopStudy(SelectEnterEventArgs args)
        {
            _teleportHand.GetComponent<XRRayInteractor>().selectEntered.RemoveListener(StopStudy);
            StudyManager.Instance.StartNextStage();
        }
    }
}
