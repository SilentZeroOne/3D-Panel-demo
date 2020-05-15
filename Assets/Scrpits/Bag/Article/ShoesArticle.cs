using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShoesArticle : Article
{
    public int moveSpeed;
    public ShoesArticle(string name, string spritePath, ArticleType articleType, int count,int moveSpeed) :
         base(name, spritePath, articleType, count)
    {
        this.moveSpeed = moveSpeed;
    }

    public override string GetArticleInfo()
    {
        string info = base.GetArticleInfo();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(info + "\n");

        stringBuilder.Append("<color=#2D87FF>");
        stringBuilder.Append("移速：").Append(moveSpeed);
        stringBuilder.Append("</color>");

        return stringBuilder.ToString();
    }
    public override void UseArticle()
    {
        base.UseArticle();
        BagPanel._instance.equipShoes.Equip(this);
    }
}
