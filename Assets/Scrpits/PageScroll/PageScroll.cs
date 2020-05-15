using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PageScrollType
{
    Horizontal,
    Vertical
}


public class PageScroll : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    protected ScrollRect rect;

    public int pageCount;
    protected float[] pages;

    public float scrollTime = 0.5f;
    public float timer = 0;
    private float startMovePos;
    protected int currentIndex=0;

    private bool isMoving;

    public bool isAutoMoving;
    public float autoScrollTime = 5f;
    private float autoTimer = 0;
    private bool isDraging ;

    public PageScrollType pageScrollType = PageScrollType.Horizontal;

    public Action<int> onPageChange;

    protected virtual void Start()
    {
        Init();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ListenerMove();
        ListenerAutoMove();
    }


    #region 接口实现
    public void OnEndDrag(PointerEventData eventData)
    {
       
        ScrollToPage(CaculateMinDistancePage());
        isDraging = false;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
        autoTimer = 0;
    }
    #endregion

    #region 方法
    public void Init()
    {
        rect = GetComponent<ScrollRect>();
        if (rect == null)
        {
            throw new System.Exception("未获取到ScrollRect!");
        }
        pageCount = rect.content.childCount;
        pages = new float[pageCount];
        if(pageCount==1)
        {
            throw new System.Exception("只有一页，不需要滚动");
        }
       
        for (int i = 0; i < pageCount; i++)
        {
            switch (pageScrollType)
            {
                case PageScrollType.Horizontal:            
                        pages[i] = i * (1.0f / (pageCount - 1));                
                    break;
                case PageScrollType.Vertical:                   
                        pages[i] = 1 - i * (1.0f / (pageCount - 1));                   
                    break;
            }
        }
    }

    public int CaculateMinDistancePage()
    {
        int minPage = 0;
        //计算出离得最近的一页
        for (int i = 1; i < pageCount; i++)
        {
            switch (pageScrollType)
            {
                case PageScrollType.Horizontal:
                    if (Mathf.Abs(pages[i] - rect.horizontalNormalizedPosition) < Mathf.Abs(pages[minPage] - rect.horizontalNormalizedPosition))
                    {
                        minPage = i;
                    }
                    break;
                case PageScrollType.Vertical:
                    if (Mathf.Abs(pages[i] - rect.verticalNormalizedPosition) < Mathf.Abs(pages[minPage] - rect.verticalNormalizedPosition))
                    {
                        minPage = i;
                    }
                    break;
            }

            
        }
        return minPage;
    }
    public void ListenerMove()
    {
        if (isMoving)
        {
            timer += Time.deltaTime * (1 / scrollTime);
            switch (pageScrollType)
            {
                case PageScrollType.Horizontal:
                    rect.horizontalNormalizedPosition = Mathf.Lerp(startMovePos, pages[currentIndex], timer);
                    break;
                case PageScrollType.Vertical:
                    rect.verticalNormalizedPosition = Mathf.Lerp(startMovePos, pages[currentIndex], timer);
                    break;
            }

            if (timer >= 1)
            {
                isMoving = false;
            }
        }

    }
    public void ListenerAutoMove()
    {
        if (isDraging) return;
        if (isAutoMoving)
        {
            autoTimer += Time.deltaTime;
            if (autoTimer >= autoScrollTime)
            {
                autoTimer = 0;
                currentIndex++;
                currentIndex %= pageCount;
                ScrollToPage(currentIndex);
            }
        }
    }

    public void ScrollToPage(int page)
    {
        isMoving = true;
        currentIndex = page;
        timer = 0;
        switch (pageScrollType)
        {
            case PageScrollType.Horizontal:
                startMovePos = rect.horizontalNormalizedPosition;
                break;
            case PageScrollType.Vertical:
                startMovePos = rect.verticalNormalizedPosition;
                break;
        }


        onPageChange?.Invoke(currentIndex);
    }
    #endregion
}
