using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class Cell : MonoBehaviour
{
    public Material[] cellMats;
    public int xPos, zPos, color;
    [SerializeField] private GameObject markPrefab;
    private MeshRenderer meshRenderer;
    private Color originColor;
    private GameObject mark;
    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;
    }
    public void Init(int x, int z)
    {
        xPos = x;
        zPos = z;
        color = 0;
    }
    public void OnSelect()
    {
        Debug.Log("On Cell Click");
        HexGrid.Instance.OnCellClick(xPos, zPos);
    }
    public void SetColor(int _color)
    {
        color = _color;
        if (_color == 0)
        {
            meshRenderer.enabled = false;
            if (mark != null) Destroy(mark);
        }
        else
        {
            if (!meshRenderer.enabled)
                meshRenderer.enabled = true;
            if (color == 1)
                meshRenderer.material = cellMats[0];
            else if (color == -1) meshRenderer.material = cellMats[1];
        }
    }
    public void PlayEffect()
    {
        originColor = meshRenderer.material.GetColor("_BaseColor");
        Tween.ShaderColor(meshRenderer.material, "_BaseColor", originColor * 0.8f, 0.75f, 0, null, Tween.LoopType.PingPong);
        mark = Instantiate(markPrefab, transform);
    }
    public void StopEffect()
    {
        Tween.Stop(meshRenderer.material.GetInstanceID());
        meshRenderer.material.SetColor("_BaseColor", originColor);
        if (mark != null) Destroy(mark);
    }
}
