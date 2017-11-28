using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
	private Transform trans;
	private int heightLimit;
	private int WidthLimit;
	
	void Start(){
		trans = transform;
	}

	void Update () {
		trans.Translate(Vector3.forward * Time.deltaTime * 5);

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
			Vector3 newPos = new Vector3(trans.position.x, trans.position.y, -WidthLimit);
			trans.position = newPos;
		}
	}
}
