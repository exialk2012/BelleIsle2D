using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D playerRB;
    public int moveSpeed;
    Vector2 movement = new Vector2();
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        playerRB.MovePosition(playerRB.position+ movement * moveSpeed * Time.deltaTime);
    }
}
