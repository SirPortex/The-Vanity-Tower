using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeeAction (A)", menuName = "ScriptableObject/Actions/SeeAction")] //IMPORTANTE
public class SeeAction : Action // dar a la bombilla para que lo complete bien la estructura
{
    public float vision;

    public override bool Check(GameObject owner)
    {
        GameObject target = owner.GetComponent<TargetReference>().target; //Cogemos al Target para saber a que perseguir
        FieldOfView fieldOfView = owner.GetComponentInChildren<FieldOfView>();

        if (fieldOfView.canSeePlayer)
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
