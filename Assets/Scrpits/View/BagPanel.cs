using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel:ViewBase
{
    #region 数据模拟
    private List<Article> articles = new List<Article>();
    private List<GameObject> articleItems = new List<GameObject>();

    public BagGrid currentHoverGrid;//当前所处格子
    public ArticleItem currentDragArticle;//当前拖拽的物品

    public static BagPanel _instance;
    #endregion

    public GameObject articleItemPrefab;

    private BagGrid[] bagGrids;
    public MenuPanel menuPanel;
    public ArticleInfo articleInfo;

    private Text tipText;

    public EquipWeapon equipWeapon;
    public EquipShoes equipShoes;

    Animator anim;
    private void Awake()
    {
        _instance = this;
        InitArticleData();
        bagGrids = transform.Find("Right").GetComponentsInChildren<BagGrid>();
        tipText = transform.Find("Tip").GetComponent<Text>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //LoadData();
        StartCoroutine(LoadDataWithAnim());
    }
    public override void Hide()
    {
       // base.Hide();
        anim.SetBool("isShow", false);
        Camera.main.GetComponent<Animator>().SetBool("isShowBag", false);
        Invoke("HideDelay", 1f);
        
    }
    public override void Show()
    {
        //base.Show();
        Camera.main.GetComponent<Animator>().SetBool("isShowBag", true);
        Invoke("ShowDelay", 1.5f);
    }

    public void ShowDelay()
    {
        base.Show();
    }
    public void HideDelay()
    {
        base.Hide();
        menuPanel.Show();
    }

    public void InitArticleData()
    {
        articles.Add(new WeaponArticle("枪", "Sprite/weapon1", ArticleType.Weapon, 1,100));
        articles.Add(new WeaponArticle("刀", "Sprite/weapon2", ArticleType.Weapon, 2,200));
        articles.Add(new WeaponArticle("戟", "Sprite/weapon3", ArticleType.Weapon, 3,500));
        articles.Add(new WeaponArticle("仙剑", "Sprite/weapon4", ArticleType.Weapon, 4,1000));

        articles.Add(new DrugArticle("饺子", "Sprite/drug1", ArticleType.Drug, 1,100));
        articles.Add(new DrugArticle("鸡肉", "Sprite/drug2", ArticleType.Drug, 2,200));
        articles.Add(new DrugArticle("药丸", "Sprite/drug3", ArticleType.Drug, 3,500));
        articles.Add(new DrugArticle("仙丹", "Sprite/drug4", ArticleType.Drug, 4,1000));

        articles.Add(new ShoesArticle("草鞋", "Sprite/shoe1", ArticleType.Shoes, 1,10));
        articles.Add(new ShoesArticle("布鞋", "Sprite/shoe2", ArticleType.Shoes, 2,20));
        articles.Add(new ShoesArticle("板鞋", "Sprite/shoe3", ArticleType.Shoes, 3,50));
        articles.Add(new ShoesArticle("皮鞋", "Sprite/shoe4", ArticleType.Shoes, 4,100));

        articles.Add(new BookArticle("降龙十八掌", "Sprite/book1", ArticleType.Book, 1));
        articles.Add(new BookArticle("九阴真经", "Sprite/book2", ArticleType.Book, 2));
        articles.Add(new BookArticle("如来神掌", "Sprite/book3", ArticleType.Book, 3));
        articles.Add(new BookArticle("葵花宝典", "Sprite/book4", ArticleType.Book, 4));

    }

    //加载所有数据
    public void LoadData()
    {
        HideAllArticleItem();
        for(int i = 0; i < articles.Count; i++)
        {
            GetBagGrid().SetArticleItem(LoadArticleItem(articles[i]));
        }
    }


    public void LoadData(ArticleType articleType)
    {
        HideAllArticleItem();
        for (int i = 0; i < articles.Count; i++)
        {
            if (articles[i].articleType == articleType)
            {
                GetBagGrid().SetArticleItem(LoadArticleItem(articles[i]));
            }
            
        }
    }

    public IEnumerator LoadDataWithAnim()
    {
        HideAllArticleItem();
        yield return null;
        for (int i = 0; i < articles.Count; i++)
        {
            ArticleItem articleItem = LoadArticleItem(articles[i]);
            GetBagGrid().SetArticleItem(articleItem);
            //修改大小
            articleItem.ScaleMoveToOne(0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator LoadDataWithAnim(ArticleType articleType)
    {
        HideAllArticleItem();
        yield return null;
        for (int i = 0; i < articles.Count; i++)
        {
            if (articles[i].articleType == articleType)
            {
                ArticleItem articleItem = LoadArticleItem(articles[i]);
                GetBagGrid().SetArticleItem(articleItem);
                //修改大小
                articleItem.ScaleMoveToOne(0);
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    //获得一个空grid
    public BagGrid GetBagGrid()
    {
        for (int i = 0; i < bagGrids.Length; i++)
        {
            if (bagGrids[i].ArticleItem == null)
            {
                return bagGrids[i];
            }
        }
        return null;
    }

    public ArticleItem LoadArticleItem(Article article)
    {
        GameObject obj = GetArticleItem();

        ArticleItem articleItem = obj.GetComponent<ArticleItem>();
        articleItem.SetArticle(article);
        return articleItem;
    }


    public GameObject GetArticleItem()
    {
        for(int i = 0; i < articleItems.Count; i++)
        {
            if (articleItems[i].activeSelf == false)
            {
                articleItems[i].SetActive(true);
                return articleItems[i];
            }
        }
        return Instantiate(articleItemPrefab);
    }

    public void HideAllArticleItem()
    {
        for (int i = 0; i < bagGrids.Length; i++)
        {
            if (bagGrids[i].ArticleItem != null)
            {
                bagGrids[i].ClearGrid();
            }
        }
    }

    //移除物品数据
    public void RemoveArticleData(Article article)
    {
        articles.Remove(article);
    }

    public void AddArticleData(Article article)
    {
        article.count++;
        if (articles.Contains(article))
        {
            
            //更新显示
            for(int i = 0; i < bagGrids.Length; i++)
            {
                if (bagGrids[i].ArticleItem != null)
                {
                    if (bagGrids[i].ArticleItem.Article == article)
                    {
                        bagGrids[i].ArticleItem.SetArticle(article);
                        bagGrids[i].ArticleItem.ScaleMoveToOne(1.2f);
                        break;
                    }
                }
               
            }
        }
        else
        {
            articles.Add(article);
            //显示
            ArticleItem articleItem = GetArticleItem().GetComponent<ArticleItem>();
            articleItem.SetArticle(article);
            GetBagGrid().SetArticleItem(articleItem);
            articleItem.ScaleMoveToOne(1.2f);
        }
    }

    public void ShowTip(string message)
    {
        HideTip();
        CancelInvoke("HideTip");
        tipText.text = message;
        tipText.gameObject.SetActive(true);
        Invoke("HideTip", 2);
    }

    public void HideTip()
    {
        tipText.gameObject.SetActive(false);
    }

    #region 点击事件
    public void OnAllToggleValueChange(bool v)
    {
        if (v) {
            StartCoroutine(LoadDataWithAnim()); 
        }      
    }
    public void OnWeaponToggleValueChange(bool v)
    {
        if (v)
        {
            StartCoroutine(LoadDataWithAnim(ArticleType.Weapon));
        }
    }
    public void OnDrugToggleValueChange(bool v)
    {
        if (v)
        {
            StartCoroutine(LoadDataWithAnim(ArticleType.Drug));
        }
    }
    public void OnShoeToggleValueChange(bool v)
    {
        if (v) {
            StartCoroutine(LoadDataWithAnim(ArticleType.Shoes));
        }
        
    }
    public void OnBookToggleValueChange(bool v)
    {
        if (v) {
           StartCoroutine(LoadDataWithAnim(ArticleType.Book)); 
        } 
    }

    #endregion
}
