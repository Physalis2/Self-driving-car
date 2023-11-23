using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 1f;

    public float rayDistance = 10f;

    public float[] inputRays = new float[5];
    public RaycastHit2D[] rays = new RaycastHit2D[5];
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        castRays();

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

    public void castRays()
    {
        RaycastHit2D rayLeft = Physics2D.Raycast(transform.position, -transform.right, rayDistance);
        RaycastHit2D rayLeftFront = Physics2D.Raycast(transform.position, (transform.up - transform.right).normalized, rayDistance);

        RaycastHit2D rayFront = Physics2D.Raycast(transform.position, transform.up, rayDistance);

        RaycastHit2D rayRightFront = Physics2D.Raycast(transform.position, (transform.up + transform.right).normalized, rayDistance);
        RaycastHit2D rayRight = Physics2D.Raycast(transform.position, transform.right, rayDistance);

        Debug.DrawRay(transform.position, -transform.right * rayDistance, Color.green);
        Debug.DrawRay(transform.position, (transform.up - transform.right).normalized * rayDistance, Color.green);
        Debug.DrawRay(transform.position, transform.up * rayDistance, Color.green);
        Debug.DrawRay(transform.position, (transform.up + transform.right).normalized * rayDistance, Color.green);
        Debug.DrawRay(transform.position, transform.right * rayDistance, Color.green);

        rays[0] = rayLeft;
        rays[1] = rayLeftFront;
        rays[2] = rayFront;
        rays[3] = rayRightFront;
        rays[4] = rayRight;

        int count = 0;

        foreach (RaycastHit2D rayHit in rays)
        {
            inputRays[count] = rayHit.distance;
            Debug.Log(count);
            count++;
        }

        count = 0;
    }
}
