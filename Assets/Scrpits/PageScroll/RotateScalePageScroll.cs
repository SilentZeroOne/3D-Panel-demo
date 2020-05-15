using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScalePageScroll : ScalePageScroll
{
    public float rotation;

    protected override void Update()
    {
        base.Update();
        ListenerItemRotation();
    }

    public void ListenerItemRotation()
    {
        if (nextPage == lastPage) return;

        float precent = (rect.horizontalNormalizedPosition - pages[lastPage]) / (pages[nextPage] - pages[lastPage]);

        if (nextPage > currentIndex)
        {
            items[lastPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0,- rotation, 0), precent));
            items[nextPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), 1 - precent));

        }
        else
        {
            items[lastPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, -rotation, 0), precent));
            items[nextPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), 1 - precent));
        }


        for (int i = 0; i < items.Length; i++)
        {
            if(i!=nextPage&&i!=lastPage)
            {
                if (i < currentIndex)
                {
                    items[i].transform.localRotation = Quaternion.Euler(0, -rotation, 0);
                }
                else if (i == currentIndex)
                {
                    items[i].transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (i > currentIndex)
                {
                    items[i].transform.localRotation = Quaternion.Euler(0, rotation, 0);
                }
            }
            
        }
    }

}
