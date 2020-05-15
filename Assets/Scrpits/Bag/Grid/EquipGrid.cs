using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipGrid :BagGrid
{
    //public ArticleType articleType;
    protected Article article;
    private Image articleSprite;

    protected ArticleType currentEquipType;

    protected override void Awake()
    {
        base.Awake();
        articleSprite = transform.Find("articleSprite").GetComponent<Image>();
    }

    public override void DragToThisGrid(ArticleItem articleItem)
    {
        if (articleItem.Article.articleType == currentEquipType)
        {
            //Equip(articleItem.Article);
            articleItem.Article.UseArticle();
        }
        else
        {
            BagPanel._instance.ShowTip("类型错误，无法装备");      
        }
        articleItem.MoveToOrigin(null);
    }

    public virtual void Equip(Article article)
    {

        UnEnquip();
        //设置数据
        this.article = article;
        //显示图片
        articleSprite.sprite = Resources.Load<Sprite>(this.article.spritePath);
        articleSprite.gameObject.SetActive(true);


    }
    public virtual void UnEnquip()
    {
        if (article != null)
        {
           //把装备放回背包           
            BagPanel._instance.AddArticleData(article);
            //卸掉当前装备
            article = null;
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (BagPanel._instance.currentDragArticle != null)
        {
            if (BagPanel._instance.currentDragArticle.Article.articleType == currentEquipType)
            {
                bagImg.color = Color.green;
            }
            else
            {
                bagImg.color = Color.red;
            }

        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }
}
