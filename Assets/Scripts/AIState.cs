using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class AIState : TurnState
{
    protected new void OnEnable()
    {
        base.OnEnable();
        PlayTurnAnim();
        hexGrid.MachineMove();
    }
}
