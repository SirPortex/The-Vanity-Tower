using UnityEngine;

[CreateAssetMenu(fileName = "ShootState (S)", menuName = "ScriptableObject/States/ShootState")] //IMPORTANTE
public class ShootState : State
{
    public override State Run(GameObject owner)
    {
        State nextState = CheckActions(owner);

        RotationReference reference = owner.GetComponentInChildren<RotationReference>(); //Referencia al objeto al que se va a rotar
        reference.LookAtTarget(); //Mirar al objeto de referencia

        Debug.Log("Shooting");

        return nextState;
    }
}
