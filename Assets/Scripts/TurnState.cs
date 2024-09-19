using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class TurnState : State
{
    protected Animator anim;
    protected HexGrid hexGrid;
    [SerializeField] protected Animator cellAnim;
    
    protected void OnEnable()
    {
        anim = GetComponent<Animator>();
        hexGrid = FindObjectOfType<HexGrid>();
    }
    protected async void PlayTurnAnim()
    {
        anim.Play("Active");
        await System.Threading.Tasks.Task.Delay(800);
        cellAnim.Play("Active");
    }
    protected void OnDisable()
    {
        anim.Play("Idle");
        cellAnim.Play("Idle");
    }
}
