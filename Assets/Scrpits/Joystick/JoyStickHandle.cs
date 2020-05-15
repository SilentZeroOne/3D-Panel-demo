using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickHandle : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

    JoyStick joyStick;
    Vector3 offset;
    Vector3 mousePos;
    RectTransform rect;
    Canvas canvas;

    //角度
    public float angle;
    float limitX;
    float limitY;

    float moveTime = 0.2f;
    float moveTimer = 0;
    Vector3 startMovePos;
    bool isMoveToOrigin;

    public Vector2 velocity;

    public Action onBeginDrag;
    public Action onDrag;
    public Action onEndDrag;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        joyStick = GetComponentInParent<JoyStick>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null) { throw new System.Exception("未查询到Canvas！"); }
    }

    private void Update()
    {
        CaculateAngle();
        LimitHandlePos();
        MovingOrigin();
        if (Vector3.Distance(transform.localPosition, Vector3.zero) >= joyStick.minDistance)
        { velocity = new Vector2(transform.localPosition.x / joyStick.maxDistance, transform.localPosition.y / joyStick.maxDistance); }
        else
        {
            velocity = Vector3.zero;
        }
            
    }

    private void MovingOrigin()
    {
        if (isMoveToOrigin)
        {
            moveTimer += Time.deltaTime * (1 / moveTime);
            transform.localPosition = Vector3.Lerp(startMovePos, Vector3.zero, moveTimer);
            if (moveTimer >= 1)
            {
                isMoveToOrigin = false;
                moveTimer = 0;
            }
        }
    }

    private void MoveToOrigin()
    {
        isMoveToOrigin = true;
        startMovePos = transform.localPosition;
    }
    public void CaculateAngle()
    {
        angle = Mathf.Atan2(transform.localPosition.y, transform.localPosition.x)*Mathf.Rad2Deg;
        
    }

    public void LimitHandlePos()
    {
        if (Vector3.Distance(transform.localPosition, Vector3.zero) >= joyStick.maxDistance)
        {
            limitY = Mathf.Sin(angle * Mathf.Deg2Rad) * joyStick.maxDistance;
            limitX = Mathf.Cos(angle * Mathf.Deg2Rad) * joyStick.maxDistance;

            transform.localPosition = new Vector3(limitX, limitY, 0);
        }
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        onBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rect.anchoredPosition += (Vector2)(Input.mousePosition - mousePos);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
        transform.localPosition = localPoint;
        //mousePos = Input.mousePosition;
        onDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MoveToOrigin();
        onEndDrag?.Invoke();
    }

    
}
