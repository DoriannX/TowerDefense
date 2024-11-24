using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
public class CharacterData : ScriptableObject
{
    //Actions
    //Move
    public Vector3GameEvent OnMovePerformed;
    public Vector3GameEvent OnMoveCanceled;
    //Jump
    public GameEvent OnJumpStarted;
    public GameEvent OnJumpCanceled;
    //Sprint
    public GameEvent OnSprintStarted;
    public GameEvent OnSprintCanceled;
    //Look
    public Vector2GameEvent OnLookPerformed;
    public Vector2GameEvent OnLookCanceled;
    //Possess
    public GameEvent OnPossess;

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
    }
}