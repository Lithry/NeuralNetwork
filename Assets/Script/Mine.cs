using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
	private Transform trans;
	private int heightLimit;
	private int WidthLimit;

	void Awake () {
		trans = transform;
	}

	public void SetLimits(int h, int w){
		heightLimit = h;
		WidthLimit = w;
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Tank"){
			trans.position = new Vector3(Random.Range(1 - (float)WidthLimit, (float)WidthLimit - 1), 0, Random.Range(1 - (float)heightLimit, (float)heightLimit - 1));
			collider.gameObject.GetComponent<Tank>().IncrementFitness(100.0f);
		}
	}
}
