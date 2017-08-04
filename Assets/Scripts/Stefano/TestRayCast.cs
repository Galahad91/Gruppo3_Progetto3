using UnityEngine;
using System.Collections;

public class TestRayCast : MonoBehaviour
{

	[Header("Layer per la casella P2 Torri")]
	public string Layer_Casella_P2_Torri = "Torre P2";
	[Header("Layer per la casella P2 Personaggi")]
	public string Layer_Casella_P2_Personaggi = "Torre P2";


	[Header("Layer per la casella P1 Torri")]
	public string Layer_Casella_P1_Torri = "Torre P1";
	[Header("Layer per la casella P1 Personaggi")]
	public string Layer_Casella_P1_Personaggi = "Torre P1";

	[Header("Oggetti per il buon funzionamento dello script")]
	public GameObject Pedina; //oggetto da istanziare 
	public GameObject Ogg_Gestore; //oggetto che contiene il riferimento del gestore
	private GestoreGioco gestore;

	[Header("Includere i layers che vuoi che collida con il raycast")]
	public LayerMask layers;

	void Awake()
	{

		gestore = Ogg_Gestore.GetComponent<GestoreGioco> ();

	}

	// Update is called once per frame
	void Update ()
	{
		
		Ray puntatore = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay(puntatore.origin, (puntatore.direction - Camera.main.transform.position));

		if (Input.GetMouseButtonDown (0)) 
		{
			
			RaycastHit hit;

			//Ray puntatore = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Ray puntatore = new Ray (Input.mousePosition, Vector3.down);

			//Debug.DrawRay (Input.mousePosition, Vector3.down);

			if (Physics.Raycast (puntatore, out hit, layers) && Pedina != null && gameObject.GetComponent<GestoreGioco> ().IsFaseCombattimento () == false)
			{

				if (gestore.GetTurno () % 2 == 0) 
				{
					//Player 2

					if (hit.transform.gameObject.tag == "Libera" && hit.transform.gameObject.layer == LayerMask.NameToLayer (Layer_Casella_P2_Torri) && Pedina.gameObject.layer == LayerMask.NameToLayer ("Player 2") && Pedina.tag == "Torre") 
					{

						hit.transform.gameObject.tag = "Occupata";

						Instantiate (Pedina, new Vector3 (hit.transform.position.x, 1, hit.transform.position.z), Quaternion.identity);
						Debug.Log ("Istanzio Torre");
					} 
					else if (hit.transform.gameObject.tag == "Libera" && hit.transform.gameObject.layer == LayerMask.NameToLayer (Layer_Casella_P2_Personaggi) && Pedina.gameObject.layer == LayerMask.NameToLayer ("Player 2") && Pedina.tag == "Personaggio") 
					{

						hit.transform.gameObject.tag = "Occupata";

						Instantiate (Pedina, new Vector3 (hit.transform.position.x, 1, hit.transform.position.z), Quaternion.identity);
						Debug.Log ("Istanzio Personaggio");
					}
				}
				else
				{
					//Player 1

					if (hit.transform.gameObject.tag == "Libera" && hit.transform.gameObject.layer == LayerMask.NameToLayer (Layer_Casella_P1_Torri) && Pedina.gameObject.layer == LayerMask.NameToLayer("Player 1") && Pedina.tag == "Torre") {

						hit.transform.gameObject.tag = "Occupata";

						Instantiate (Pedina, new Vector3 (hit.transform.position.x, 1, hit.transform.position.z), Quaternion.identity);
						Debug.Log ("Istanzio Torre");
					}
					else if (hit.transform.gameObject.tag == "Libera" && hit.transform.gameObject.layer == LayerMask.NameToLayer (Layer_Casella_P1_Personaggi) && Pedina.gameObject.layer == LayerMask.NameToLayer ("Player 1") && Pedina.tag == "Personaggio") 
					{

						hit.transform.gameObject.tag = "Occupata";

						Instantiate (Pedina, new Vector3 (hit.transform.position.x, 1, hit.transform.position.z), Quaternion.identity);
						Debug.Log ("Istanzio Personaggio");
					}

				}

			}

		}
				
	}

	/// <summary>
	/// Stesso metodo di quello sopra solo che viene richiamato da bottone
	/// </summary>
	/// <param name="oggetto">Oggetto.</param>
	public void ScegliClasse(GameObject oggetto)
	{

		Pedina = oggetto;
		Debug.Log ("Personaggio assegnato");

		//Debug.Log ("Premuto");

	}

}

