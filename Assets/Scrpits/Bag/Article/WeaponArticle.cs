using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WeaponArticle : Article
{
    public int attack;
    public WeaponArticle(string name, string spritePath, ArticleType articleType, int count,int attack) :
         base(name, spritePath, articleType, count)
    {
        this.attack = attack;
    }

    public override string GetArticleInfo()
    {
        string info = base.GetArticleInfo();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(info + "\n");

        stringBuilder.Append("<color=#FFE500>");
        stringBuilder.Append("攻击力：").Append(attack);
        stringBuilder.Append("</color>");

        return stringBuilder.ToString();
    }
    public override void UseArticle()
    {
        base.UseArticle();
        BagPanel._instance.equipWeapon.Equip(this);
    }
}
