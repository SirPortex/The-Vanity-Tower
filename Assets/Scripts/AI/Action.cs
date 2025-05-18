
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public bool value; //Si la accion tiene o no que cumplirse ( si veo al jugador o no lo veo )
    public abstract bool Check(GameObject owner); //En el check va a ejecutar la accion, y comprobar que se cumpla ciertos parametros.

    public abstract void DrawGizmos(GameObject owner);

}
