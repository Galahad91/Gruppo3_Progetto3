using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IstanziaTorri : MonoBehaviour 
{

	public GameObject oggettoManager;
	private IstanziareOggetto istanzia;
	private GestoreGioco gestore;

	public bool isCamminabile;
	public float x;
	public float z;

	void Start()
	{

		oggettoManager = GameObject.FindGameObjectWithTag ("GameManager");
		istanzia = oggettoManager.GetComponent<IstanziareOggetto> ();
		gestore = oggettoManager.GetComponent<GestoreGioco> ();

	}
	
	void OnMouseUp()
	{

		Debug.Log ("Clicco");

		//Debug.Log ("click");
		istanzia.x = gameObject.transform.position.x;
		istanzia.z = gameObject.transform.position.z;


	}

}
