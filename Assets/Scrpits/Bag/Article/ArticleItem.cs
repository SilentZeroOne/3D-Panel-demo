using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleItem : MonoBehaviour
{
    private Image articleSprite;
    private Text number;

    private Article article;

    private UIDrag uiDrag;

    private Canvas canvas;
    private int defaultSort;

    private Vector3 currentLocalPos;
    private float moveOriginTimer; //记时
    private float moveOriginTime=0.2f; //时间
    private bool isMovingOrigin;
    private Action onMoveEnd;
    //Scale动画
    private float scaleTimer;
    private float scaleTime;
    private Vector3 startScale;
    bool isScaling;

    public Article Article
    {
        get { return article; }
    }

    private void Awake()
    {
        articleSprite = GetComponent<Image>();
        number = GetComponentInChildren<Text>();
        uiDrag = GetComponent<UIDrag>();
        canvas = GetComponent<Canvas>();
        defaultSort = canvas.sortingOrder;

        uiDrag.onDrag += OnDrag;
        uiDrag.onStartDrag += OnStartDrag;
        uiDrag.onEndDrag += OnEndDrag;
    }

    private void Update()
    {
        //向原点移动
        MovingOrigin();
        //改变Scale
        MovingScaleOne();
    }

    public void SetArticle(Article article)
    {
        this.article = article;
        
        //绑定事件
        this.article.onDataChange = OnDataChange;
        articleSprite.sprite = Resources.Load<Sprite>(article.spritePath);
        number.text = article.count.ToString();
       
    }

    public void OnStartDrag() 
    {
        canvas.sortingOrder = defaultSort + 1;
        BagPanel._instance.currentDragArticle = this;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnEndDrag()
    {
        if (BagPanel._instance.currentHoverGrid != null)
        {
            //进入格子
            BagPanel._instance.currentHoverGrid.DragToThisGrid(BagPanel._instance.currentDragArticle);
            canvas.sortingOrder = defaultSort;
        }
        else
        {
            //回到原点
            MoveToOrigin(() => { canvas.sortingOrder = defaultSort; });
            
        }
        BagPanel._instance.currentDragArticle = null;

        //恢复
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnDrag()
    {
        
    }

    public void MoveToOrigin(Action onMoveEnd)
    {
        isMovingOrigin = true;
        moveOriginTimer = 0;
        currentLocalPos = transform.localPosition;
        this.onMoveEnd = onMoveEnd;
    }
    private void MovingOrigin()
    {
        if (isMovingOrigin)
        {
            moveOriginTimer += Time.deltaTime * (1 / moveOriginTime);
            transform.localPosition = Vector3.Lerp(currentLocalPos, Vector3.zero, moveOriginTimer);
            if (moveOriginTimer >= 1)
            {
                isMovingOrigin = false;
                onMoveEnd?.Invoke();
            }
        }
    }

    private void MovingScaleOne()
    {
        if (isScaling)
        {
            scaleTimer += Time.deltaTime * (1 / scaleTime);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, scaleTimer);
            if (scaleTimer >= 1) isScaling = false;
        }
       
    }
    public void ScaleMoveToOne(float scale,float time=0.5f)
    {
        isScaling = true;
        scaleTimer = 0;
        scaleTime = time;
        startScale = Vector3.one * scale;

    }

    public string GetArticleInfo()
    {
        return article.GetArticleInfo();
    }
    public void OnDataChange(Article article)
    {
        if (article.count == 0)
        {
            //清空格子
            transform.parent.GetComponent<BagGrid>().ClearGrid();

        }
        else
        {  //更新数据
            SetArticle(article);
        }
        
    }

}
