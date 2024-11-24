using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private Transform _pivotTransform;

    [Header("Properties")] 
    [SerializeField] [Range(0, 2)] private float _sensibilityX;
    [SerializeField] [Range(0, 2)] private float _sensibilityY;

    [Header("Events")] [SerializeField] private CharacterData _characterData;

    //Components
    private Id _id;
    private Transform _transform;
    private Camera _camera;

    //Properties
    private float _pivotXRot;
        

    private void Awake()
    {
        _transform = transform;
        _id = GetComponentInParent<Id>();
        _camera = GetComponent<Camera>();
        _camera.enabled = false;
            
        if (_characterData is null)
        {
            Assert.IsNull(_characterData, "character data is null");
            return;
        }

        _characterData.OnLookPerformed.AddListener(SetLookDeltaValue);
        _characterData.OnPossess.AddListener(OnPossess);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnPossess(int[] obj)
    {
        if (!obj.Contains(_id.GetId()))
        {
            return;
        }
            
        //To disable the camera of the non-possessed characters
        _camera.enabled = true;
    }

    private void SetLookDeltaValue(Vector2 lookDeltaValue, params int[] id)
    {
        if (!id.Contains(_id.GetId()))
        {
            return;
        }

        RotatePivot(lookDeltaValue.x * _sensibilityX);
        RotateCamera(lookDeltaValue.y * _sensibilityY);
    }


    private void RotateCamera(float y)
    {
        _pivotXRot -= y;
        _pivotXRot = Mathf.Clamp(_pivotXRot, -90, 90);
        _transform.localEulerAngles = Vector3.right * _pivotXRot;
    }

    private void RotatePivot(float x)
    {
        _pivotTransform.Rotate(0, x, 0);
    }
}