using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : ViewBase
{
    public LoopScrollView scrollView;

    private bool isGetData;
    private void Start()
    {
        LoopDataItem[] loopDataItem = new LoopDataItem[100];
        for(int i = 0; i < loopDataItem.Length; i++)
        {
            loopDataItem[i] = new LoopDataItem(i);
        }


        scrollView.InitData(loopDataItem,null);
        scrollView.onMoveDataEnd += OnMoveDataEnd;
    }

    public void OnMoveDataEnd()
    {
        //向服务端发送请求获取数据
        Debug.Log("去获取数据");
        if (!isGetData) 
          StartGetData();
    }

    public void StartGetData() {
        isGetData = true;
        Invoke("OnGetDataSuccess", 1);
    }

    public void OnGetDataSuccess()
    {
        LoopDataItem[] loopDataItem = new LoopDataItem[100];
        for (int i = 0; i < loopDataItem.Length; i++)
        {
            loopDataItem[i] = new LoopDataItem(i+100);
        }
        scrollView.AddData(loopDataItem);
        isGetData = false;
    }
}
