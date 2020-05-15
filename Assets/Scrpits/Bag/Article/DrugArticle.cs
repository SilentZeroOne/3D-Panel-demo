using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class DrugArticle : Article
{
    public int hp;
    public DrugArticle(string name, string spritePath, ArticleType articleType, int count,int hp) :
         base(name, spritePath, articleType, count)
    {
        this.hp = hp;
    }

    public override string GetArticleInfo()
    {
        string info= base.GetArticleInfo();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(info+"\n");

        stringBuilder.Append("<color=#79F33B>");
        stringBuilder.Append("恢复血量：").Append(hp);
        stringBuilder.Append("</color>");

        return stringBuilder.ToString();
    }
    public override void UseArticle()
    {
        base.UseArticle();
        BagPanel._instance.ShowTip("恢复血量+" + hp);
    }
}
