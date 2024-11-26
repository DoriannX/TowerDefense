using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
public class CharacterData : ScriptableObject
{
    //Actions
    //Move
    public Vector3GameEventId OnMovePerformed;
    public Vector3GameEventId OnMoveCanceled;
    //Jump
    public GameEventId OnJumpStarted;
    public GameEventId OnJumpCanceled;
    //Sprint
    public GameEventId OnSprintStarted;
    public GameEventId OnSprintCanceled;
    //Look
    public Vector2GameEventId OnLookPerformed;
    public Vector2GameEventId OnLookCanceled;
    //Shoot
    public GameEventId OnShootStarted;
    public GameEventId OnShootCanceled;
    //Possess
    public GameEventId OnPossess;
    //Creation
    public GameEventId OnCreated;

    public void RemoveAllListeners()
    {
        OnMovePerformed.ClearListeners();
        OnMoveCanceled.ClearListeners();
        OnJumpStarted.ClearListeners();
        OnJumpCanceled.ClearListeners();
        OnSprintStarted.ClearListeners();
        OnSprintCanceled.ClearListeners();
        OnLookPerformed.ClearListeners();
        OnLookCanceled.ClearListeners();
        OnShootStarted.ClearListeners();
        OnShootCanceled.ClearListeners();
        OnPossess.ClearListeners();
        OnCreated.ClearListeners();
    }
}