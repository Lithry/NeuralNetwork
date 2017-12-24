using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
	const float SpeedFactor = 10.0f;
	private Transform trans;
	private int heightLimit;
	private int WidthLimit;
	private NeuralNetwork brain;
	private float rotation;
	private float speed;
	private float right;
	private float left;
	public float fitness;
	private Vector3 closestMine;
	
	void Start(){
		trans = transform;
	}

	public void SetBrain(int inputs, int outputs, int hiddenLayers, int neuronsPerHidLayer, float bias, float sigmoidPending){
		brain = new NeuralNetwork(inputs, outputs, hiddenLayers, neuronsPerHidLayer, bias, sigmoidPending);
	}

	public void UpdateTank (float dt) {
		//this will store all the inputs for the NN
		List<float> inp = new List<float>();
		
		//get vector to closest mine
		closestMine = GetClosestMine();
		
		//add in the vector to the closest mine
		inp.Add(closestMine.x);
		inp.Add(closestMine.z);
		
		//add in the sweeper's look at vector
		inp.Add(trans.forward.x);
		inp.Add(trans.forward.z);
	
		//update the brain and get the output from the network
		List<float> outp = brain.Think(inp);
		
		//assign the outputs to the sweepers left & right tracks
		right = outp[0];
		left = outp[1];

		//calculate steering forces
		float rotForce = right - left;
		
		//clamp rotation
		Mathf.Clamp(rotForce, -2, 2);
		speed = (right + left);

		//update the minesweepers rotation
		rotation += rotForce;
		
		//update Look At
		Vector3 lookAt = trans.forward;
		lookAt.x = -Mathf.Sin(rotation);
		lookAt.z = Mathf.Cos(rotation);

		//update position
		trans.position += (lookAt * speed * dt * SpeedFactor);
		trans.forward = lookAt;
		Limits();
	}

	public void SetLimits(int h, int w){
		heightLimit = h;
		WidthLimit = w;
	}

	private void Limits(){
		if (trans.position.x < -WidthLimit){
			Vector3 newPos = new Vector3(WidthLimit, trans.position.y, trans.position.z);
			trans.position = newPos;
		}
		else if (trans.position.x > WidthLimit){
			Vector3 newPos = new Vector3(-WidthLimit, trans.position.y, trans.position.z);
			trans.position = newPos;
		}

		if (trans.position.z < -heightLimit){
			Vector3 newPos = new Vector3(trans.position.x, trans.position.y, heightLimit);
			trans.position = newPos;
		}
		else if (trans.position.z > heightLimit){
			Vector3 newPos = new Vector3(trans.position.x, trans.position.y, -heightLimit);
			trans.position = newPos;
		}
	}

	private Vector3 GetClosestMine(){
		List<GameObject> mines = Manager.instance.GetMines();
		float distance = Vector3.Distance(trans.position, mines[0].transform.position);
		Vector3 dir = mines[0].transform.position - trans.position;
		for (int i = 1; i < mines.Count; i++){
			if (Vector3.Distance(trans.position, mines[i].transform.position) < distance){
				distance = Vector3.Distance(trans.position, mines[i].transform.position);
				dir = mines[i].transform.position - trans.position;
			}
		}
		return dir.normalized;
	}

	public void IncrementFitness(float val){
		fitness += val;
	}

	public float GetFitness(){
		return fitness;
	}

	public void SetWeights(List<float> w){
		brain.SetWeights(w);
		fitness = 0;
	}

	public List<float> GetWeights(){
		return brain.GetWeights();
	}

	public int GetNumberOfWeights(){
		return brain.GetNumberOfWeights();
	}

	public void EvolveBrain(){

	}

	/*void OnTriggerEnter(Collider collider){
		IncrementFitness(10);
	}*/
}
