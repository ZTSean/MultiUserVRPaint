using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{

	private buttonClick clicked;

	public buttonClick Clicked {
		get { return clicked; }
		set {
			clicked = value;
		}
	}
}
