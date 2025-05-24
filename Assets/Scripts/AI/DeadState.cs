using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeadState (S)", menuName = "ScriptableObject/States/EnemyDeadState")] //IMPORTANTE


public class DeadState : State
{
    public override State Run(GameObject owner) // No hace absolutamente nada
    {
        State nextState = CheckActions(owner);

        return nextState;
    }
}
