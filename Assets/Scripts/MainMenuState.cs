using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class MainMenuState : State
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("Idle");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    public void OnClickAI()
    {
        StateMachine.Next();
    }
    public void OnClickHuman()
    {
        SceneLoader.Instance.LoadScene("Game_1");
    }
}
