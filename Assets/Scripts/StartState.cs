using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class StartState : State
{
    [SerializeField] private GameObject[] tiles, firstTurns;
    void OnEnable()
    {
        tiles[0].SetActive(false);
        tiles[1].SetActive(false);
        PlayStartAnimation();
    }
    private async void PlayStartAnimation()
    {
        firstTurns[0].SetActive(false);
        firstTurns[1].SetActive(false);
        var hexGrid = FindObjectOfType<HexGrid>();
        await System.Threading.Tasks.Task.Delay(100);
        firstTurns[0].SetActive(hexGrid.humanMoveFirst);
        firstTurns[1].SetActive(!hexGrid.humanMoveFirst);
        GetComponent<Animator>().Play("Appear");
        await System.Threading.Tasks.Task.Delay(2000);
        GetComponent<Animator>().Play("Disappear");
        await System.Threading.Tasks.Task.Delay(1000);
        firstTurns[0].SetActive(false);
        firstTurns[1].SetActive(false);
        hexGrid.ConstructBoard();
        await System.Threading.Tasks.Task.Delay(1000);
        hexGrid.StartGame();
        tiles[0].SetActive(true);
        tiles[1].SetActive(true);
    }
    void OnDisable()
    {

    }
}
