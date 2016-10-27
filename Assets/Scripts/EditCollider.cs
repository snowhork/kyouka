using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (CarController))]
	public class EditCollider : MonoBehaviour {

		Vector3 start_position;
		UnityEngine.Quaternion start_rotation;
		Rigidbody rigid_body;
		bool move_flag;

		CarController m_Car; 
		public GameObject ExploadObj;
		public ConditionController condition_controller;

		void Start() {
			move_flag = true;
			start_position = this.gameObject.transform.position;
			start_rotation = this.gameObject.transform.rotation;
			rigid_body = this.GetComponent<Rigidbody>();
			m_Car = GetComponent<CarController>();
		}

		private void FixedUpdate(){
			if (move_flag) {
				print ("update");

				float h = CrossPlatformInputManager.GetAxis ("Horizontal");
				float v = CrossPlatformInputManager.GetAxis ("Vertical");
			
				float handbrake = CrossPlatformInputManager.GetAxis ("Jump");

			
				m_Car.Move (condition_controller.get_handling(), 1f, 1f, 0f);
				//m_Car.Move (h, 1f, 1f, 0f);
				//m_Car.Move(h, v, v, handbrake);
			}
			//     
		}

		void OnCollisionEnter (Collision other) {
			if (other.gameObject.tag == "Wall" && move_flag) {
				rigid_body.velocity = new Vector3 (0, 0, 0);
				Instantiate (ExploadObj, this.gameObject.transform.position, Quaternion.identity);
				StartCoroutine ("Wait");
				move_flag = false;
			}
		}

		IEnumerator Wait () {
			yield return new WaitForSeconds (1.0f);
			this.gameObject.transform.position = start_position;
			this.gameObject.transform.rotation = start_rotation;
			rigid_body.velocity = new Vector3 (0, 0, 0);
			move_flag = true;
			condition_controller.crush ();
		}

	}

}


