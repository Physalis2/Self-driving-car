using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject carPrefab;

    public bool isTraining = false;
    public int gen = 0;
    public int amount = 50;

    public GameObject spawnpoint;

    private List<NeuralNetWork> nets; //Liste meiner NN
    private List<CarMovement> cars = null; //List der autos

    private int[] layers = new int[] { 6, 12, 12, 12, 2 }; //5 inputs 2 outputs, 5 direction gas or turn  

    void Timer()
    {
        isTraining = false;
    }

    void Update()
    {
        if (isTraining == false)
        {
            if (gen == 0)
            {
                creatNewtWorks();
                Debug.Log("creat NN");
            }
            else
            {
                nets.Sort();

                for (int i = 0; i < nets.Count / 2; i++)
                {
                    nets[i] = new NeuralNetWork(nets[i + amount / 2]);
                    nets[i].mutate();

                    nets[i + amount / 2] = new NeuralNetWork(nets[i + amount / 2]);
                    nets[i + amount / 2].mutate();
                }

                for (int i = 0; i < amount; i++)
                {
                    nets[i].setFitness(0f);
                }
            }
            gen ++;

            Debug.Log(gen);

            isTraining = true;
            Invoke("Timer",10f + gen/40);
            spawnCar();
        }
    }

    private void spawnCar()
    {
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);
            }

        }

        cars = new List<CarMovement>();

        for (int i = 0; i < amount; i++)
        {
            CarMovement car = ((GameObject)Instantiate(carPrefab, spawnpoint.transform.position , spawnpoint.transform.rotation)).GetComponent<CarMovement>();
            car.initCar(nets[i]);
            cars.Add(car);
        }
    }

    private void creatNewtWorks()
    {
        if (amount % 2 != 0)
        {
            amount = 20;
        }

        nets = new List<NeuralNetWork>();

        for (int i = 0; i < amount; i++)
        {
            NeuralNetWork netL = new(layers);
            netL.mutate();
            nets.Add(netL);
        }
        Debug.Log("NN are up");
    }
}
