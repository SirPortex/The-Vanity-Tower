using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State initialState;
    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = initialState;
    }

    // Update is called once per frame
    void Update()
    {
        State nextState = currentState.Run(gameObject);

        if (nextState != null) // si se devuelve algun tipo de informacion cambiamos de estado
        {
            currentState = nextState;
        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

    public void OnDrawGizmos()
    {
        if (currentState)
            currentState.DrawnAllActionsGizmos(gameObject);
        else if (initialState)
            initialState.DrawnAllActionsGizmos(gameObject);
    }
}

