using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public enum LoopScrollViewType
{
    Horizontal,
    Vertical
}


public class LoopScrollView : MonoBehaviour
{
    public GameObject childItemPrefab;
    private GridLayoutGroup contentLayoutGroup;
    private ContentSizeFitter sizeFitter;
    private RectTransform content;

    public LoopScrollViewType viewType = LoopScrollViewType.Vertical;

    private IDataApdater dataAdapter;

    private ISetLoopItemData setLoopItemData;

    public Action onMoveDataEnd;
    private void Awake()
    {
        Init();
    }
    





    private void Init()
    {
        content= transform.Find("Viewport/Content").GetComponent<RectTransform>();
        if (content == null) { throw new System.Exception("content 初始化失败！"); }
        contentLayoutGroup = content.GetComponent<GridLayoutGroup>();
        if(contentLayoutGroup==null) { throw new System.Exception("contentLayoutGroup 初始化失败！"); }
        sizeFitter = content.GetComponent<ContentSizeFitter>();
        if (sizeFitter == null) { throw new System.Exception("sizeFitter 初始化失败！"); }

        dataAdapter = GetComponent<IDataApdater>();

        if (dataAdapter == null)
        {
            dataAdapter = new DataAdapter();
        }

        setLoopItemData = GetComponent<ISetLoopItemData>();
        if (setLoopItemData == null)
        {
            throw new System.Exception("未实现 数据设置接口！");
        }
        
        ////----------------------测试模拟数据-----------------------------
        //List<LoopDataItem> loopDataItems = new List<LoopDataItem>();

        //for(int i = 0; i < 100; i++)
        //{
        //    loopDataItems.Add(new LoopDataItem(i));
        //}
        //dataAdapter.InitData(loopDataItems.ToArray());
        ////--------------------------------------------------
    }


    public void InitData(object[] datas,GameObject childItem)
    {

        if (childItem != null)
        {
            childItemPrefab = childItem;
        }
        contentLayoutGroup.enabled = true;
        sizeFitter.enabled = true;

        //隐藏所有子结点
        HideAllChildItem();
        //初始化数据
        dataAdapter.InitData(datas);


        OnAddHead();
        Invoke("EnableFalseGrid", 0.1f);
    }

    public void AddData(object[] datas)
    {
        dataAdapter.AddData(datas);
    }

    //获取一个子结点
    private GameObject GetChildItem()
    {
        //查找有没有被回收的子结点
        for(int i = 0; i < content.childCount; i++)
        {
            if (!content.GetChild(i).gameObject.activeSelf)
            {
                content.GetChild(i).gameObject.SetActive(true);
                return content.GetChild(i).gameObject;
            }
        }
        //如果没有创建一个
        GameObject childItem = Instantiate(childItemPrefab,content.transform);

        childItem.transform.localScale = Vector3.one;
        childItem.transform.localPosition = Vector3.zero;

        childItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        childItem.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);

        childItem.GetComponent<RectTransform>().sizeDelta = contentLayoutGroup.cellSize;

        LoopItem loopItem= childItem.AddComponent<LoopItem>();
        loopItem.onAddHead += OnAddHead;
        loopItem.onRemoveHead += OnRemoveHead;
        loopItem.onAddLast += OnAddLast;
        loopItem.onRemoveLast += OnRemoveLast;

        loopItem.SetLoopScrollViewType(viewType);
        return childItem;
    }

    private void OnAddHead()
    {
        object loopDataItem = dataAdapter.GetHeaderData();

        if (loopDataItem != null)
        {
            Transform first = FindFirst();

            GameObject obj = GetChildItem();
            obj.transform.SetAsFirstSibling();
            //设置数据
            setLoopItemData.SetData(obj, loopDataItem);


            //动态设置位置
            if (first != null)
            {
                switch (viewType)
                {
                    case LoopScrollViewType.Vertical:
                        obj.transform.localPosition = first.localPosition + new Vector3(0, contentLayoutGroup.cellSize.y + contentLayoutGroup.spacing.y, 0);
                        break;
                    case LoopScrollViewType.Horizontal:
                        obj.transform.localPosition = first.localPosition - new Vector3(contentLayoutGroup.cellSize.x + contentLayoutGroup.spacing.x, 0, 0);
                        break;
                }
                
            }

        }




    }
    private void OnRemoveHead()
    {
        if (dataAdapter.RemoveHeaderData())
        {
            Transform first = FindFirst();
            if (first != null)
            {
                first.gameObject.SetActive(false);
            }
        }

       
    }
    private void OnAddLast()
    {
        object loopDataItem = dataAdapter.GetLastData();
        if (loopDataItem != null)
        {
            Transform last = FindLast();

            GameObject obj = GetChildItem();
            obj.transform.SetAsLastSibling();

            setLoopItemData.SetData(obj, loopDataItem);

            switch (viewType)
            {
                case LoopScrollViewType.Vertical:
                    //动态设置位置
                    if (last != null)
                    {
                        obj.transform.localPosition = last.localPosition - new Vector3(0, contentLayoutGroup.cellSize.y + contentLayoutGroup.spacing.y, 0);
                    }

                    //要不要增加content高度
                    if (IsNeedAddContentHeight(obj.transform))
                    {
                        //对高度增加
                        content.sizeDelta += new Vector2(0, contentLayoutGroup.cellSize.y + contentLayoutGroup.spacing.y);
                    }
                    break;
                case LoopScrollViewType.Horizontal:
                    //动态设置位置
                    if (last != null)
                    {
                        obj.transform.localPosition = last.localPosition + new Vector3(contentLayoutGroup.cellSize.x + contentLayoutGroup.spacing.x,0 , 0);
                    }

                    //要不要增加content高度
                    if (IsNeedAddContentHeight(obj.transform))
                    {
                        //对高度增加
                        content.sizeDelta += new Vector2(contentLayoutGroup.cellSize.x + contentLayoutGroup.spacing.x,0);
                    }
                    break;
            }

        }
        else
        {
            //没有找到数据
            onMoveDataEnd?.Invoke();
        }

        

    }
    private void OnRemoveLast()
    {
        if (dataAdapter.RemoveLastData())
        {
            Transform last = FindLast();
            if (last != null)
            {
                last.gameObject.SetActive(false);
            }
        }

        
    }

    private Transform FindFirst()
    {
        for(int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                return content.GetChild(i);
            }
        }
        return null;
    }

    private Transform FindLast()
    {
        for (int i = content.childCount-1; i >= 0; i--)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                return content.GetChild(i);
            }
        }
        return null;
    }

    private void EnableFalseGrid()
    {
        contentLayoutGroup.enabled = false;
        sizeFitter.enabled = false;
    }

    private bool IsNeedAddContentHeight(Transform trans)
    {
        Vector3[] rectCorners = new Vector3[4];
        Vector3[] contentCorners = new Vector3[4];
        trans.GetComponent<RectTransform>().GetWorldCorners(rectCorners);
        content.GetWorldCorners(contentCorners);

        switch (viewType)
        {
            case LoopScrollViewType.Vertical:
                if (rectCorners[0].y < contentCorners[0].y)
                {
                    return true;
                }

                break;
            case LoopScrollViewType.Horizontal:
                if (rectCorners[3].x > contentCorners[3].x)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    //隐藏所有子结点（回收所有子结点）
    private void HideAllChildItem()
    {
        for(int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
    }


    //设置数据
    
    //public void SetData(GameObject childItem,object loopData)
    //{
    //    childItem.transform.Find("Text").GetComponent<Text>().text ="任务"+ ((LoopDataItem)loopData).id.ToString();
    //}
}
