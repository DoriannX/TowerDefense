using System;
using Runtime.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
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
    //pause
    private Action _onPause;
        
    private bool _isSprinting;

    private void SetupCharacter()
    {
        Debug.Log("setup character");
        _onMovePerformed += GameEvents.OnMovePerformed.Invoke;
        _onMoveCanceled += GameEvents.OnMoveCanceled.Invoke;
        _onMoveCanceled += GameEvents.OnMoveCanceled.Invoke;
        _onJumpStarted += GameEvents.OnJumpStarted.Invoke;
        _onJumpCanceled += GameEvents.OnJumpCanceled.Invoke;
        _onSprintStarted += GameEvents.OnSprintStarted.Invoke;
        _onSprintCanceled += GameEvents.OnSprintCanceled.Invoke;
        _onLookPerformed += GameEvents.OnLookPerformed.Invoke;
        _onLookCanceled += GameEvents.OnLookCanceled.Invoke;
        _onShootStarted += GameEvents.OnShootStarted.Invoke;
        _onShootCanceled += GameEvents.OnShootCanceled.Invoke;
        _onPossess += GameEvents.OnPossess.Invoke;
        _onToggleMode += GameEvents.OnToggleMode.Invoke;
        _onToggleFocusCamera += GameEvents.OnToggleFocusCamera.Invoke;
        _onPause += EventManager.OnPause.Invoke;
    }

    private void Start()
    {
        SetupCharacter();
        _onPossess?.Invoke(_ids);
        EventManager.OnEnd += DisableInputs;
    }

    private void DisableInputs()
    {
        _onMovePerformed -= GameEvents.OnMovePerformed.Invoke;
        _onMoveCanceled -= GameEvents.OnMoveCanceled.Invoke;
        _onMoveCanceled -= GameEvents.OnMoveCanceled.Invoke;
        _onJumpStarted -= GameEvents.OnJumpStarted.Invoke;
        _onJumpCanceled -= GameEvents.OnJumpCanceled.Invoke;
        _onSprintStarted -= GameEvents.OnSprintStarted.Invoke;
        _onSprintCanceled -= GameEvents.OnSprintCanceled.Invoke;
        _onLookPerformed -= GameEvents.OnLookPerformed.Invoke;
        _onLookCanceled -= GameEvents.OnLookCanceled.Invoke;
        _onShootStarted -= GameEvents.OnShootStarted.Invoke;
        _onShootCanceled -= GameEvents.OnShootCanceled.Invoke;
        _onPossess -= GameEvents.OnPossess.Invoke;
        _onToggleMode -= GameEvents.OnToggleMode.Invoke;
        _onToggleFocusCamera -= GameEvents.OnToggleFocusCamera.Invoke;
        _onPause -= EventManager.OnPause.Invoke;
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
    
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            _onPause?.Invoke();
            
        }
    }
}