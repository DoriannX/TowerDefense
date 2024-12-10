using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CharacterData _serializedCharacterData;
    [SerializeField] private int[] _ids;
        
    //Values
    private Vector3 _moveValue;
    private Vector2 LookDeltaValue { get; set; }

    //Actions
    //Move
    private Action<Vector3, int[]> _onMovePerformed;
    private Action<Vector3, int[]> _onMoveCanceled;
    //Jump
    private Action<int[]> _onJumpStarted;
    private Action<int[]> _onJumpCanceled;
    //Sprint
    private Action<int[]> _onSprintStarted;
    private Action<int[]> _onSprintCanceled;
    //Look
    private Action<Vector2, int[]> _onLookPerformed;
    private Action<Vector2, int[]> _onLookCanceled;
    //Shoot
    private Action<int[]> _onShootStarted;
    private Action<int[]> _onShootCanceled;
    //Possess
    private Action<int[]> _onPossess;
    //unfocus camera
    private Action<int[]> _onToggleFocusCamera;
    //change mode
    private Action<int[]> _onToggleMode;
        
    private bool _isSprinting;

    private void SetupCharacter(CharacterData characterData)
    {
        _onMovePerformed += characterData.OnMovePerformed.Invoke;
        _onMoveCanceled += characterData.OnMoveCanceled.Invoke;
        _onMoveCanceled += characterData.OnMoveCanceled.Invoke;
        _onJumpStarted += characterData.OnJumpStarted.Invoke;
        _onJumpCanceled += characterData.OnJumpCanceled.Invoke;
        _onSprintStarted += characterData.OnSprintStarted.Invoke;
        _onSprintCanceled += characterData.OnSprintCanceled.Invoke;
        _onLookPerformed += characterData.OnLookPerformed.Invoke;
        _onLookCanceled += characterData.OnLookCanceled.Invoke;
        _onShootStarted += characterData.OnShootStarted.Invoke;
        _onShootCanceled += characterData.OnShootCanceled.Invoke;
        _onPossess += characterData.OnPossess.Invoke;
        _onToggleMode += GameEvents.OnToggleMode.Invoke;
        _onToggleFocusCamera += GameEvents.OnToggleFocusCamera.Invoke;
    }

    private void Start()
    {
        SetupCharacter(_serializedCharacterData);
        _onPossess?.Invoke(_ids);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        if (ctx.performed)
        {
            _moveValue.Set(input.x, _moveValue.y, input.y);
            _onMovePerformed?.Invoke(_moveValue, _ids);
        }

        if (ctx.canceled)
        {
            _onMoveCanceled?.Invoke(Vector3.zero, _ids);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _onJumpStarted?.Invoke(_ids);
        }

        if (ctx.canceled)
        {
            _onJumpCanceled?.Invoke(_ids);
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            _onSprintStarted?.Invoke(_ids);
        }
            
        if (ctx.canceled)
        {
            _onSprintCanceled?.Invoke(_ids);
        }
    }
        
    public void OnLook(InputAction.CallbackContext ctx)
    {
        LookDeltaValue = ctx.ReadValue<Vector2>();
            
        if(ctx.started)
        {
            _onLookPerformed?.Invoke(LookDeltaValue, _ids);
        }
            
        if (ctx.canceled)
        {
            _onLookCanceled?.Invoke(LookDeltaValue, _ids);
        }
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _onShootStarted?.Invoke(_ids);
        }

        if (ctx.canceled)
        {
            _onShootCanceled?.Invoke(_ids);
        }
    }

    public void OnToggleFocusCamera(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            _onToggleFocusCamera?.Invoke(_ids);
        }
        
        if(ctx.canceled)
        {
            _onToggleFocusCamera?.Invoke(_ids);
        }
    }
    
    public void OnToggleMode(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            _onToggleMode?.Invoke( _ids);
        }
    }
}