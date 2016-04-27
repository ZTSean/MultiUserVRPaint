using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Main Menu Options
using UnityEngine.UI;


enum MenuChoices
{
	ChangeColor,
	ChangeBrushType,
	Collaboration
}

public delegate void buttonClick (GameObject currentMenu);

public class MenuManager
{
	// Contain all of menus
	private static List<Menu> menus;

	public static List<Menu> Menus {
		get {
			if (menus == null) {
				menus = new List<Menu> ();
			}

			return menus;
		}
	}

	private static bool showMenu;

	public static bool ShowMenu {
		get {
			return showMenu;
		}

		set {
			showMenu = value;
		}
	}

	private static GameObject curMenu;

	public static GameObject CurMenu {
		get {
			return (curMenu == null) ? null : curMenu;
		}

		set {
			curMenu = value;
		}
	}

	//===================================================
	// MenuContainer: Gameobject hold sub-menus
	public static void CreateDefaultMenu (GameObject MenuContainer)
	{
		// Create list of functions
		Dictionary<string, buttonClick> btnFunctions = new Dictionary<string, buttonClick> ();

		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		// Add Main Menu functions to the list
		btnFunctions.Add ("Change Color", new buttonClick (ShowColorMap));
		btnFunctions.Add ("Change Brush", new buttonClick (ShowChangeBrushTypeMenu));
		btnFunctions.Add ("Collaboration", new buttonClick (StartCollaboration));
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

		CreateMenu (MenuContainer, btnFunctions, "MainMenu");
	}


	public static void CreateMenu (GameObject MenuContainer, Dictionary<string, buttonClick> btnFunctions, string name)
	{
		// Find menu prefab
		// Path: relative to Resources folder
		string path = "Menu/Menu";

		GameObject prefab = (GameObject)Resources.Load (path, typeof(GameObject));

		GameObject newMenu = GameObject.Instantiate (prefab, MenuContainer.transform.position, MenuContainer.transform.rotation) as GameObject;
		newMenu.name = name; 
		newMenu.AddComponent <Menu> ();

		// Rotate Menu to zox plane
		newMenu.transform.Rotate (90, 0, 0, Space.Self);

		newMenu.transform.SetParent (MenuContainer.transform, false);

		Menu mc = newMenu.GetComponent <Menu> ();

		// Initialize new Created Menu position and size
		mc.Initialize ();

		// Initialize button in new created Menu
		mc.InitializeButtions (btnFunctions);

		Menus.Add (mc);
		newMenu.SetActive (false);

		// ---- For test use -----

		mc.gameObject.SetActive (true);
		MenuManager.ShowMenu = true;
		CurMenu = newMenu;


	}

	//===================================================
	// Menu selection methods

	public static void isHover (Vector3 pos)
	{
		CurMenu.GetComponent<Menu> ().isHover (pos);
	}


	//===================================================
	// All Button Functionalities

	//--------------------------------------------------- First Level
	public static void ShowColorMap (GameObject currentMenu)
	{
		/*
		// Hide menu this function attached to
		MenuManager.CloseMenu ();

		// TODO: Show Color Picker map

		Painting paintingComponent = GameObject.Find ("Painting").GetComponent<Painting> ();
		// TODO: catch when could not find painting

		Dictionary<string, object> curOptions = paintingComponent.CurStroke.Brush.GetOptions ();

		curOptions ["StartColor"] = Color.red;
		curOptions ["EndColor"] = Color.red;

		paintingComponent.CurStroke.Brush.SetOptions (curOptions);
		*/

		Text temp = GameObject.Find ("MinVRUnityClient/VRCameraPair/DrawingDebug").GetComponent<Text> ();
		temp.GetComponent<Text> ().text = ("Success add: " + currentMenu.name);

	}

	public static void ShowChangeBrushTypeMenu (GameObject currentMenu)
	{
		// Show BrushType options
		//CreateMenu (currentMenu);
		Text temp = GameObject.Find ("MinVRUnityClient/VRCameraPair/DrawingDebug").GetComponent<Text> ();
		temp.GetComponent<Text> ().text = ("Success add: " + currentMenu.name);

	}

	public static void StartCollaboration (GameObject currentMenu)
	{
		Text temp = GameObject.Find ("MinVRUnityClient/VRCameraPair/DrawingDebug").GetComponent<Text> ();
		temp.GetComponent<Text> ().text = ("Success add: " + currentMenu.name);
	}

	//--------------------------------------------------- Second Level
	public static void ChangeColor (GameObject currentMenu)
	{
		
	}

	public static void ChangeBrushType (GameObject currentMenu)
	{
		
	}

	// Debug Functions
	public static void drawDebugLine (Vector3 start, Vector3 dir, Color c)
	{
		GameObject tmp = GameObject.Find ("DebugLine");

		if (tmp.GetComponent<LineRenderer> () == null) {
			tmp.AddComponent<LineRenderer> ();
		}
		LineRenderer lr = tmp.GetComponent<LineRenderer> ();
		lr.SetWidth ((float)0.01, (float)0.01);
		lr.SetColors (c, c);
		lr.SetVertexCount (2);

		lr.SetPosition (0, start);
		lr.SetPosition (1, start + dir);


	}

	public static void drawDebugLine2 (Vector3 start, Vector3 dir, Color c)
	{
		GameObject tmp = GameObject.Find ("DebugLine2");

		if (tmp.GetComponent<LineRenderer> () == null) {
			tmp.AddComponent<LineRenderer> ();
		}
		LineRenderer lr = tmp.GetComponent<LineRenderer> ();
		lr.SetWidth ((float)0.01, (float)0.01);
		lr.SetColors (c, c);
		lr.SetVertexCount (2);

		lr.SetPosition (0, start);
		lr.SetPosition (1, start + dir);


	}

	public static void OpenMenu (Menu m, Vector3 pos, GameObject mContainer)
	{
		m.ShowMenu = true;
		m.gameObject.SetActive (true);
		MenuManager.CurMenu = m.gameObject;
		MenuManager.ShowMenu = true;

		mContainer.transform.position = pos;
	}

	public static void CloseMenu ()
	{
		MenuManager.CurMenu.GetComponent<Menu> ().ShowMenu = false;
		MenuManager.CurMenu.SetActive (false);
		MenuManager.CurMenu = null;
		MenuManager.ShowMenu = false;
	}
}
