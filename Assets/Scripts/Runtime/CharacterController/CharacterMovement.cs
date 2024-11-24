using SerializedProperties;
using UnityEngine;

public class CharacterMovement
{
    private readonly CharacterBodyProperties _characterBodyProperties;
    private readonly CharacterMovementProperties _characterMovementProperties;
    private Vector3 _moveAmount;

    private Vector3 _velocity;
    private bool _isSprinting;

    public CharacterMovement(ref CharacterBodyProperties characterBodyProperties,
        ref CharacterMovementProperties characterMovementProperties)
    {
        _characterBodyProperties = characterBodyProperties;
        _characterMovementProperties = characterMovementProperties;
    }

    public Vector3 Velocity => _velocity;

    private void MoveTo(Vector3 targetPos)
    {
        _characterBodyProperties.Transform.position = targetPos;
    }

    private void AddVelocity(Vector3 velocity)
    {
        _velocity += velocity;
    }

    private void SetVelocity(float x, float y, float z)
    {
        _velocity.Set(x, y, z);
    }

    private void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void SetMoveAmount(Vector3 moveAmount)
    {
        _moveAmount = moveAmount;
    }

    public void StartSprint()
    {
        _isSprinting = true;
    }

    public void StopSprint()
    {
        _isSprinting = false;
    }

    public void StartJump()
    {
        AddVelocity(Vector3.up * _characterMovementProperties.JumpForce);
    }
        
    public void StopJump()
    {
        //TODO: make a jump higher the more long it is pressed
    }

    public Vector3 CalculateDeltaMoveAmount()
    {
        float currentSpeed = _isSprinting
            ? _characterMovementProperties.SprintSpeed
            : _characterMovementProperties.WalkSpeed;
            
        //To get the right direction based on the forward of the character and apply the speed
        return _characterBodyProperties.Transform.TransformDirection(_moveAmount * (Time.deltaTime * currentSpeed));
    }

    public void MoveAndApplyVel(Vector3 collidedGravity, Vector3 calculatedMoveAmount, Vector3 pushForce)
    {
        SetVelocity(calculatedMoveAmount + collidedGravity);
        MoveTo(_characterBodyProperties.Transform.position + (_velocity) * Time.deltaTime + pushForce);
            
        //To not keep the old velocity in y but just the gravity
        SetVelocity(_velocity.x, collidedGravity.y, _velocity.z);
    }

        
}