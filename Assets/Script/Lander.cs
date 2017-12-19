using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour {
	private Transform trans;
	private NeuralNetwork brain;
	private Vector3 plataformDir;
	private Vector3 lastDir;
	private float right;
	private float left;
	private float up;
	private float speed;
	public float fitness;
	private bool onPlataform;
	private float distX;
	private float lastDistX;
	private float distZ;
	private float lastDistZ;
	private float dist;
	private float lastDist;
	private int heightLimit;
	private int WidthLimit;

	void Start () {
		onPlataform = false;
		trans = transform;
		lastDir = Vector3.zero;
	}

	public void SetBrain(int inputs, int outputs, int hiddenLayers, int neuronsPerHidLayer, float bias, float sigmoidPending){
		brain = new NeuralNetwork(inputs, outputs, hiddenLayers, neuronsPerHidLayer, bias, sigmoidPending);
	}
	
	public void UpdateLander (float dt) {
		if (!onPlataform){
			List<float> inp = new List<float>();

			plataformDir = GetPlataformDir();

			inp.Add(plataformDir.x);
			inp.Add(plataformDir.z);
			inp.Add(lastDir.x);
			inp.Add(lastDir.z);

			List<float> outp = brain.Think(inp);

			up = outp[0];
			right = outp[1];
			left = outp[2];

        	Vector3 dir = (trans.right * right) + (-trans.right * left) + (trans.up * up);
			Debug.DrawRay(trans.position, plataformDir, Color.red);
			Debug.DrawRay(trans.position, lastDir, Color.magenta);
			Debug.DrawRay(trans.position, dir, Color.green);
			lastDir = dir.normalized;

			float movForce = (((Mathf.Abs(right - left)) * 2) + up) * 5;

			trans.position += (dir * dt * movForce);
			trans.position += (-trans.up * 2 * dt);
		}
		
		Fitness();
		//Limits();
	}

	private void Fitness(){
		dist = Vector3.Distance(trans.position, GetPlataformPos());
		Vector3 dif = GetPlataformPos() - trans.position;
		distX = Mathf.Abs(dif.x);
		distZ = Mathf.Abs(dif.z);
		if (dist < lastDist){
			IncrementFitness(100 / Vector3.Distance(trans.position, GetPlataformPos()));
			if (distX < lastDistX)
				IncrementFitness(70 / Vector3.Distance(trans.position, GetPlataformPos()));
			
			if (distZ < lastDistZ)
				IncrementFitness(40 / Vector3.Distance(trans.position, GetPlataformPos()));
		}
		lastDist = dist;
		lastDistX = distX;
		lastDistZ = distZ;
	}

	private Vector3 GetPlataformDir(){
		GameObject plataform = MoonLanderManager.instance.GetPlataform();
		Vector3 dir = plataform.transform.position - trans.position;
		return dir.normalized;
	}
    
    private Vector3 GetPlataformPos(){
        GameObject plataform = MoonLanderManager.instance.GetPlataform();
        return plataform.transform.position;
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
		onPlataform = false;
		dist = Vector3.Distance(trans.position, GetPlataformPos());
	}

	public List<float> GetWeights(){
		return brain.GetWeights();
	}

	public void IncrementFitness(float val){
		fitness += val;
	}

	public void SetLimits(int h, int w){
		heightLimit = h;
		WidthLimit = w;
	}

	private void Limits(){
		if (trans.position.z < -heightLimit){
			Vector3 newPos = new Vector3(trans.position.x, trans.position.y, heightLimit);
			trans.position = newPos;
		}
		else if (trans.position.z > heightLimit){
			Vector3 newPos = new Vector3(trans.position.x, trans.position.y, -heightLimit);
			trans.position = newPos;
		}
	}
	
	void OnTriggerEnter(Collider coll){
		if (coll.tag == "Plataform")
			onPlataform = true;
	}
	
	void OnTriggerStay(Collider coll){
		if (coll.tag == "PlataformInside" && onPlataform)
			IncrementFitness(10000);
	}
}
