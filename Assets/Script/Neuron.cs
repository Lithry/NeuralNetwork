using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
	public int inputs;
	public List<float> weights = new List<float>();

	public Neuron(int numOfInputs){
		inputs = numOfInputs;
		for (int i = 0; i < inputs + 1; i++){
			float weight = Random.Range(-1.0f, 1.0f);
			weights.Add(weight);
		}
	}
}
