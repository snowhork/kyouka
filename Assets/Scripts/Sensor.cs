using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

	public float distance;
	// Use this for initialization
	void Start () {
		distance = 0;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast (gameObject.transform.position,
				gameObject.transform.forward,
				out hit)) {
			Debug.DrawLine (gameObject.transform.position, hit.point, Color.black);
			this.distance = hit.distance;
		}
	}

	void OnTriggerEnter(Collider other) {
	//	print (other.gameObject.transform.position);
	}
}
