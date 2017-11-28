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
	}

	public List<GameObject> GetMines(){
		return mines;
	}

	private void Evolve(){
		List<Chromosome> weights = new List<Chromosome>();
		for (int i = 0; i < agents.Count; i++){
			weights.Add(agents[i].GetWeights());
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
