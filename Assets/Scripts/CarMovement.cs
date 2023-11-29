using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CarMovement : MonoBehaviour
{
    private bool build = true;
    private NeuralNetWork net;

    float speed = 2f;
    float rotateSpeed = 50f;
    float currspeed = 0;

    public Rigidbody2D rb;

    private int[] layers; 
    public float rayDistance = 10f;
    private float[] inputs = new float[6];
    private RaycastHit2D[] rays = new RaycastHit2D[5];
    // Start is called before the first frame update

    private float totalDistance = 0;
    private Vector3 previousLoc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        net = new NeuralNetWork(layers);
    }

    public void initCar(NeuralNetWork net)
    {
        this.net = net;
        build = true;
    }

    private void FixedUpdate()
    {
        castRays();
      
        if (build)
        {
            currspeed = rb.velocity.magnitude;
            castRays();

            float[] outPut = net.feedForward(inputs);

            transform.position += outPut[0] * transform.up * Time.deltaTime * speed;

            transform.Rotate(0, 0, outPut[1] * Time.deltaTime * rotateSpeed);

            RecordDistance();
            net.setFitness(totalDistance);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colission");
        build = false;
        net.setFitness(net.getFitness() * 0.7f);
        speed = 0;
    }
    void RecordDistance()
    {
        Vector2 currentMovement = (transform.position - previousLoc);
        if (currentMovement.y > 0)
        {
            totalDistance += Vector2.Distance(transform.position, previousLoc);
            previousLoc = transform.position;
        }
        if (currentMovement.y > 0)
        {
            totalDistance -= Vector2.Distance(transform.position, previousLoc);
            previousLoc = transform.position;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        net.addFitness(-120);
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
        loadInputs();
    }

    public void loadInputs() 
    {
        for (int i = 0; i < rays.Length; i++)
        {
            inputs[i] = rays[i].distance;
        }
        inputs[5] = currspeed;
    }
}
