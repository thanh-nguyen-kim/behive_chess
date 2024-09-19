using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanState : TurnState
{
    [SerializeField] protected AudioClip hurryUpClip;
    protected float startTime;
    protected bool canPlaySFX;
    protected new void OnEnable()
    {
        base.OnEnable();
        startTime = Time.time;
        canPlaySFX = true;
        if (hexGrid.pendingCell != null)//handle when player back from tutorial and ready to make a move
            anim.Play("Appear");
        else
            PlayTurnAnim();
    }
    private void Update()
    {
        if (canPlaySFX && Time.time - startTime > 15)
        {
            canPlaySFX = false;
            AudioController.Instance.PlaySfx(hurryUpClip, 0.5f);
        }
    }
    public void OnClickTutorial()
    {
        ChangeState("Tutorial");
    }
    public void OnPlayerReady()
    {
        anim.Play("Appear");
    }
    public void OnClickGo()
    {
        hexGrid.NextTurn();
    }
}
