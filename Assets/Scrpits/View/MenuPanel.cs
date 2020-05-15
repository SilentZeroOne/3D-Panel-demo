using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : ViewBase
{

    public AnnouncementPanel announcementPanel;
    public FriendPanel friendPanel;
    public TaskPanel taskPanel;
    public BagPanel bagPanel;
    public MiniMapPanel miniMapPanel;
    public JoyStickPanel joyStickPanel;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnnouncementBtnClick()
    {
        announcementPanel.Show();
    }
    public void OnFriendBtnClick()
    {
        friendPanel.Show();
    }
    public void OnMissionBtnClick()
    {
        taskPanel.Show();
    }
    public void OnBagBtnClikc()
    {
        bagPanel.Show();
        Hide();
    }

    public void OnStartBtnClikc()
    {
        miniMapPanel.Show();
        joyStickPanel.Show();
        Hide();
    }
    public override void Hide()
    {
        //base.Hide();
        anim.SetBool("isShow", false);
        
    }
    public override void Show()
    {
       // base.Show();
        anim.SetBool("isShow", true);
    }
}
