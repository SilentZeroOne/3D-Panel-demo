using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour,IBeginDragHandler,ICanvasRaycastFilter,IDragHandler
{

    private Vector3 mosusePos;

    private RectTransform rect;

    public Action onStartDrag;
    public Action onDrag;
    public Action onEndDrag;

    private bool isDraging;
    private Camera uiCamera;
    private Canvas canvas;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (rect == null)
            throw new System.Exception("只能拖拽UI物体");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        uiCamera=canvas.worldCamera;
    }
    private void Update()
    {
        if (isDraging)
        {   
            //界面未旋转时使用上面
            //rect.anchoredPosition += (Vector2)(Input.mousePosition - mosusePos);

            rect.position = uiCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, canvas.planeDistance));
            mosusePos = Input.mousePosition;
            onDrag?.Invoke();
        }
        if (Input.GetMouseButtonUp(0)&&isDraging)
        {
            onEndDrag?.Invoke();
            isDraging = false;
        }
       
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        mosusePos = Input.mousePosition;
        onStartDrag?.Invoke();
        isDraging = true;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !isDraging;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
