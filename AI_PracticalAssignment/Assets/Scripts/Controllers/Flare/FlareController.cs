using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareController : MonoBehaviour
{

    public float moveSpeed;
    public float rotationSpeed;
    public float destroyTime;

    Rigidbody2D rb;

    public float maxSize = 2f;
    public float growingRate = .5f;

    bool maxSizeReached = false;
    bool targetReached = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * moveSpeed * Time.deltaTime;

        Debug.Log(moveSpeed);
    }


    void FixedUpdate()
    {
        if (!targetReached)
            transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);

        if (!maxSizeReached)
            SizeGrow();
        else
            SizeShrink();

        if (targetReached)
            FlareOnFloor();

    }

    void SizeGrow()
    {
        float sizeX = transform.localScale.x;
        float sizeY = transform.localScale.y;

        sizeX += growingRate * Time.deltaTime;
        sizeY += growingRate * Time.deltaTime;

        if (sizeX >= maxSize && sizeY >= maxSize)
        {
            sizeX = maxSize;
            sizeY = maxSize;

            maxSizeReached = true;
        }

        transform.localScale = new Vector2(sizeX, sizeY);
    }

    void SizeShrink()
    {
        float sizeX = transform.localScale.x;
        float sizeY = transform.localScale.y;

        sizeX -= growingRate * Time.deltaTime;
        sizeY -= growingRate * Time.deltaTime;

        if (sizeX <= 1 && sizeY <= 1)
        {
            sizeX = 1;
            sizeY = 1;

            targetReached = true;
        }

        transform.localScale = new Vector2(sizeX, sizeY);
    }

    public void FlareOnFloor()
    {
        transform.tag = "Flare";
        rb.velocity = Vector2.zero;
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, l_EulerAngles.z);
        Invoke("DestroyObject", destroyTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
            DestroyObject();
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
