using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UndoController : MonoBehaviour
{
    private int totalUndo = 0;
    [SerializeField] private GameObject cube;
    private void OnEnable()
    {
        var hexGrid = FindObjectOfType<HexGrid>();
        cube.SetActive(hexGrid.CanUndo());
    }
    public void OnClickUndo()
    {
        var hexGrid = FindObjectOfType<HexGrid>();
        if (hexGrid.CanUndo())
        {
            if (UnityAdsController.Instance.IsVideoRewardAdsReady() && totalUndo > 0)
            {
                UnityAdsController.Instance.ShowVideoAds(() =>
                {
                    totalUndo++;
                    hexGrid.Undo();
                    cube.SetActive(hexGrid.CanUndo());
                }, null);
            }
            else
            {
                totalUndo++;
                hexGrid.Undo();
                cube.SetActive(hexGrid.CanUndo());
            }
        }
    }

}
