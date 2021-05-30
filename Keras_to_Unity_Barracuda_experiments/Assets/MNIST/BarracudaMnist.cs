using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class BarracudaMnist : MonoBehaviour
{
    // https://github.com/mushe/Keras_to_Unity_Barracuda_experiments/blob/main/Keras_to_onnx_Unity_Barracuda_MNIST.ipynb
    public NNModel _kerasModel;

    // https://github.com/onnx/models/blob/master/vision/classification/mnist/model/mnist-8.onnx
    public NNModel _mnist8Model;

    int Predict(NNModel model, Texture2D tex)
    {
        var input = new Tensor(1, 28, 28, 1);
        for (var y = 0; y < 28; y++)
        {
            for (var x = 0; x < 28; x++)
            {
                var tx = x * tex.width / 28;
                var ty = y * tex.height / 28;
                input[0, 27 - y, x, 0] = tex.GetPixel(tx, ty).grayscale;
            }
        }

        var worker = ModelLoader.Load(model).CreateWorker();
        worker.Execute(input);
        var output = worker.PeekOutput();


        int result = 0;
        float maxProbability = 0.0f;
        for (var i = 0; i < 10; i++)
        {
            var probability = output[0, 0, 0, i];
            if (probability > maxProbability)
            {
                maxProbability = probability;
                result = i;
            }
        }

        input.Dispose();
        worker.Dispose();

        return result;
    }

    public int PredictByKerasModel(Texture2D tex)
    {
        return Predict(_kerasModel, tex);
    }

    public int PredictByMNIST8Model(Texture2D tex)
    {
        return Predict(_mnist8Model, tex);
    }

}
