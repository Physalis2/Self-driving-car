using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetWork
{
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;
    //[] Layer,
    //[][] Layer,   einzelnen Neuronen
    //[][][] layer,     einzelnen Neuronen,     weigth des neurons angegeben zwischen -1 bis 1 
    //[][][0] ist die gewichtung zum 0ten (1.) neuron in der layer i - 1 und neuron j [i][j][k] weight zwischen neuron j in layer i und neuron k in layer i - 1


    public NeuralNetWork(int[] layer)
    {
        this.layers = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
        {
            this.layers[i] = layer[i];
        }

        InitNeurons(); //Constructs Matrix
        InitWeigths(); //Constructs Matrix

    }

    private void InitNeurons()
    {
        //Neuron Initilization
        List<float[]> neuronList = new List<float[]>();

        for (int i = 0; i < layers.Length; ++i) //durch layers bewegen
        {
            neuronList.Add(new float[layers[i]]);//Hinzufügen der layer anzahl neurons[x][]
                                                 //layer[i] enthält anzahl neuronen in layer i => neurons[i][x]
        }

        neurons = neuronList.ToArray();
    }

    private void InitWeigths()
    {
        //weights Initilization
        List<float[][]> weigthsList = new List<float[][]>();

        for (int i = 1; i < layers.Length; ++i) //durchgeht alle layer
        {
            List<float[]> layerWeigthsList = new List<float[]>();

            int neuronInPriviousLayer = layers[i - 1]; //anzahl neuronen in der layer davor i - 1

            for (int j = 0; j < neurons[i].Length; ++j) //durch gehen aller neuronen j in layer i. 
            {
                float[] neuronWeigths = new float[neuronInPriviousLayer]; //alle weigths einer neurons zu der vorherigen layer in einem Array gespeichert

                //set weigths random (-1 < x < 1)
                for (int k = 0; k < neuronInPriviousLayer; k++) //durchgeht alle verbindung des neurons j in layer i mit neuron k in layer i - 1 
                {
                    neuronWeigths[k] = UnityEngine.Random.Range(-0.5f, 0.5f); //random weigth to neuron weigth geben
                }
                layerWeigthsList.Add(neuronWeigths);
            }
            weigthsList.Add(layerWeigthsList.ToArray());
        }
        weights = weigthsList.ToArray();
    }

    public float[] feedForward(float[] inputs) //input array an inputs
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i]; //neuron i auf layer 0 bekommt input von i zugewiesen => neuron i = iput i
        }

        for (int i = 1; i < layers.Length; i++) //Durchgeht aller layer außer iputs
        {
            for (int j = 0; i < neurons[i].Length; j++) //durchgeht dann aller neuronen auf der layer i
            {
                float value = 0.25f;

                for (int k = 0; k < neurons[i - 1].Length; k++) //durchgeht aller neuronen der layer i - 1 // start auf 2. layer also i - 1
                {
                    value += weights[i - 1][j][k]; //errechnet value errechnet value des neurons j auf der layer i zu durch die neuron k auf layer i - 1  * weights
                }
                neurons[i][j] = (float)Math.Tanh(value); //weisst neuron j auf layer i einen wert zu.
            }
        }
        return neurons[neurons.Length - 1]; //gibt den wert des out puts zurück 
    }

    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++) //durchgeht alle Layers i
        {
            for (int j = 0; j < weights[i].Length; j++) //durchgeht alle neurons j
            {
                for (int k = 0; k < weights[i][j].Length; k++) //ändert alle weights k im neuron j auf layer i
                {
                    float weight = weights[i][j][k];

                    float randomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumber <= 2f)
                    { //if 1
                      //flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    { //if 2
                      //pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { //if 3
                      //randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { //if 4
                      //randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    private float fitness;

    public void addFitness(float amount)
    {
        fitness += amount;
    }

    public void detFitness(float amount)
    {
        fitness = amount;
    }

    public float getFitness() 
    {
        return fitness;
    }

    public int CompareTo(NeuralNetWork other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness) 
            return -1;
        else 
            return 0;
    }
}