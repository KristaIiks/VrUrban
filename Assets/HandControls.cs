using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandControls : MonoBehaviour
{
    [SerializeField] private XRRayInteractor _standartHand;

    [SerializeField] private XRRayInteractor _teleportHand;
    [SerializeField] private XRInteractorLineVisual _teleportVisual;

	[SerializeField] private InputActionReference _trackpad;

    [SerializeField] private InputActionReference _snapValue;
    [SerializeField] private float _snapSpeed = 45;

    [SerializeField] private InputActionReference _teleport;

    [SerializeField] private ControllerTips[] _tips;

    private XROrigin _rig;
    private TeleportationProvider _provider;

    private GameObject _current;


    private void Awake()
    {
        _rig = GetComponentInParent<XROrigin>();
        _provider = GetComponentInParent<TeleportationProvider>();
       
        //_trackpad.action.performed += (s) => _rig.RotateAroundCameraUsingOriginUp((_snapValue.action.ReadValue<Vector2>().x != 0 ? Mathf.Sign(_snapValue.action.ReadValue<Vector2>().x) : 0) * _snapSpeed);

        _trackpad.action.performed += (s) => StartTeleport();
        _trackpad.action.canceled += (s) => FinishTeleport();
    }

    private void StartTeleport()
    {
        _standartHand.gameObject.SetActive(false);
        _teleportHand.gameObject.SetActive(true);
    }

    private void FinishTeleport()
    {
        _standartHand.gameObject.SetActive(true);
        _teleportHand.gameObject.SetActive(false);

        if (_teleportHand.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            _provider.QueueTeleportRequest(new TeleportRequest
            {
                destinationPosition = hit.point
            });
        }
    }

    #region Study

    public void StartStudy(ControllerBtn _btn) => Study(_btn);
    public void StartStudy(ControllerBtn _btn, bool _autoFinish) => Study(_btn, _autoFinish);

    private void Study(ControllerBtn _btn, bool _autoFinish = true)
    {
        foreach (ControllerTips item in _tips)
        {
            if (item._btn == _btn)
            {
                _current = item._model;

                if (_autoFinish)
                {
                    item._action.performed += (s) => StopStudy(_btn);
                }
                break;
            }
        }

    }

    public void StopStudy(ControllerBtn _btn = ControllerBtn.None)
    {
        if (_btn != ControllerBtn.None) // Auto finish
        {

            foreach (ControllerTips item in _tips)
            {
                if (item._btn == _btn)
                {
                    item._model.SetActive(false);
                    break;
                }
            }

            return;
        }
        _current?.SetActive(false);
    }

    #endregion

}

[System.Serializable]
public struct ControllerTips
{
    public GameObject _model;
    public ControllerBtn _btn;
    public InputAction _action;
}

[System.Serializable]
public enum ControllerBtn
{
    Trigger,
    Joystick,
    Catch,
    Menu,
    None
}
