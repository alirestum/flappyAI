using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN
{
    class NeuralNetwork
    {
        public List<LayerDense> hiddenLayers
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
            hiddenLayers.
        }
    }
}
