using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAdapter:IDataApdater
{
    //保存的所有数据
    public List<object> allData = new List<object>();
    //保存当前显示的数据
    public LinkedList<object> currentData = new LinkedList<object>();

    public void InitData(object[] t)
    {
        allData.Clear();
        currentData.Clear();
        allData.AddRange(t);
    }

    public void InitData(List<object> t)
    {
        InitData(t.ToArray());
    }

    public void AddData(object[] t)
    {
        allData.AddRange(t);
    }

    public void AddData(List<object> t)
    {
        allData.AddRange(t.ToArray());
    }

   public object GetHeaderData()
    {
        //判断总数据的数量
        if (allData.Count == 0)
            return null;


        if (currentData.Count == 0)
        {
            currentData.AddFirst(allData[0]);
            return allData[0];
        }

        //当前正在显示的第一个数据的上一个
        object t = currentData.First.Value;
        int index= allData.IndexOf(t);
        if (index == 0) return null;
        object header = allData[index - 1];
        currentData.AddFirst(header);
        return header;
    }

    public bool RemoveHeaderData()
    {
        if (currentData.Count == 0||currentData.Count==1) return false;

        currentData.RemoveFirst();
        return true;
    }

    public object GetLastData()
    {
        if (allData.Count == 0)
            return null;


        if (currentData.Count == 0)
        {
            currentData.AddLast(allData[0]);
            return allData[0];
        }
        object last = currentData.Last.Value;
        int index = allData.IndexOf(last);
        if (index != (allData.Count - 1))
        {
            object currentLast = allData[index + 1];
            currentData.AddLast(currentLast);
            return currentLast;
        }

        return null;
    }

    public bool RemoveLastData()
    {
        if (currentData.Count == 0||currentData.Count==1) return false;
        currentData.RemoveLast();
        return true;
    }
}
