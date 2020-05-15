using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleInfo :ViewBase
{

    private RectTransform rectInfo;
    private Text text;

    private Vector3[] infoCorners;
    private Vector3[] screenCorners;

    private void Awake()
    {
        infoCorners = new Vector3[4];
        screenCorners = new Vector3[4];
        text = GetComponentInChildren<Text>();
        rectInfo = transform.Find("Text").GetComponent<RectTransform>();
        //text = rectInfo.GetComponentInChildren<Text>();
        Hide();
    }


   
    private void Update()
    {
        rectInfo.anchoredPosition = Input.mousePosition;
        
    }
    private void LateUpdate()
    {
        ListenerCorners();
    }
    public void SetShowInfo(string info)
    {
        if (text != null)
        {
            text.text = info;
        }
        
    }
    public override void Show()
    {
        base.Show();
        rectInfo.pivot = new Vector2(-0.1f, 1.15f);
    }
    public void ListenerCorners()
    {
         rectInfo.GetWorldCorners(infoCorners);
        GetComponent<RectTransform>().GetWorldCorners(screenCorners);

        Vector2 pivot = rectInfo.pivot;

        if (infoCorners[0].x < screenCorners[0].x)
        {
            //左边超界
            pivot.x = -0.1f;
        }
        if (infoCorners[3].x > screenCorners[3].x)
        {
            //右边超界
            pivot.x = 1.15f;
        }
        if (infoCorners[1].y > screenCorners[1].y)
        {
            //上边超界
            pivot.y = 1.15f;
        }
        if (infoCorners[0].y < screenCorners[0].y)
        {
            //下边超界
            pivot.y = -0.1f;
        }
        rectInfo.pivot = pivot;
    }
}
