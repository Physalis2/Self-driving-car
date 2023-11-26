using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMovement : MonoBehaviour
{
    private bool build = false;
    private NeuralNetWork net;

    public float speed = 1f;

    private int[] layers = new int[] { 5, 4, 4, 2 };
    public float rayDistance = 10f;
    public float[] inputRays = new float[5];
    public RaycastHit2D[] rays = new RaycastHit2D[5];
    // Start is called before the first frame update
    void Start()
    {
        net = new NeuralNetWork(layers);
    }

    private void FixedUpdate()
    {
        if (build)
        {
            castRays();

            float[] outPut = net.feedForward(inputRays);

            transform.position += outPut[1] * transform.up * Time.deltaTime;

            transform.Rotate(0, 0, outPut[0]);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colission");
        build = true;
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

        for (int i = 0; i < rays.Length; i++)
        {
            inputRays[i] = rays[i].distance;
        }
    }
}
