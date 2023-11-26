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
    float rotateSpeed = 25f;

    private int[] layers; 
    public float rayDistance = 10f;
    private float[] inputRays = new float[5];
    private RaycastHit2D[] rays = new RaycastHit2D[5];
    // Start is called before the first frame update

    private float totalDistance = 0;
    private Vector3 previousLoc;

    private List<Collider> colliders = new List<Collider>();
    private float treat = 15f;
    void Start()
    {
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
            castRays();

            float[] outPut = net.feedForward(inputRays);

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
        net.addFitness(15 - colliders.Count);
        speed = 0;
    }
    void RecordDistance()
    {
        totalDistance += Vector3.Distance(transform.position, previousLoc);
        previousLoc = transform.position;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (colliders.Contains(other) == false) 
        {
            colliders.Add(other);
            net.addFitness(colliders.Count + 1);
        }
        else if (colliders.Count  >= 15)
        {
            colliders.Clear();
            net.addFitness(colliders.Count * 3);
        }
    }*/

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
