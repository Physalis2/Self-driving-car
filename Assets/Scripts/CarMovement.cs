using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
                transform.position += speed * transform.up * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.S))
        {
                transform.position -= speed * transform.up * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colission");
        speed = 0;
    }
}
