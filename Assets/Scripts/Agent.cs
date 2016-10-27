using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;


public class Agent : MonoBehaviour {

	public const int distance_max = 6;
	public const int velocity_max = 10;
	public const int actions = 3;
	public const int memories_max = 1000;

	public Text text;
	public Text condition;

	float gamma;
	float epsilon;
	float[,,,,] Q;
	int[,,,,] counts;
	Memory[] memories;
	int memories_index;
	int current_memories_num;

	// Use this for initialization
	void Start () {
		gamma = 0.96f;
		epsilon = 0.05f;
		Q = new float[distance_max, distance_max, distance_max, velocity_max, actions];
		counts = new int[distance_max, distance_max, distance_max, velocity_max, actions];
		memories = new Memory[memories_max];

		memories_index = 0;
		current_memories_num = 0;
	}

	public void recollection(Condition condition, int action, float reward) {
		memories[memories_index] = new Memory (condition, action, reward);
		memories_index++;
		if (memories_index == memories_max)
			memories_index = 0;
		if (!(current_memories_num == memories_max))
			current_memories_num++;
	}

	public int decision_action(Condition condition) {
		int max_index = 0;
		float max = 
			Q [condition.sensors_distance [0], 
				condition.sensors_distance [1], 
				condition.sensors_distance [2],
				condition.speed, 0];
		if (counts [condition.sensors_distance [0], 
			    condition.sensors_distance [1], 
			    condition.sensors_distance [2],
			    condition.speed, 0] == 0 &&
		    counts [condition.sensors_distance [0], 
			    condition.sensors_distance [1], 
			    condition.sensors_distance [2],
			    condition.speed, 1] == 0 &&
		    counts [condition.sensors_distance [0], 
			    condition.sensors_distance [1], 
			    condition.sensors_distance [2],
			    condition.speed, 2] == 0) { //first condition
			int action = Random.Range (0, 3);
			int index = 0;
			int distance_max = condition.sensors_distance[0];
			for (int i = 1; i < 3; i++) {
				if (condition.sensors_distance [i] > distance_max) {
					index = i;
					distance_max = condition.sensors_distance [i];
				}
			
			}
			text.text = "Random";
			return action;
		}
		for (int i = 1; i < actions; i++) {
			float compare = Q [condition.sensors_distance [0], condition.sensors_distance [1], condition.sensors_distance [2], 
				                condition.speed, i];
		
			if (compare > max) {
				
				max = compare;
				max_index = i;
			}
		}
//		print (Q [condition.sensors_distance [0], 
//			condition.sensors_distance [1], 
//			condition.sensors_distance [2],
//			condition.speed, 0].ToString() + "|" +
//			Q [condition.sensors_distance [0], 
//				condition.sensors_distance [1], 
//				condition.sensors_distance [2],
//				condition.speed, 1].ToString() + "|" +
//			Q [condition.sensors_distance [0], 
//				condition.sensors_distance [1], 
//				condition.sensors_distance [2],
//				condition.speed, 2].ToString());
//		print (max_index);
		if (!Input.GetKey("space") && Random.Range (0, 20) == 0) {
			text.text = "max-epsilon";
			return Random.Range (0, 2);
		} else {
			text.text = "max";
		}
		return max_index;
	}

	public void train() {
		for (int i = 0; i < current_memories_num; i++) {
			Memory memory = memories [i];
			try {
				int c = ++counts [memory.condition.sensors_distance [0],
					           memory.condition.sensors_distance [1],
					           memory.condition.sensors_distance [2],
					           memory.condition.speed,
					           memory.action];
			} catch {
				print (memory.condition.sensors_distance [0] + "|" +
				memory.condition.sensors_distance [1] + "|" +
				memory.condition.sensors_distance [2] + "|" +
				memory.condition.speed + "|" +
				memory.action);
			}
			int count = ++counts [memory.condition.sensors_distance [0],
				memory.condition.sensors_distance [1],
				memory.condition.sensors_distance [2],
				memory.condition.speed,
				memory.action];
		
							
			float old_q = Q [memory.condition.sensors_distance [0],
				              memory.condition.sensors_distance [1],
				              memory.condition.sensors_distance [2],
				              memory.condition.speed,
				              memory.action];
			float reward = 0;
			float gamma = 1.0f;

			for (int j = i + 1; j < current_memories_num; j++) {
				reward += memories [j].reward * gamma;
				gamma *= this.gamma;
			}
			//print (reward);

			Q [memory.condition.sensors_distance [0],
				memory.condition.sensors_distance [1],
				memory.condition.sensors_distance [2],
				memory.condition.speed,
				memory.action] = old_q * (count - 1) / count + reward / count;
			//print (old_q);
		}
		memories_index = 0;
		current_memories_num = 0;
	}

}

public class Memory {

	public Condition condition;
	public int action;
	public float reward;

	public Memory(Condition condition, int action, float reward) {
		this.condition = condition;
		this.action = action;
		this.reward = reward;
	}
}