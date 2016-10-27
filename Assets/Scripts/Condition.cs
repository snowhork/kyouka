using UnityEngine;
using System.Collections;

public class Condition : MonoBehaviour {

	public int[] sensors_distance;
	public int speed;

	public Condition(int[] sensors_distance, int speed) {
		set_params (sensors_distance, speed);
	}

	public void set_params(int[] sensors_distance, int speed) {
		this.sensors_distance = sensors_distance;
		this.speed = speed;
	}

	public void print_params() {
		print (this.speed);
	}
}