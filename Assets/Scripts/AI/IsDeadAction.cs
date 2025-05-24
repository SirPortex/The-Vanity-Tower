using UnityEngine;

[CreateAssetMenu(fileName = "IsDeadAction (A)", menuName = "ScriptableObject/Actions/IsDeadAction")] //IMPORTANTE

public class IsDeadAction : Action
{
    public override bool Check(GameObject owner)
    {
        SimpleCannon simpleCannon = owner.GetComponent<SimpleCannon>();

        if (simpleCannon.isEnemyDefeated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void DrawGizmos(GameObject owner)
    {

    }
}
