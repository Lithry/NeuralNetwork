using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonLanderManager : MonoBehaviour {
	public static MoonLanderManager instance;
	private GeneticAlg ga;
	private List<Lander> landers = new List<Lander>();
	public GameObject landerPrefab;
	public GameObject plataformPrefab;
	private GameObject plataform;
	public int lunarsNum;
	public float durationOfGeneration;
	public int inputs;
	public int outputs;
	public int numHiddenLayers;
	public int numNeuronPerHiddenLayer;
	public int elitesNum;
	[Range(-1.0f, 1.0f)]
	public float bias;
	[Range(0.00f, 1.00f)]
	public float mutation;
	[Range(0.01f, 3.0f)]
	public float sigmoidPending;
	private int generation;
	public Text generationText;
	private float timer;
	public Text timerText;
	public Text plataformText;
	[Range(1, 15)]
	public int iterations;
	public int maxHeight;
	public int maxWidth;
	
	void Awake () {
		instance = this;
		generation = 1;
		timer = 0.0f;

		for (int i = 0; i < lunarsNum; i++){
            GameObject obj = Instantiate(landerPrefab, new Vector3(Random.Range(-16, 17), 0, Random.Range(-15, 16)), Quaternion.Euler(90, 0, 0));
			Lander lan = obj.GetComponent<Lander>();
			lan.SetBrain(inputs, outputs, numHiddenLayers, numNeuronPerHiddenLayer, bias, sigmoidPending);
			lan.SetLimits(maxHeight, maxWidth);
			if (i == 0)
				ga = new GeneticAlg(elitesNum, lunarsNum, lan.GetNumberOfWeights(), mutation);
			landers.Add(lan);
		}
		plataform = Instantiate(plataformPrefab, new Vector3(Random.Range(-20, 21), 0, Random.Range(-12, 13)), Quaternion.Euler(90, 0, 0));
	}
	
	void FixedUpdate () {
		generationText.text = "Generation: " + generation.ToString();
		timerText.text = "Time: " + timer.ToString("F2");
		plataformText.text = "Plataform Pos: " + plataform.transform.position;
		for (int i = 0; i < iterations; i++){
			for (int j = 0; j < landers.Count; j++){
				landers[j].UpdateLander(Time.fixedDeltaTime);
			}
			timer += Time.fixedDeltaTime;
		}
		if (timer > durationOfGeneration)
			Evolve();
	}

	public GameObject GetPlataform(){
		return plataform;
	}

	private void Evolve(){
		float maxFitness = landers[0].GetFitness();
		List<Chromosome> chromList = new List<Chromosome>();
		for (int i = 0; i < landers.Count; i++){
			Chromosome c = new Chromosome();
			c.fitness = landers[i].GetFitness();
			if (maxFitness < c.fitness)
				maxFitness = c.fitness;
			c.weights = landers[i].GetWeights();
			chromList.Add(c);
		}
		Debug.Log("Gen " + generation.ToString() + " max Fitness: " + maxFitness.ToString("F0"));
		chromList = ga.Evolv(chromList);

		for (int i = 0; i < landers.Count; i++)
		{
            landers[i].transform.position = new Vector3(Random.Range(-16, 17), 0, Random.Range(-15, 16));
		}
		
		for (int i = 0; i < landers.Count; i++)
		{
			landers[i].SetWeights(chromList[i].weights);
		}
		
		plataform.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-12, 13));

		timer = 0.0f;
		generation++;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(-maxWidth, 0, maxHeight));
	}
}
