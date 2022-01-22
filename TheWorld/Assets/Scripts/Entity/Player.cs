using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float moveSpeed = 1.0f;                  //	Default movement speed of character


    public override void Init()
    {
        base.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    private void Move(float dx, float dy)
    {
        _Rigidbody2D.velocity = new Vector2(dx * moveSpeed, dy * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //	Take in a float value from Input axes
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //	Apply the values in here.
        Move(x, y);
    }
}
