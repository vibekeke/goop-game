using UnityEngine;

public class Dragggable : MonoBehaviour
{
    Vector2 oldPos = Vector2.zero, releaseVelocity;
    private Rigidbody2D rb;
    private bool grabbing = false, queueRelease = false;
    public float throwSpeedNerf = 10f; //Most natural way of making the throw less powerful lmao

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == gameObject)
            {
                grabbing = true;
                oldPos = rb.position;
                rb.bodyType = RigidbodyType2D.Kinematic; // Disable physics while dragging
            }
        }
        else if (Input.GetMouseButtonUp(0) && grabbing)
        {
            grabbing = false;
            queueRelease = true;
        }
    }

    void FixedUpdate()
    {
        if (grabbing)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            releaseVelocity = pos - oldPos; // raw delta per frame
            rb.MovePosition(pos);
            oldPos = pos;
        }

        if (queueRelease)
        {
            queueRelease = false;
            rb.bodyType = RigidbodyType2D.Dynamic;

            // Convert to velocity (units/second) and clamp it
            rb.linearVelocity = releaseVelocity / Time.fixedDeltaTime / throwSpeedNerf;
        }
    }
}