using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	public GameObject agent;
	public int agentNum;
	private List<GameObject> agents = new List<GameObject>();
	public GameObject mine;
	public int mineNum;
	private List<GameObject> mines = new List<GameObject>();
	public int maxHeight;
	public int maxWidth;

	// Use this for initialization
	void Awake () {
		for (int i = 0; i < mineNum; i++){
			GameObject obj = Instantiate(mine, new Vector3(Random.Range(1 - (float)maxWidth, (float)maxWidth) - 1, 0, Random.Range(1 - (float)maxHeight, (float)maxHeight - 1)), Quaternion.Euler(0, 0, 0));
			obj.GetComponent<Mine>().SetLimits(maxHeight, maxWidth);
			mines.Add(obj);
		}
		
		for (int i = 0; i < agentNum; i++){
			GameObject obj = Instantiate(agent, new Vector3(Random.Range(1 - (float)maxWidth, (float)maxWidth) - 1, 0, Random.Range(1 - (float)maxHeight, (float)maxHeight - 1)), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
			obj.GetComponent<Tank>().SetLimits(maxHeight, maxWidth);
			agents.Add(obj);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, -maxHeight));
		Gizmos.DrawLine(new Vector3(-maxWidth, 0, -maxHeight), new Vector3(-maxWidth, 0, maxHeight));
		Gizmos.DrawLine(new Vector3(maxWidth, 0, maxHeight), new Vector3(-maxWidth, 0, maxHeight));
	}
}
