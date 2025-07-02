using UnityEngine;

//With throwing mechanics
public class Dragggable : MonoBehaviour
{
    Vector2 oldPos = Vector2.zero, releaseVelocity;
    private Rigidbody2D rb;
    bool grabbing = false, queueRelease =false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            grabbing = true;
            oldPos = transform.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            grabbing= false;
            queueRelease = true;
        }
    }

    private void FixedUpdate()
    {
        if (grabbing) //Get the velocity of the mousepos
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(pos);            
            releaseVelocity = rb.position - oldPos;
            oldPos = rb.position;

            
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -180f, 180f);
        }
        if (queueRelease)   //Add releaseVelocity to the rb
        {            
            queueRelease = false;
            rb.linearVelocity = (releaseVelocity / Mathf.Sqrt(Time.fixedDeltaTime));

        }

    }
}