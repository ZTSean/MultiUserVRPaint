using UnityEngine;
using System.Collections;

public class Player
{

	private int user_id;

	public int User_id {
		get {
			return user_id;
		}

		set {
			user_id = value;
		}
	}


	private Painting paintingComponent;

	public Painting PaintingComponent {
		get {
			return paintingComponent;
		}

		set {
			paintingComponent = value;
		}
	}




	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
