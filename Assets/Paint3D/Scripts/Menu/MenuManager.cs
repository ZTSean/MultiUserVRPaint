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

		// ---- For test use -----
		//mc.ShowMenu = true;
		//MenuManager.ShowMenu = true;
		//CurMenu = newMenu;

		// Initialize new Created Menu position and size
		mc.Initialize ();

		// Initialize button in new created Menu
		mc.InitializeButtions (btnFunctions);

		Menus.Add (mc);
		newMenu.SetActive (false);

	}

	//===================================================
	// Menu selection methods

	public static void isHover (Vector3 pos)
	{
		CurMenu.GetComponent<Menu> ().isHover (pos);
	}


	//===================================================
	// All Button Functionalities

	//---------------------------------------------------
	// First Level
	public static void ShowColorMap (GameObject currentMenu)
	{
		// Hide menu this function attached to
		currentMenu.SetActive (false);
		MenuManager.ShowMenu = false;
		MenuManager.CurMenu = null;

		// Show Color Picker map

		Painting paintingComponent = GameObject.Find ("Painting").GetComponent<Painting> ();
		// TODO: catch when could not find painting

		Dictionary<string, object> curOptions = paintingComponent.CurStroke.Brush.GetOptions ();

		curOptions ["StartColor"] = Color.red;
		curOptions ["EndColor"] = Color.red;

		paintingComponent.CurStroke.Brush.SetOptions (curOptions);

	}

	public static void ShowChangeBrushTypeMenu (GameObject currentMenu)
	{
		// Show BrushType options
		//CreateMenu (currentMenu);


	}

	public static void StartCollaboration (GameObject currentMenu)
	{
		
	}

	//---------------------------------------------------
	// Second Level
	public static void ChangeColor (GameObject currentMenu)
	{
		
	}

	public static void ChangeBrushType (GameObject currentMenu)
	{
		
	}


}
