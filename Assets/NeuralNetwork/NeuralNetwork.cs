using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace NN
{
    class NeuralNetwork
    {
        public List<LayerDense> HiddenLayers
        {
            get;
        }

        public LayerDense InputLayer
        {
            get;
        }

        public LayerDense OutputLayer
        {
            get;
        }

        public NeuralNetwork()
        {
            InputLayer = new LayerDense(3, 3);
            HiddenLayers = new List<LayerDense>();
            HiddenLayers.Add(new LayerDense(3,5));
            HiddenLayers.Add(new LayerDense(5,5));
            OutputLayer = new LayerDense(5, 2);
        }

        public bool Think(Matrix<double> input)
        {
            //InputLayer.Forward(input);
            HiddenLayers[0].Forward(input);
            HiddenLayers[1].Forward(HiddenLayers[0].Output);
            OutputLayer.Forward(HiddenLayers[1].Output);
            Console.WriteLine(OutputLayer.Output);
            //If the first then jump, if the second then not
            Debug.Log(Math.Abs(1 - OutputLayer.Output[0,0]));
            Debug.Log(Math.Abs(1 - OutputLayer.Output[0,1]));
            if (Math.Abs(1 - OutputLayer.Output[0,0]) < Math.Abs(1 - OutputLayer.Output[0,1]))
            {
                Debug.Log("kene ugrani");
                return true;
            }

            return false;
        }
    }
}