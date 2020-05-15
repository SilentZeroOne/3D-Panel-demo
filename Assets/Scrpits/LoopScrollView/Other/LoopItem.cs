using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopItem : MonoBehaviour
{
    private RectTransform rect;
    private RectTransform viewRect;

    private Vector3[] rectCorners;
    private Vector3[] viewCorners;

    private LoopScrollViewType viewType ;

    public Action onAddHead;
    public Action onRemoveHead;
    public Action onAddLast;
    public Action onRemoveLast;
    private void Awake()
    {
        rect =GetComponent<RectTransform>();        
        viewRect =transform.GetComponentInParent<LoopScrollView>().GetComponent<RectTransform>();
        rectCorners = new Vector3[4];
        viewCorners = new Vector3[4];
    }

    void Update()
    {
        ListenerCorners();
    }
    
    public void ListenerCorners()
    {
        //获取自身的边界
        rect.GetWorldCorners(rectCorners);
        //获取显示区域的边界
        viewRect.GetWorldCorners(viewCorners);
        if (isFirst())
        {
            switch (viewType)
            {
                case LoopScrollViewType.Vertical:
                    if (rectCorners[0].y > viewCorners[1].y)
                    {
                        //隐藏头节点
                        onRemoveHead?.Invoke();

                    }
                    if (rectCorners[1].y < viewCorners[1].y)
                    {
                        //添加头节点
                        onAddHead?.Invoke();
                    }
                    break;
                case LoopScrollViewType.Horizontal:
                    if (rectCorners[3].x < viewCorners[0].x)
                    {
                        //隐藏头节点
                        onRemoveHead?.Invoke();

                    }
                    if (rectCorners[0].x > viewCorners[0].x)
                    {
                        //添加头节点
                        onAddHead?.Invoke();
                    }
                    break;
            }

           
        }

        if (isLast())
        {

            switch (viewType)
            {
                case LoopScrollViewType.Vertical:
                    //添加尾部
                    if (rectCorners[0].y > viewCorners[0].y)
                    {
                        onAddLast?.Invoke();
                    }
                    //回收尾部
                    if (rectCorners[1].y < viewCorners[0].y)
                    {
                        onRemoveLast?.Invoke();
                    }
                    break;
                case LoopScrollViewType.Horizontal:
                    //添加尾部
                    if (rectCorners[3].x < viewCorners[3].x)
                    {
                        onAddLast?.Invoke();
                    }
                    //回收尾部
                    if (rectCorners[0].x > viewCorners[3].x)
                    {
                        onRemoveLast?.Invoke();
                    }
                    break;
            }

            
        }
    }

    public bool isFirst()
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    public bool isLast()
    {
        for (int i = transform.parent.childCount-1; i >=0; i--)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    public void SetLoopScrollViewType(LoopScrollViewType viewType)
    {
        this.viewType = viewType;
    }
}
