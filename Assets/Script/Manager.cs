using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	public static Manager instance;
	public GameObject agent;
	public int agentNum;
	private List<Tank> agents = new List<Tank>();
	public GameObject mine;
	public int mineNum;
	private List<GameObject> mines = new List<GameObject>();
	public int elitesNum;
	[Range(0.00f, 1.00f)]
	public float mutation;
	public int maxHeight;
	public int maxWidth;
	public int inputs;
	public int outputs;
	public int numHiddenLayers;
	public int numNeuronPerHiddenLayer;
	[Range(-1.0f, 1.0f)]
	public float bias;
	[Range(0.01f, 3.0f)]
	public float sigmoidPending;
	[Range(1, 50)]
	public int iterations;
	public int durationOfGeneration;
	private float timer;
	private GeneticAlg ga;

	// Use this for initialization
	void Awake () {
		instance = this;
		for (int i = 0; i < mineNum; i++){
			GameObject obj = Instantiate(mine, new Vector3(Random.Range(1 - (float)maxWidth, (float)maxWidth) - 1, 0, Random.Range(1 - (float)maxHeight, (float)maxHeight - 1)), Quaternion.Euler(0, 0, 0));
			obj.GetComponent<Mine>().SetLimits(maxHeight, maxWidth);
			mines.Add(obj);
		}
		
		for (int i = 0; i < agentNum; i++){
			GameObject obj = Instantiate(agent, new Vector3(Random.Range(1 - (float)maxWidth, (float)maxWidth) - 1, 0, Random.Range(1 - (float)maxHeight, (float)maxHeight - 1)), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
			Tank tank = obj.GetComponent<Tank>();
			tank.SetLimits(maxHeight, maxWidth);
			tank.SetBrain(inputs, outputs, numHiddenLayers, numNeuronPerHiddenLayer, bias, sigmoidPending);
			if (i == 0)
				ga = new GeneticAlg(elitesNum, agentNum, tank.GetNumberOfWeights(), mutation);
			agents.Add(tank);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for (int i = 0; i < iterations; i++){
			for (int j = 0; j < agents.Count; j++){
				agents[j].UpdateTank(Time.fixedDeltaTime);
			}
		}

		timer += Time.fixedDeltaTime;
		if (timer > durationOfGeneration)
			Evolve();
	}

	public List<GameObject> GetMines(){
		return mines;
	}

	private void Evolve(){
		List<Chromosome> weights = new List<Chromosome>();
		for (int i = 0; i < agents.Count; i++){
			Chromosome c = weights[i];
			c.fitness = agents[i].GetFitness();
			c.weights = agents[i].GetWeights();
			weights[i] = c;
		}
		weights = ga.Evolv(weights);

		for (int i = 0; i < agents.Count; i++)
		{
			agents[i].SetWeights(weights[i].weights);
		}
	}

	

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(-maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(-maxWidth, 0, maxHeight));
	}
}
