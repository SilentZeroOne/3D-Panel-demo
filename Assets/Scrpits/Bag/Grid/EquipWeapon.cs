using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipWeapon : EquipGrid
{
    protected override void Awake()
    {
        base.Awake();
        currentEquipType = ArticleType.Weapon;
    }

    //public override void DragToThisGrid(ArticleItem articleItem)
    //{
    //    base.DragToThisGrid(articleItem);
    //    if (articleItem.Article.articleType == ArticleType.Weapon)
    //    {
    //        //处理装备武器的逻辑
    //        Equip(articleItem.Article);
    //        articleItem.transform.parent.GetComponent<BagGrid>().UseArticle();
    //    }   
    //        articleItem.MoveToOrigin(null);      
    //}
    public override void Equip(Article article)
    {
        base.Equip(article);
        //处理武器装备效果
        WeaponArticle weaponArticle = (WeaponArticle)article;
        BagPanel._instance.ShowTip("攻击+" + weaponArticle.attack);
    }
 
}
