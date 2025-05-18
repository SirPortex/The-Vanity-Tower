
using UnityEngine;

[CreateAssetMenu(fileName = "RotateState (S)", menuName = "ScriptableObject/States/RotateState")] //IMPORTANTE

public class RotateState : State
{



    public override State Run(GameObject owner)
    {
        State nextState = CheckActions(owner);
        RotationReference reference = owner.GetComponentInChildren<RotationReference>(); //Referencia al objeto al que se va a rotar

        reference.RotateObject(); //Rotar el objeto

        return nextState;

    }

}
