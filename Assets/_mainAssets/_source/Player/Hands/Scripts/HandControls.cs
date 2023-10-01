using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Hands
{
    public class HandControls : MonoBehaviour
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

    }
}
