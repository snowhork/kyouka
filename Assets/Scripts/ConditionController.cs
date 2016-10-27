using UnityEngine;
using System.Collections;

public class ConditionController : MonoBehaviour {
		
	public Sensor[] sensors;
	public GameObject car;
	public Agent agent;
	Rigidbody car_rigidbody;
	int timecount;


	float handling;

	public float get_handling(){
		return handling;
	}

	public void crush() {
		Condition condition = set_condition ();
		float reward = -10f;
		int action = agent.decision_action (condition);
		set_action (action);
		agent.recollection (condition, action, reward);
		agent.train ();
	}

	void set_action(int action) {
		switch (action) {
		case 0:
			handling = 0f;
			break;
		case 1:
			handling = 1f;
			break;
		case 2:
			handling = -1f;
			break;
		}
	}

	// Use this for initialization
	void Start () {
		car_rigidbody = car.GetComponent<Rigidbody> ();
		timecount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Condition condition = set_condition ();
		float reward = Vector3.Dot (car_rigidbody.transform.forward, car_rigidbody.velocity);
		int action = agent.decision_action (condition);
		set_action (action);

		if((++timecount)%4 == 0)
			agent.recollection (condition, action, reward);

	}

	Condition set_condition() {
		int[] distances = new int[sensors.Length];
		int velocity;

		for (int i = 0; i < sensors.Length; i++) {
			distances [i] = distance_convert (sensors [i].distance);
		}

		float car_velocity = Vector3.Dot (car_rigidbody.transform.forward, car_rigidbody.velocity);
		velocity = velocity_convert (car_velocity);

		return new Condition (distances, velocity);
	}

	int distance_convert(float distance) {
		int result = (int)(distance/2);
		if (result < 0)
			result = 0;
		if (result >= Agent.distance_max)
			result = Agent.distance_max - 1;
		return result;
	}

	int velocity_convert(float velocity) {
		int result = (int)velocity;
		if (result < 0)
			result = 0;
		if (result >= Agent.velocity_max)
			result = Agent.velocity_max - 1;
		return result;
	}
}
