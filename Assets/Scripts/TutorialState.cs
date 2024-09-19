using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class TutorialState : State
{
    [SerializeField] private GameObject linePrefab, ringPrefab;
    [SerializeField] private GameObject mainCamera;
    private GameObject activeLineTutorial, activeRingTutorial;
    private bool canInteract;
    private float timeStamp;
    void OnEnable()
    {
        activeLineTutorial = Instantiate(linePrefab);
        activeRingTutorial = Instantiate(ringPrefab);
        Tween.Position(mainCamera.transform, new Vector3(0, 35, -42.7f), 0.75f, 0, Tween.EaseOutStrong);
        Tween.Position(activeLineTutorial.transform, new Vector3(10, 0, -32f), 1, 0, Tween.EaseOutStrong);
        Tween.Position(activeRingTutorial.transform, new Vector3(-10, 0, -32f), 1, 0, Tween.EaseOutStrong);
        GetComponent<Animator>().Play("Appear");
        timeStamp = Time.time;
        canInteract = true;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canInteract && Time.time - timeStamp > 2)
        {
            canInteract = false;
            GetComponent<Animator>().Play("Disappear");
            Tween.Position(activeLineTutorial.transform, new Vector3(40, 0, -32f), 1, 0, Tween.EaseLinear);
            Tween.Position(activeRingTutorial.transform, new Vector3(-40, 0, -32f), 1, 0, Tween.EaseLinear);
            Tween.Position(mainCamera.transform, new Vector3(0, 43.39f, -11.55f), 0.5f, 0.5f, Tween.EaseLinear);
            Destroy(activeLineTutorial, 1);
            Destroy(activeRingTutorial, 1);
            DelayChangeState();
        }
    }
    private async void DelayChangeState()
    {
        await System.Threading.Tasks.Task.Delay(1);
        ChangeState("Human");
    }
}
