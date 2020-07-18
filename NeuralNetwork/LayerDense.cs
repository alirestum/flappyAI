using Extreme.Mathematics;
using Extreme.Mathematics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN
{

    class LayerDense
    {
        public DenseMatrix<double> Weights
        {
            get;
        }
        public DenseVector<double> Biases
        {
            get;
        }

        public DenseMatrix<double> Output
        {
            get;
        }

        private static Random rnd = new Random();
        //Random double numbers for the weight init process.
        private Func<int, int, double> weightInitializer = (row, column) => rnd.NextDouble() * 0.1;
        //Random double numbers for the weight init process.
        private Func<int, double> biasInititializer = (item) => rnd.NextDouble();

        public LayerDense(int n_inputs, int n_neurons)
        {
            Weights = Matrix.Create(n_inputs, n_neurons, weightInitializer, ArrayMutability.MutableSize);
            Biases = Vector.Create(n_neurons, biasInititializer, ArrayMutability.MutableSize);
        }

        public void forward()
        {

        }

        private void activationFunction()
        {

        }
    }
}
