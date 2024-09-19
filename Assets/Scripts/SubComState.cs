using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class SubComState : State
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("Appear");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    public void OnClickHard()
    {
        PlayerPrefs.SetInt(HexGrid.WIN_COUNT, 0);
        SceneLoader.Instance.LoadScene("Game");
    }
    public void OnClickElite()
    {
        PlayerPrefs.SetInt(HexGrid.WIN_COUNT, 7);
        SceneLoader.Instance.LoadScene("Game");
    }
}
