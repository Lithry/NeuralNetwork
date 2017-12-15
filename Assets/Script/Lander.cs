using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour {
	private Transform trans;
	private NeuralNetwork brain;
	private Vector3 plataform;
	private float right;
	private float left;
	private float forward;
	private float back;
	private float up;
	private float down;
	private float speed;
	public float fitness;
	private Vector3 lastPosition;
	private float distX;
	private float lastDistX;
	private float distZ;
	private float lastDistZ;

	void Start () {
		trans = transform;
	}

	public void SetBrain(int inputs, int outputs, int hiddenLayers, int neuronsPerHidLayer, float bias, float sigmoidPending){
		brain = new NeuralNetwork(inputs, outputs, hiddenLayers, neuronsPerHidLayer, bias, sigmoidPending);
	}
	
	// Update is called once per frame
	public void UpdateLander (float dt) {
		List<float> inp = new List<float>();
		
		plataform = GetPlataform();

		inp.Add(plataform.x);
		inp.Add(plataform.y);
		inp.Add(plataform.z);

		List<float> outp = brain.Think(inp);
		
		right = outp[0];
		left = outp[1];
		up = outp[2];
		down = outp[3];

		//calculate steering forces
		float movForce = ((right - left) + (up - down)) * 2;
		Vector3 dir = new Vector3(right + left, 0, forward + back);

		//update position
		lastPosition = trans.position;
		trans.position += (dir * dt * movForce);

		distX = Mathf.Abs(trans.position.x - plataform.x);
		distZ = Mathf.Abs(trans.position.z - plataform.z);

		if (distX < lastDistX && distZ < lastDistZ){
			IncrementFitness(10);
		}

		lastDistX = distX;
		lastDistZ = distZ;
	}

	private Vector3 GetPlataform(){
		GameObject plataform = MoonLanderManager.instance.GetPlataform();
		Vector3 dir = plataform.transform.position - trans.position;
		return dir.normalized;
	}

	public int GetNumberOfWeights(){
		return brain.GetNumberOfWeights();
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

	public void IncrementFitness(float val){
		fitness += val;
	}
}
