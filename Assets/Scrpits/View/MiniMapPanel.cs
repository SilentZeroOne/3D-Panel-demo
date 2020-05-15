using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniMapPanel : ViewBase
{
    public GameObject vCam;

    private void Awake()
    {
        //vCam = GameObject.Find("CM vcam1");
    }
    public override void Show()
    {
       // Camera.main.GetComponent<Animator>().SetBool("isShowMiniMap", true);
        vCam.GetComponent<Animator>().SetBool("isShowMiniMap", true);
        vCam.GetComponent<CinemachineVirtualCamera>().m_LookAt = GameObject.Find("Chest_M").transform;
        Invoke("ShowDelay", 1.5f);
    }

    public void ShowDelay()
    {
        base.Show();
        GameObject.Find("Riko").GetComponent<PlayerController>().enabled = true;
    }
}
