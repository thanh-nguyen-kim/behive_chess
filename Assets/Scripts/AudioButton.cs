using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private AudioClip btnClickAudio;
    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }
    private void OnDestroy()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(OnClick);
    }
    public void OnClick()
    {
        AudioController.Instance.PlaySfx(btnClickAudio, 0.5f);
    }

}
