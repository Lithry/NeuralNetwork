using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chromosome{
	public float fitness;
	public List<float> weights;

	public Chromosome(){
		weights = new List<float>();
	}
}
