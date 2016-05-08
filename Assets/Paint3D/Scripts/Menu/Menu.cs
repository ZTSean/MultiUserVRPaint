using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
	// Indicate showing status of menu
	private bool showMenu;

	public bool ShowMenu {
		get {
			return showMenu;
		}
		set {
			showMenu = value;
		}
	}

	// Current using brush
	private BrushType brushType;

	private List<GameObject> buttons;

	public List<GameObject> Buttons {
		get {
			if (buttons == null) {
				buttons = new List<GameObject> ();
			}

			return buttons;
		}
	}

	private GameObject selectedButton;

	public GameObject SelectedButton {
		get {
			if (selectedButton == null) {
				return null;
			}

			return selectedButton;
		}
		set {
			selectedButton = value;
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


	//========================================================
	public void Initialize ()
	{
		// TODO: Set the width and heigh of menu according to camera scale
	}

	// Create different menu items
	public void InitializeButtions (Dictionary<string, buttonClick> buttonClickFunctions)
	{
		// Divide the menu into 9 square and create button for each square
		RectTransform canvasRect = gameObject.GetComponent<RectTransform> ();
		var menuWidth = canvasRect.sizeDelta.x;
		var menuHeight = canvasRect.sizeDelta.y;

		// Find menu item prefab
		string path = "Menu/MenuItem";
		GameObject prefab = (GameObject)Resources.Load (path);

		// Calculate button size
		var itemMenuWidth = menuWidth / 3;
		var itemMenuHeight = menuHeight / 3;

		for (int i = 0; i < 9; i++) {
			GameObject newMenuItem = (GameObject)Instantiate (prefab) as GameObject;

			// reset width and heigh of newMenuItem to 1/3 of Menu
			newMenuItem.GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemMenuWidth, itemMenuHeight);
			newMenuItem.name = "Button_" + i.ToString ();
			// set position of newMenuItem according to index
			int div = i / 3;
			int mod = i % 3;

			// World space position!!!!
			newMenuItem.GetComponent<RectTransform> ().position = new Vector3 ((div - 1) * itemMenuWidth, (mod - 1) * itemMenuHeight, 0);

			// Set Box Collider Size
			newMenuItem.GetComponent<BoxCollider> ().size = new Vector3 (itemMenuWidth, itemMenuHeight, 1);

			// Set newMenuItem as child of Menu: set parent after set position will modify world space to local relative
			newMenuItem.transform.SetParent (gameObject.transform, false);

			Buttons.Add (newMenuItem);
		}


		// Register functions
		for (int i = 0; i < Buttons.Count && i < buttonClickFunctions.Count; i++) {
			Button b = Buttons [i].GetComponent<Button> ();

			// Rename button name
			string temp = buttonClickFunctions.Keys.ElementAt (i);
			b.GetComponentInChildren<Text> ().text = temp;

			b.GetComponent<Item> ().Clicked = buttonClickFunctions.Values.ElementAt (i);
			//b.GetComponent<Item> ().Clicked = new buttonClick (testClick);

			// Add listener: hide the menu when choice been made
			Debug.Log ("Function added: " + buttonClickFunctions.Keys.ElementAt (i));
		}
	}

	public void isHover (Vector3 pos)
	{
		// Objects with colliders being hit by ray
		RaycastHit[] hits;
		// Only against colliders in layer 8
		int layerMask = 1 << 8;

		hits = Physics.RaycastAll (pos, new Vector3 (0, -1, 0), Mathf.Infinity, layerMask);

		// Test whether there is a collision
		if (hits.Length > 0) {
			/* test use
			Debug.DrawRay (pos, new Vector3 (0, -1, 0), Color.blue);
			foreach (var item in hits) {
				Debug.Log (item.collider.gameObject.name);
			}*/

			// if hover (collide), change to hover color
			GameObject tmp = hits.First ().collider.gameObject;
			tmp.GetComponent<Button> ().Select ();
			SelectedButton = tmp;

			Debug.Log ("Add button selected: " + tmp.name);

			//Debug.DrawLine (pos, pos + new Vector3 (0, -10, 0), Color.blue); // test use
			//Debug.DrawRay (pos, new Vector3 (0, -1, 0), Color.blue); // test use
			MenuManager.drawDebugLine2 (pos, pos + new Vector3 (0, -10, 0), Color.blue);
		} else {
			hits = Physics.RaycastAll (pos, new Vector3 (0, 1, 0), Mathf.Infinity, layerMask);

			if (hits.Length > 0) {

				// if hover (collide), change to hover color
				GameObject tmp = hits.First ().collider.gameObject;
				tmp.GetComponent<Button> ().Select ();
				SelectedButton = tmp;

			} else {
				// remove "select" state from all button
				foreach (var item in buttons) {
					item.GetComponent<Button> ().navigation = Navigation.defaultNavigation;
				}

				//Debug.DrawRay (pos, new Vector3 (0, -1, 0), Color.black); // test use
				//Debug.DrawLine (pos, pos + new Vector3 (0, -10, 0), Color.red); // test use
				MenuManager.drawDebugLine2 (pos, pos + new Vector3 (0, -10, 0), Color.red);

				//Debug.Log ("remove button selected");
				SelectedButton = null;
			}
		}



		//MenuManager.drawDebugLine (GameObject.Find ("MenuContainer").transform.position, GameObject.Find ("MenuContainer").transform.position + new Vector3 (0, 10, 0), Color.green);
			


	}

	public void Clicked (Vector3 pos)
	{
		if (SelectedButton == null) {
			// TODO: no button has been selected
			//Debug.Log ("no selected button");
		} else {
			// if hover (collide), change to hover color
			SelectedButton.GetComponent<Item> ().Clicked (gameObject);

			//Debug.Log ("button has been selected");
		}

	}

	public void testClick (GameObject obj)
	{
		Text temp = GameObject.Find ("MinVRUnityClient/VRCameraPair/DrawingDebug").GetComponent<Text> ();
		temp.GetComponent<Text> ().text = ("Success add: " + obj.name);
	}

}
