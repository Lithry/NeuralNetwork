using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	private int generation;
	public Text generationText;
	public Text timerText;

	// Use this for initialization
	void Awake () {
		instance = this;
		generation = 1;
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
	
	void FixedUpdate () {
		generationText.text = "Generation: " + generation.ToString();
		timerText.text = "Time: " + timer.ToString("F2");
		for (int i = 0; i < iterations; i++){
			for (int j = 0; j < agents.Count; j++){
				agents[j].UpdateTank(Time.fixedDeltaTime);
			}
			timer += Time.fixedDeltaTime;
		}
		if (timer > durationOfGeneration)
			Evolve();
	}

	public List<GameObject> GetMines(){
		return mines;
	}

	private void Evolve(){
		float maxFitness = agents[0].GetFitness();
		List<Chromosome> chromList = new List<Chromosome>();
		for (int i = 0; i < agents.Count; i++){
			Chromosome c = new Chromosome();
			c.fitness = agents[i].GetFitness();
			if (maxFitness < c.fitness)
				maxFitness = c.fitness;
			c.weights = agents[i].GetWeights();
			chromList.Add(c);
		}
		Debug.Log("Gen " + generation.ToString() + " max Fitness: " + maxFitness.ToString("F0"));
		chromList = ga.Evolv(chromList);

		for (int i = 0; i < agents.Count; i++)
		{
			agents[i].SetWeights(chromList[i].weights);
		}

		for (int i = 0; i < agents.Count; i++)
		{
			agents[i].transform.position = new Vector3(Random.Range(1 - (float)maxWidth, (float)maxWidth) - 1, 0, Random.Range(1 - (float)maxHeight, (float)maxHeight - 1));
			agents[i].transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
		}

		timer = 0.0f;
		generation++;
	}

	

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(-maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(-maxWidth, 0, maxHeight));
	}
}
