using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DannoNexus : MonoBehaviour {

	/// <summary>
	///  Quando il personaggio muore richiama il metodo di morte e toglie danno al Nexus
	/// </summary>
	/// <param name="other">Oggetto con cui collide</param>
	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Nexus") 
		{
			Debug.Log ("Mi suicido");

			transform.parent.gameObject.GetComponent<Assassin> ().ToraToraTora ();

		}

	}

}
