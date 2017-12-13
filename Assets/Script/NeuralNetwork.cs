using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {
	private int numInputs;
	private int numOutputs;
	private int numHiddenLayers;
	private int neuronsPerHiddenLayer;
	private float bias;
	private float sigmoidPending;
	private List<NeuronLayer> neuronlayers = new List<NeuronLayer>();

	public NeuralNetwork(int inputs, int outputs, int hiddenLayers, int neuronsPerHidLayer, float bias, float sigmoidPending){
		numInputs = inputs;
		numOutputs = outputs;
		numHiddenLayers = hiddenLayers;
		neuronsPerHiddenLayer = neuronsPerHidLayer;
		this.bias = bias;
		this.sigmoidPending = sigmoidPending;
		CreateNetwork();
	}

	public void CreateNetwork(){
		// Input Layer
		neuronlayers.Add(new NeuronLayer(numInputs, numInputs));

		// Create the Hidden Layers of the network
		if (numHiddenLayers > 0){

			// Create first Hidden Layer
			neuronlayers.Add(new NeuronLayer(neuronsPerHiddenLayer, numInputs));
			for (int i = 0; i < numHiddenLayers - 1; ++i){
				// Create next Hidden Layer
				neuronlayers.Add(new NeuronLayer(neuronsPerHiddenLayer, neuronsPerHiddenLayer));
			}

			// Create Output Layer
			neuronlayers.Add(new NeuronLayer(numOutputs, neuronsPerHiddenLayer));
		}
		else{
			
			// Create Output Layer
			neuronlayers.Add(new NeuronLayer(numOutputs, numInputs));
		}
	}

	public List<float> GetWeights(){
		List<float> w = new List<float>();

		for (int i = 0; i < neuronlayers.Count; i++){
			for (int j = 0; j < neuronlayers[i].neurons.Count; j++){
				for (int k = 0; k < neuronlayers[i].neurons[j].weights.Count; k++)
				{
					w.Add(neuronlayers[i].neurons[j].weights[k]);
				}
			}
		}

		return w;
	}

	public int GetNumberOfWeights(){
		int w = 0;
		int inp = numInputs;
		
		for (int i = 0; i < neuronlayers.Count; i++){
			for (int j = 0; j < neuronlayers[i].neurons.Count; j++){
				for (int k = 0; k < neuronlayers[i].neurons[j].weights.Count; k++){
					w++;
				}
			}
		}

		return w;
	}

	public void SetWeights(List<float> weights){
		int index = 0;
		for (int i = 0; i < neuronlayers.Count; i++){
			for (int j = 0; j < neuronlayers[i].neurons.Count; j++){
				for (int k = 0; k < neuronlayers[i].neurons[j].weights.Count; k++)
				{
					neuronlayers[i].neurons[j].weights[k] = weights[index++];
				}
			}
		}
	}

	public float Sigmoid(float activation, float response){
		return 1 / (1 + Mathf.Pow(2.7183f, (-activation/response)));
	}

	public List<float> Think(List<float> inputs){
		List<float> outputs = new List<float>();
		
		if (inputs.Count != numInputs){
			return outputs;
		}

		for (int i = 0; i < neuronlayers.Count; i++){
			if ( i > 0 ){
				inputs.Clear();
				inputs.AddRange(outputs);
			}

			outputs.Clear();

			for (int j = 0; j < neuronlayers[i].neurons.Count; j++){
				
				float netinput = 0;
				for (int k = 0; k < neuronlayers[i].neurons[j].weights.Count - 1; ++k){
					netinput += neuronlayers[i].neurons[j].weights[k] *	inputs[k];
				}

				netinput += neuronlayers[i].neurons[j].weights[neuronlayers[i].neurons[j].weights.Count - 1] * bias;

				outputs.Add(Sigmoid(netinput, sigmoidPending));
			}
		}
		return outputs;
	}
}
