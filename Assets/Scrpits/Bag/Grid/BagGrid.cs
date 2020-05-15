using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagGrid : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    protected ArticleItem articleItem;
    protected Image bagImg;
    protected Color defaultBagImgColor;

   
    public ArticleItem ArticleItem
    {
        get { return articleItem; }
    }


    protected virtual void Awake()
    {
        bagImg = GetComponent<Image>();
        defaultBagImgColor = bagImg.color;
    }

    public void SetArticleItem(ArticleItem articleItem)
    {
        this.articleItem = articleItem;
        this.articleItem.gameObject.SetActive(true);
        this.articleItem.transform.SetParent(transform);

        //this.articleItem.transform.localPosition = Vector3.zero;
        this.articleItem.MoveToOrigin(()=> {
            this.articleItem.GetComponent<RectTransform>().offsetMax = new Vector2(-5, -5);
            this.articleItem.GetComponent<RectTransform>().offsetMin = new Vector2(5, 5);
        });
        this.articleItem.transform.localScale = Vector3.one;
        //修改rotation
        this.articleItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
    }

    public void ClearGrid()
    {
        articleItem.gameObject.SetActive(false);
        //articleItem.transform.parent = null;
        articleItem = null;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (BagPanel._instance.currentDragArticle != null)
        {
            BagPanel._instance.currentHoverGrid = this;
            bagImg.color = new Color(1, 1, 0.23f, 0.4f);
            
        }
        if (articleItem != null)
        {
            //显示当前格子物品信息
            BagPanel._instance.articleInfo.Show();
            BagPanel._instance.articleInfo.SetShowInfo(articleItem.GetArticleInfo());
        }
       


    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        BagPanel._instance.currentHoverGrid = null;
        bagImg.color = defaultBagImgColor;
        //隐藏当前格子物品信息
        BagPanel._instance.articleInfo.Hide();
    }
    
    public virtual void DragToThisGrid(ArticleItem articleItem)
    {
        //清空以前格子
        BagGrid lastGrid = articleItem.transform.parent.GetComponent<BagGrid>();
        if (this.articleItem == null)
        {
            lastGrid.ClearGrid();
            SetArticleItem(articleItem);
            
        }
        else
        {
            //交换
            lastGrid.SetArticleItem(this.articleItem);
            SetArticleItem(articleItem);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId == -2)
        {
            if (articleItem != null)
            {
                articleItem.Article.UseArticle();
            }
            
        }
    }
}
