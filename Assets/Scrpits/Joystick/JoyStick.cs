using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class OnVelocityChange : UnityEvent<Vector2>
{

}


public class JoyStick : MonoBehaviour
{
    public float maxDistance;
    public float minDistance;
    JoyStickHandle joyStickHandle;
    Transform direction;


    public OnVelocityChange onVelocityChange;

    //public Action onBeginDragHandle;
    //public Action onDragHandle;
    //public Action onEndDragHandle;

    public JoyStickHandle JoyStickHandle
    {
        get { return joyStickHandle; }
    }
    private void Awake()
    {
        Initialized();
    }
    private void Update()
    {
        onVelocityChange?.Invoke(joyStickHandle.velocity);
        UpdateDirection();
    }

    public void UpdateDirection()
    {
        direction.eulerAngles = new Vector3(0, 0, joyStickHandle.angle);
        if (joyStickHandle.velocity.Equals(Vector2.zero))
        {
            direction.gameObject.SetActive(false);
        }
        else
        {
            direction.gameObject.SetActive(true);
        }
    }
    private void Initialized()
    {
        joyStickHandle = GetComponentInChildren<JoyStickHandle>();
        if (joyStickHandle == null) throw new System.Exception("未获取到JoystickHandle");

        direction = transform.Find("direction");
    }

}
