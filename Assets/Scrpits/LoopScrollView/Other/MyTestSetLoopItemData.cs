using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTestSetLoopItemData : MonoBehaviour, ISetLoopItemData
{
    public void SetData(GameObject childItem, object loopData)
    {
        childItem.transform.Find("Text").GetComponent<Text>().text = "任务" + ((LoopDataItem)loopData).id.ToString();
    }
}
