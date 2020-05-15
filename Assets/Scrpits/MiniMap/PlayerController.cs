using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;

    float h;
    float v;

    Vector3 move;
    public float moveSpeed;
    public float rotateSpeed;
    Animator animator;

    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        move = new Vector3(h, 0, v);
        Debug.Log(move);
        move = transform.TransformDirection(move);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2f;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /=2f;
        }
        rb.MovePosition(transform.position + move * Time.deltaTime * moveSpeed);
        if (h == 0 && v == 0)
        {
            animator.SetInteger("animation", 1);//idle
        }
        else if (h == 0 && v != 0)
        {
            
            if (v > 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetInteger("animation", 10);
                }
                else
                {
                    animator.SetInteger("animation", 6);
                }
                
            }
            else
            {
                animator.SetInteger("animation", 9);
            }
        }
        else if (h != 0 && v == 0)
        {
           
            if (h < 0)
            {
                animator.SetInteger("animation", 7);
            }
            else
            {
                animator.SetInteger("animation", 8);
            }
        }
        else
        {
            animator.SetInteger("animation", 6);
        }
    }

    private void Rotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            //
            //Input.mousePosition-mousePos
            transform.eulerAngles += new Vector3(0, (Input.mousePosition.x - mousePos.x) * Time.deltaTime * rotateSpeed);
            //transform.rotation = Quaternion.Euler(, 0));
            mousePos = Input.mousePosition;
        }
    }
    public void OnJoyStickVelocityChange(Vector2 velocity)
    {
       // Debug.Log(velocity);
        h = velocity.x;
        v = velocity.y;
    }
}
