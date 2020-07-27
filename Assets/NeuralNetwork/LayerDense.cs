using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Random = System.Random;

namespace NN
{

    public class LayerDense
    {
        public Matrix<double> Weights
        {
            get;
        }
        public Vector<double> Biases
        {
            get;
        }

        public Matrix<double> Output
        {
            get;
            private set;
        }

        private static Random rnd = new Random();
        //Random double numbers for the weight init process.
        private Func<int, int, double> weightInitializer = (row, column) => rnd.NextDouble() * 0.2;
        //Random double numbers for the weight init process.
        private Func<int, double> biasInititializer = (item) => rnd.Next();

        public LayerDense(int nInputs, int nNeurons)
        {
            Weights = DenseMatrix.Create(nInputs, nNeurons, weightInitializer);
            Biases = DenseVector.Create(nNeurons, biasInititializer);
        }

        public void Forward(Matrix<double> input)
        {
            Matrix<double> processed = input.Multiply(Weights);
            for (int i = 0; i < processed.RowCount; i++)
            {
                processed.Row(i).Add(Biases);
            }
            this.ActivationFunction(processed);
        }

        private void ActivationFunction(Matrix<double> input)
        {
            for (int i = 0; i < input.RowCount; i++)
            {
                for (int j = 0; j < input.ColumnCount; j++)
                {
                    input[i, j] = ReLuFunction(input[i,j]);
                }
            }

            this.Output = input;
        }

        private double ReLuFunction(double x)
        {
            if (x < 0)
                return 0;
            
            return x;
        }
    }
}
