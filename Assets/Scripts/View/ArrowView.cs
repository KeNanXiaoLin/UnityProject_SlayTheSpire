using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject arrowHead;
    [SerializeField] private LineRenderer lineRenderer;

    private Vector3 startPos;

    private void Update()
    {
        Vector3 endPos = MouseUtil.GetMousePositionInWorldSpace(-1);
        Vector3 direction = -(startPos - arrowHead.transform.position).normalized;
        lineRenderer.SetPosition(1,endPos- direction * 0.5f);
        arrowHead.transform.position = endPos;
        arrowHead.transform.right = direction;
    }

    public void SetUpArrow(Vector3 pos)
    {
        startPos = pos;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace(-1));
    }
}
