using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickPanel : ViewBase
{
    public JoyStick joyStick;
    private CanvasGroup joyStickGroup;
    private Canvas canvas;

    private void Start()
    {
        joyStick.JoyStickHandle.onBeginDrag += OnBeginDrag;
        joyStick.JoyStickHandle.onEndDrag += OnEndDrag;
        joyStickGroup = joyStick.GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public override void Show()
    {
        Invoke("ShowDelay", 1.5f);

    }
    private void ShowDelay()
    {
        base.Show();
    }

    public void OnBeginDrag()
    {
        joyStickGroup.alpha = 1;
    }
    public void OnEndDrag()
    {
        joyStickGroup.alpha = 0.4f;
    }
    public void OnJoystickAreaClick()
    {
        //joyStick.transform.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
       joyStick.transform.position= canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, canvas.planeDistance));
    }
}
