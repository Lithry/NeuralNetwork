using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour {
	private Transform trans;
	private NeuralNetwork brain;
	private Vector3 plataformDir;
    private Vector3 plataformPos;
	private float right;
    private Vector3 vecRight;
	private float left;
    private Vector3 vecLeft;
	private float up;
    private Vector3 vecUp;
	private float down;
    private Vector3 vecDown;
	private float speed;
	public float fitness;

	void Start () {
		trans = transform;
        vecRight = new Vector3(-1, 0, 0);
        vecLeft = new Vector3(1, 0, 0);
        vecUp = new Vector3(0, 0, 1);
        vecDown = new Vector3(0, 0, -1);
	}

	public void SetBrain(int inputs, int outputs, int hiddenLayers, int neuronsPerHidLayer, float bias, float sigmoidPending){
		brain = new NeuralNetwork(inputs, outputs, hiddenLayers, neuronsPerHidLayer, bias, sigmoidPending);
	}
	
	public void UpdateLander (float dt) {
		List<float> inp = new List<float>();
		
		plataformDir = GetPlataformDir();
        plataformPos = GetPlataformPos();

		inp.Add(plataformDir.x);
		inp.Add(plataformDir.z);
        inp.Add(plataformPos.x);
        inp.Add(plataformPos.z);

		List<float> outp = brain.Think(inp);
		
		right = outp[0];
		left = outp[1];
		up = outp[2];
		down = outp[3];

        Vector3 dir = (vecRight * right) + (vecLeft * left) + (vecUp * up) + (vecDown * down);

		float movForce = ((right - left) + (up - down)) * 5;

		trans.position += (dir * dt * movForce);

        IncrementFitness(100 / Vector3.Distance(trans.position, plataformPos));
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
	}

	public List<float> GetWeights(){
		return brain.GetWeights();
	}

	public void IncrementFitness(float val){
		fitness += val;
	}

    void OnCollisionEnter(Collision coll){
        if (coll.transform.tag == "Plataform"){
            IncrementFitness(500);
        }
    }

    void OnCollisionStay(Collision coll){
        if (coll.transform.tag == "Plataform"){
            IncrementFitness(500);
        }
    }
}
