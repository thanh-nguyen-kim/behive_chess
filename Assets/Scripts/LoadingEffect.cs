using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingEffect : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] cells;
    [SerializeField] private Material[] mats;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoLoadingEffect());
    }
    private IEnumerator DoLoadingEffect()
    {
        currentIndex = 0;
        var delay = new WaitForSecondsRealtime(0.1f);
        while (true)
        {
            cells[currentIndex].material = mats[0];
            currentIndex = (currentIndex + 1) % 6;
            cells[currentIndex].material = mats[1];
            yield return delay;

        }
    }
}
