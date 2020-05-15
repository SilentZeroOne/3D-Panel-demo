using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipShoes : EquipGrid
{

    protected override void Awake()
    {
        base.Awake();
        currentEquipType = ArticleType.Shoes;
    }
    //public override void DragToThisGrid(ArticleItem articleItem)
    //{
    //    base.DragToThisGrid(articleItem);
    //    if (articleItem.Article.articleType == ArticleType.Shoes)
    //    {
    //        //处理装备鞋子的逻辑
    //        Equip(articleItem.Article);
    //        //使用物品的方法
    //        articleItem.transform.parent.GetComponent<BagGrid>().UseArticle();
    //    }       
    //        articleItem.MoveToOrigin(null);       
    //}

    public override void Equip(Article article)
    {
        base.Equip(article);
        //处理装备鞋子效果
        ShoesArticle shoesArticle = (ShoesArticle)article;
        BagPanel._instance.ShowTip("移速+" + shoesArticle.moveSpeed);
    }
}

