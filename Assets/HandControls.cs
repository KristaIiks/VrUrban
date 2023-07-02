using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandControls : MonoBehaviour
{
	[SerializeField] private InputActionReference _rotateInput;
    [SerializeField] private InputActionReference _snapValue;
    [SerializeField] private float _snapSpeed = 45;

    [SerializeField] private ControllerTips[] _tips;

    private XROrigin _rig;
    private GameObject _current;


    private void Awake()
    {
        _rig = GetComponentInParent<XROrigin>();
       
        _rotateInput.action.performed += (s) => _rig.RotateAroundCameraUsingOriginUp((_snapValue.action.ReadValue<Vector2>().x != 0 ? Mathf.Sign(_snapValue.action.ReadValue<Vector2>().x) : 0) * _snapSpeed);
    }

    public void StartStudy(ControllerBtn _btn) => Study(_btn);
    public void StartStudy(ControllerBtn _btn, bool _autoFinish) => Study(_btn, _autoFinish);

    private void Study(ControllerBtn _btn, bool _autoFinish = true)
    {
        //switch (_btn)
        //{
        //    case ControllerBtn.Joystick:
        //        break;
        //    default:
        //        Debug.Log("No study");
        //        break;
        //}
        //_rotateInput.action.performed += (s) => StopStudy(obj);
    }

    public void StopStudy(GameObject _model = null)
    {
        if(_model != null)
        {
            _model.SetActive(false);
        }
        _current?.SetActive(false);
    }

}

[System.Serializable]
public struct ControllerTips
{
    public GameObject _model;
    public ControllerBtn _btn;
}

[System.Serializable]
public enum ControllerBtn
{
    Trigger,
    Joystick
}
