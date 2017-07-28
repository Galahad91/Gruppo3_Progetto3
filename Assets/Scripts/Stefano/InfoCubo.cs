using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCubo : MonoBehaviour {

	public GameObject oggettoManager;
	private IstanziareOggetto istanzia;
	private MyGameManager G_manager;

	void Start()
	{

		oggettoManager = GameObject.FindGameObjectWithTag ("GameManager");
		istanzia = oggettoManager.GetComponent<IstanziareOggetto> ();
		G_manager = oggettoManager.GetComponent<MyGameManager> ();

	}

	//prendiamo il riferimento dell'oggetto da posizionare in base al turno 
	void OnMouseUp()
	{

		//turno player 2
		/*if (G_manager.GetTurno () % 2 == 0 && gameObject.layer == LayerMask.NameToLayer("Player 2")) 
		{
			
			istanzia.oggetto = gameObject;

		} //turno player 1
		else if(G_manager.GetTurno () % 2 != 0 && gameObject.layer == LayerMask.NameToLayer("Player 1"))
		{

			istanzia.oggetto = gameObject;

		}*/


		istanzia.SetOggettoIstanzia (gameObject);



	}
}
