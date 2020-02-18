using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    public float diagonalSpeed;
    public Camera cam;

    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }


    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(movement.x) > 0.5f && Mathf.Abs(movement.y) > 0.5f)
            currentSpeed = moveSpeed * diagonalSpeed;
        else
            currentSpeed = moveSpeed;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90.0f;
        rb.rotation = angle;
    }
}
