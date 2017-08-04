using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCasella : MonoBehaviour {

	/*public GameObject oggettoManager;
	private IstanziareOggetto istanzia;
	private GestoreGioco gestore;

	public bool isCamminabile;
	public float x;
	public float z;

	[Header("Layer dove istanziare le torri (numerico)")]
	public int Layer_Torri; //Nome del layer in cui si possono istanziare le torri
	

	void Start()
	{

		oggettoManager = GameObject.FindGameObjectWithTag ("GameManager");
		istanzia = oggettoManager.GetComponent<IstanziareOggetto> ();
		gestore = oggettoManager.GetComponent<GestoreGioco> ();

	}

	void OnMouseUp()
	{

		if (gameObject.layer == LayerMask.NameToLayer ("Player 1") && gestore.GetTurno () % 2 != 0 && istanzia.GetOggettoIstanzia ().layer == gameObject.layer) {
			
			//Debug.Log ("click");
			istanzia.x = gameObject.transform.position.x;
			istanzia.z = gameObject.transform.position.z;

			if (gameObject.tag == "Corsia1") {

				istanzia.SetTorreTarget (gestore.Torre1_Player1);

			}
			if (gameObject.tag == "Corsia2") {

				istanzia.SetTorreTarget (gestore.Torre2_Player1);

			}
			if (gameObject.tag == "Corsia3") {

				istanzia.SetTorreTarget (gestore.Torre3_Player1);

			}


		} else if (gameObject.layer == LayerMask.NameToLayer ("Player 2") && gestore.GetTurno () % 2 == 0 && istanzia.GetOggettoIstanzia ().layer == gameObject.layer) {
			
			//Debug.Log ("click");
			istanzia.x = gameObject.transform.position.x;
			istanzia.z = gameObject.transform.position.z;

			if (gameObject.tag == "Corsia1") {

				istanzia.SetTorreTarget (gestore.Torre1_Player2);

			}
			if (gameObject.tag == "Corsia2") {

				istanzia.SetTorreTarget (gestore.Torre2_Player2);

			}
			if (gameObject.tag == "Corsia3") {

				istanzia.SetTorreTarget (gestore.Torre3_Player2);

			}

		} 
		else if (gameObject.layer == Layer_Torri) 
		{

			Debug.Log ("Istanzio le torri");

			istanzia.x = gameObject.transform.position.x;
			istanzia.z = gameObject.transform.position.z;

		}



	}

*/
}
