using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour 
{

	public List<string> Lista_Nome_Nemico = new List<string> ();

	void Awake()
	{

		//Lista_Nome_Nemico.Add ("franco");

	}

	void Update()
	{
		

		if (Input.GetKeyDown (KeyCode.A)) 
		{

			ResetListaNemici ();

		}
			

	}

	//Metodo che riempie la lista di nomi dei nemici in zona
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Assassin") 
		{
			Lista_Nome_Nemico.Add (other.name);
		}
	}

	//Metodo che restituisce la lista di nomi di nemici 
	public List<string> GetListaNemici()
	{

		if (Lista_Nome_Nemico.Count > 0) {

			Debug.LogError ("Elementi lista: "+Lista_Nome_Nemico.Count);

			return Lista_Nome_Nemico;

		}



		return null;
	}

	//Pulisco la lista di nemici e la reinizializzo
	public void ResetListaNemici()
	{

		Lista_Nome_Nemico.Clear ();
	}
}
