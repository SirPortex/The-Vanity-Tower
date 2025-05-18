using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct ActionParameters
{
    [Tooltip("Action that is gonna be executed")]
    public Action action;
    [Tooltip("Indicates if the action´s check must be true or false")]
    public bool actionValue;
}

[System.Serializable]

public struct StateParameters
{
    [Tooltip("ActionParameters array")]
    public ActionParameters[] actionParameters;
    [Tooltip("If the actions´s check equals acionValue, nextState is pushed")]
    public State nextState;
    [Tooltip("All actoins must be cheked if they are all true")]
    public bool and;

}

public abstract class State : ScriptableObject
// El run de la clase state no hace nada.
{

    public StateParameters[] stateParameters;

    public void DrawnAllActionsGizmos(GameObject owner)
    {
        //for (int i = 0; i < stateParameters.Length; i++)
        //{
        //    stateParameters[i].action.DrawGizmos(owner);
        //}

        foreach (StateParameters parameters in stateParameters)
        {
            foreach (ActionParameters aP in parameters.actionParameters) // recorre toas las acciones
            {
                aP.action.DrawGizmos(owner);
            }
        }
    }

    protected State CheckActions(GameObject owner) //devolvera true si alguna de sus acciones de cumple, o false si es el contrario
    {
        for (int i = 0; i < stateParameters.Length; i++) // Recorremos el array de los parametros que tenemos en este for
        {
            bool allActionsTrue = true;
            for (int j = 0; j < stateParameters[i].actionParameters.Length; j++) // recorremos el array de las acciones que se tienen que repetir
            {
                ActionParameters actionParameter = stateParameters[i].actionParameters[j];
                if (actionParameter.action.Check(owner) == actionParameter.actionValue) //si se cumplen todas las condiciones
                {
                    if (!stateParameters[i].and)//si solo se tiene que cumplir una
                    {
                        //devolvemos directamente el siguiente estado
                        return stateParameters[i].nextState;
                    }

                }
                else if (stateParameters[i].and)
                {
                    allActionsTrue = false;

                    //estamos seguros de que esta accion no se ha cumplido
                    //y el disenador me ha marcado que se tienen que cumplir todas.

                    break; // Se sale del bucle, lo rompe. Por que una accion no se ha cumplido y estamos en "and".
                }
            }

            //Si llegamos hasta aqui, significa que el disenador ha marcado que todas las acciones
            //tienen que cumplirse. Tenemos que comprobar si de verdad se han cumplido todas.s
            if (stateParameters[i].and && allActionsTrue)
            {
                return stateParameters[i].nextState; // Si esta puesto el and 
            }

        }

        return null; // ninguna accion se cumple por lo que no cambiamos de estado
    }

    public abstract State Run(GameObject owner); // comprueba si las acciones se cumplen y ademas, ejecuta el comportamiento asociado al estado.
}
