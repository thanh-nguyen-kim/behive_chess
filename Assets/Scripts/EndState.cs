using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.SceneManagement;
public class EndState : State
{
    [SerializeField] private GameObject[] winLabels, winNotifications;
    [SerializeField] private AudioClip[] endClips;
    [SerializeField] private AudioClip clickClip;
    private bool canInteract = false;
    private float timeStamp;
    private async void OnEnable()
    {
        var hexGrid = FindObjectOfType<HexGrid>();
        winLabels[0].SetActive(hexGrid.winnerID == -1);
        winLabels[1].SetActive(hexGrid.winnerID == 1);
        winNotifications[0].SetActive(hexGrid.winnerID == -1);
        winNotifications[1].SetActive(hexGrid.winnerID == 1);
        canInteract = true;
        timeStamp = Time.time;
        GetComponent<Animator>().Play("Appear");
        await System.Threading.Tasks.Task.Delay(1);
        if (hexGrid.winnerID == -1)
            AudioController.Instance.PlaySfx(endClips[0]);
        else
            AudioController.Instance.PlaySfx(endClips[1]);
    }
    private void OnDisable()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canInteract && Time.time - timeStamp > 2)
        {
            canInteract = false;
            GetComponent<Animator>().Play("Disappear");
            AudioController.Instance.PlaySfx(clickClip, 0.5f);
        }
    }
    public void OnClickLoadScene(string scene)
    {
        SceneLoader.Instance.LoadScene(scene);
    }
}
