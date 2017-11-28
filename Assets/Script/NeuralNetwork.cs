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
		//create the layers of the network
		if (numHiddenLayers > 0){
			
			//create first hidden layer
			neuronlayers.Add(new NeuronLayer(neuronsPerHiddenLayer, numInputs));
			for (int i = 0; i < numHiddenLayers - 1; ++i){
				neuronlayers.Add(new NeuronLayer(neuronsPerHiddenLayer, neuronsPerHiddenLayer));
			}

			//create output layer
			neuronlayers.Add(new NeuronLayer(numOutputs, neuronsPerHiddenLayer));
		}
		else{
		
		//create output layer
			neuronlayers.Add(new NeuronLayer(numOutputs, numInputs));
		}
	}

	public List<float> GetWeights(){

		return null;
	}

	public int GetNumberOfWeights(){

		return 0;
	}

	public void SetWeights(List<float> weights){
	
	}

	public float Sigmoid(float activation, float response){

		return 0;
	}

	public List<float> Tink(List<float> inputs){
		//stores the resultant outputs from each layer
		List<float> outputs = new List<float>();
		int cWeight = 0;
		
		//first check that we have the correct amount of inputs
		if (inputs.Count != numInputs){
			//just return an empty vector if incorrect.
			return outputs;
		}
		
		//For each layer...
		for (int i = 0; i < numHiddenLayers + 1; ++i){
			if ( i > 0 ){
				inputs = outputs;
			}
			outputs.Clear();
			cWeight = 0;
			
			//for each neuron sum the inputs * corresponding weights. Throw
			//the total at the sigmoid function to get the output.
			for (int j = 0; j < neuronlayers[i].neuronNumber; ++j){
				float netinput = 0;
				int NumInputs = neuronlayers[i].neurons[j].inputs;
				//for each weight
				for (int k = 0; k < NumInputs - 1; ++k)
				{
					//sum the weights x inputs
					netinput += neuronlayers[i].neurons[j].weights[k] *	inputs[cWeight++];
				}
				//add in the bias
				netinput += neuronlayers[i].neurons[j].weights[NumInputs - 1] * bias;

				//we can store the outputs from each layer as we generate them.
				//The combined activation is first filtered through the sigmoid
				//function
				outputs.Add(Sigmoid(netinput, sigmoidPending));
				cWeight = 0;
			}
		}
		return outputs;
	}
}
