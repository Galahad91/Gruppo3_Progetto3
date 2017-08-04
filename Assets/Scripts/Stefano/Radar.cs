using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour 
{
	 
	[SerializeField]
	private List<string> Lista_Nome_Nemico = new List<string> ();

	[SerializeField]
	private GameObject[] Nemici_torre;
	private int indice = 0; //Indice del vettore di gameobject

	void Awake()
	{

		Nemici_torre = new GameObject[100];

	}

	void Update()
	{
		

		//Forziamo la lista a resettarsi
		if (Input.GetKeyDown (KeyCode.A)) 
		{

			ResetListaNemici ();

		}
			

	}

	/// <summary>
	/// Riempiamo la lista dei nemici in zona secondo certi parametri (Per ora funziona solo con Assasin)
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerEnter(Collider other)
	{

		if (transform.parent.tag == "Tower") 
		{

			if (other.tag == "Assassin") 
			{


				Nemici_torre[indice] = other.gameObject;
				indice++;

			}

		} 
		else 
		{

			if (other.tag == "Assassin") 
			{

				if (other.GetComponent<Characters> ().isFighting != true) 
				{

					Lista_Nome_Nemico.Add (other.name);

				}

			}

		}
	}

	/// <summary>
	/// Metodo che restituisce la lista dei nemici in zona
	/// </summary>
	/// <returns>The lista nemici.</returns>
	public List<string> GetListaNemici()
	{

	
		return Lista_Nome_Nemico;
	
	}

	/// <summary>
	/// Puliamo la lista e la reinizializiamo 
	/// </summary>
	public void ResetListaNemici()
	{

		Lista_Nome_Nemico.Clear ();
	}


	/// <summary>
	/// Metodo che restituisce il vettore di nemici da colpire da una torre
	/// </summary>
	/// <returns> Vettore di nemici</returns>
	public GameObject[] GetVettoreNemici()
	{
		
		return Nemici_torre;

	}


	/// <summary>
	/// Resetta vettore dei nemici
	/// </summary>
	public void ResetVettoreNemici()
	{

		for (int i = 0; i < Nemici_torre.Length - 1; i++) 
		{

			Nemici_torre [i] = null;

		}

		indice = 0;
	}
}
