using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CharacterData _serializedCharacterData;
    [SerializeField] private int[] _ids;
        
    //Values
    private Vector3 _moveValue;
    public Vector2 LookDeltaValue { get; private set; }

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
    //Possess
    private Action<int[]> _onPossess;
        
    private bool _isSprinting;

    public void SetupCharacter(CharacterData characterData)
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
        _onPossess += characterData.OnPossess.Invoke;
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
        
    private void ObsoleteUpdate()
    {
        /*float force = _isSprinting ? _sprintForce : _moveForce;
            if (Input.GetKey(KeyCode.W))
            {
                _characterController.AddVelocity(_characterController.transform.forward * (force * Time.fixedDeltaTime));
            }

            if (Input.GetKey(KeyCode.A))
            {
                _characterController.AddVelocity(-_characterController.transform.right * (Time.fixedDeltaTime * force));
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                _characterController.AddVelocity(-_characterController.transform.forward * (force * Time.fixedDeltaTime));
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                _characterController.AddVelocity(_characterController.transform.right * (force * Time.fixedDeltaTime));
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _isSprinting = true;
            }
            
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _isSprinting = false;
            }
            
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }*/
    }
}