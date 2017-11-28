using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronLayer {
	public int neuronNumber;
	public List<Neuron> neurons = new List<Neuron>();

	public NeuronLayer(int numOfNeurons, int inputsPerNeuron){
		neuronNumber = numOfNeurons;
		for (int i = 0; i < neuronNumber; i++){
			Neuron n = new Neuron(inputsPerNeuron);
			neurons.Add(n);
		}
	}
}
