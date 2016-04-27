using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using MinVR;
using UnityEngine.UI;


/** This is the main class for the Paint3D application.  It listens for VREvents from the
 * MinVR server in order to update the tracker data for the brush, hand, and head each frame.
 * It also uses the FixedUpdate() callback to create new '3D paint strokes' when the brush 
 * button is pressed.
 */
public class Paint3DMain : MonoBehaviour
{
	// Current User
	[Tooltip ("This GameObject will move around with the tracker attached to the Brush.")]
	public GameObject brushCursor;
	[Tooltip ("This GameObject will move around with the tracker attached to the Hand.")]
	public GameObject handCursor;
	// The other user
	public GameObject brushCursor2;
	public GameObject handCursor2;

	// GameObject contain all menus
	public GameObject MenuContainer;

	// holding all strokes of one user
	public GameObject painting;
	public GameObject painting2;

	// painting script class attached to painting gameobject
	private Painting paintingComponent;
	private Painting paintingComponent2;

	private bool user1StartDrawing = false;
	private bool user2StartDrawing = false;

	// For fake line drawing test use
	private bool isMousePressed;

	private int user_id;

	public Text hPos;
	public Text mPos;
	public Text bPos;
	public Text drawingDebug;

	void Start ()
	{
		VRMain.VREventHandler += OnVREvent;
		isMousePressed = false;
		paintingComponent = painting.GetComponent <Painting> ();
		paintingComponent2 = painting2.GetComponent <Painting> ();
		/*
		Debug.Log (paintingComponent);
		if (paintingComponent is Painting) {
			Debug.Log ("found painting");
		} else {
			Debug.Log ("does not found painting");
		}
		*/
		// TODO: catch if painting Component is null exception

		Debug.Log ("Create default Menu");
		MenuManager.CreateDefaultMenu (MenuContainer);

		GameObject tmp = GameObject.Find ("MinVRUnityClient/VRCameraPair");
		user_id = tmp.GetComponent<VRCameraPairHeadTracking> ().user_id;
	}


	void FixedUpdate ()
	{
		// Fake mouse simulation
		//====================================================================
		// If brush button is down, then add to the painting
		// ---------------------test drawing using mouse left button
		if (Input.GetMouseButtonDown (0)) {
			/*
			paintingComponent.startNewStroke (painting, brushCursor);
			Vertex v = new Vertex (
				           brushCursor.transform.position,
				           brushCursor.transform.rotation,
				           1,
				           new Vector3 (0, 0, 0)
			           );
			paintingComponent.AddVertex (v);

			isMousePressed = true;
			*/

		}

		if (Input.GetMouseButtonUp (0)) {
			isMousePressed = false;
			paintingComponent.EndStroke ();
		}
		// ------------------------------------------------------------

		// If the hand button is down, then grab on to the painting and move it about
		if (Input.GetMouseButton (1)) {
			Vector3 posChange = handPos - lastHandPos;
			Quaternion rotChange = Quaternion.Inverse (handRot) * lastHandRot;
			GameObject painting = GameObject.Find ("Painting");
			painting.transform.position += posChange;
			painting.transform.rotation *= rotChange;

			MenuManager.CurMenu.GetComponent<Menu> ().Clicked (brushCursor.transform.position);
		}

		if (isMousePressed) {
			// when the left mouse button pressed
			// continue to add vertex to line renderer
			Vertex v = new Vertex (
				           brushCursor.transform.position,
				           brushCursor.transform.rotation,
				           1,
				           new Vector3 (0, 0, 0)
			           );
			paintingComponent.AddVertex (v);
		}
		//====================================================================

		// Real time cave drawing
		//====================================================================
		if (user1StartDrawing) {
			drawingDebug.text = "Drawing user 1";
			if (user_id == 1 && !MenuManager.ShowMenu) {
				Vertex v = new Vertex (
					           brushCursor.transform.position,
					           brushCursor.transform.rotation,
					           1,
					           new Vector3 (0, 0, 0)
				           );
				paintingComponent.AddVertex (v);
			}
		}

		if (user2StartDrawing) {
			if (user_id == 2 && !MenuManager.ShowMenu) {
				Vertex v = new Vertex (
					           brushCursor2.transform.position,
					           brushCursor2.transform.rotation,
					           1,
					           new Vector3 (0, 0, 0)
				           );
				paintingComponent2.AddVertex (v);
			}
		}
		//====================================================================

		// Save the current state of the hand
		lastHandPos = handPos;
		lastHandRot = handRot;

		// test
		//MenuManager.drawDebugLine (GameObject.Find ("MenuContainer").transform.position, GameObject.Find ("MenuContainer").transform.position + new Vector3 (0, -10, 0), Color.blue);
		MenuManager.isHover (brushCursor.transform.position);
	}


	// This function gets called every time a new VREvent is generated.  Typically, VREvents will come
	// from the MinVR server, which polls trackers, buttons, and other input devices for input.  When
	// debugging on your laptop, you can also generate 'fake' VREvents using the VRMain script.
	void OnVREvent (VREvent e)
	{
		

		// USER 2 ==========================================================
		if (e.Name == "Brush2_Move") {		
			Matrix4x4 m = VRConvert.ToMatrix4x4 (e.DataIndex.GetValueAsDoubleArray ("Transform"));
			brushPos = m.GetTranslation ();
			brushRot = m.GetRotation ();
			brushCursor2.transform.position = brushPos;
			brushCursor2.transform.rotation = brushRot;

			// Menu Selection ----------------------------------------------
			if (MenuManager.ShowMenu && user_id == 2) {
				// Color change when hover on the button
				MenuManager.isHover (brushCursor2.transform.position);

			}
		} else if (e.Name == "Hand2_Move") {
			Matrix4x4 m = VRConvert.ToMatrix4x4 (e.DataIndex.GetValueAsDoubleArray ("Transform"));
			handPos = m.GetTranslation ();
			handRot = m.GetRotation ();
			handCursor2.transform.position = handPos;
			handCursor2.transform.rotation = handRot;
		} else if (e.Name == "Head2_Move") {
			Matrix4x4 m = VRConvert.ToMatrix4x4 (e.DataIndex.GetValueAsDoubleArray ("Transform"));
			headPos = m.GetTranslation ();
			headRot = m.GetRotation ();

		} 
		// Button Controls --------------------------------------------------
		else if (e.Name == "stylus0_btn1_up") {
			// stylys 0: blue pen

			if (user_id == 2) {
				// Show Main Menu
				if (MenuManager.CurMenu == null) {
					// Open Main Menu
					Menu mainMenu = null;
					//Debug.Log (MenuManager.Menus.Count);
					foreach (var item in MenuManager.Menus) {
						if (item.gameObject.name == "MainMenu") {
							mainMenu = item;
							break;
						}
					}

					if (mainMenu == null) {
						// TODO: catch could not find error
					} else {
						/*
						mainMenu.ShowMenu = true;
						mainMenu.gameObject.SetActive (true);
						MenuManager.CurMenu = mainMenu.gameObject;
						MenuManager.ShowMenu = true;
						Vector3 hpos = GameObject.Find ("User2").transform.position;
						MenuContainer.transform.position = new Vector3 (hpos.x, hpos.y - 70, hpos.z);
						*/
						Vector3 hpos = GameObject.Find ("User2").transform.position;

						MenuManager.OpenMenu (mainMenu, new Vector3 (hpos.x, hpos.y - 2, hpos.z), MenuContainer);
					}

				} else {
					MenuManager.CloseMenu ();
				}
			}



		} else if (e.Name == "stylus0_btn0_down") {
			if (user_id == 2) {
				if (!MenuManager.ShowMenu) {
					paintingComponent2.startNewStroke (painting2, brushCursor2);
				}
			} else if (user_id == 1) {
				// determine user 2 whether menu is open
			}

		} else if (e.Name == "stylus0_btn0_up") {
			if (user_id == 2) {
				if (MenuManager.ShowMenu) {
					// Call the function when the button is selected
					MenuManager.CurMenu.GetComponent<Menu> ().Clicked (brushCursor2.transform.position);
				} else {
					paintingComponent2.EndStroke ();
				}
			}

		}

		// USER 1 ==========================================================
		else if (e.Name == "Brush1_Move") {		
			Matrix4x4 m = VRConvert.ToMatrix4x4 (e.DataIndex.GetValueAsDoubleArray ("Transform"));
			brushPos = m.GetTranslation ();
			brushRot = m.GetRotation ();
			brushCursor.transform.position = brushPos;
			brushCursor.transform.rotation = brushRot;

			if (MenuManager.ShowMenu && user_id == 1) {
				// Color change when hover on the button
				MenuManager.isHover (brushCursor.transform.position);
			} 
		} else if (e.Name == "Hand1_Move") {
			Matrix4x4 m = VRConvert.ToMatrix4x4 (e.DataIndex.GetValueAsDoubleArray ("Transform"));
			handPos = m.GetTranslation ();
			handRot = m.GetRotation ();
			handCursor.transform.position = handPos;
			handCursor.transform.rotation = handRot;
		} else if (e.Name == "Head1_Move") {
			Matrix4x4 m = VRConvert.ToMatrix4x4 (e.DataIndex.GetValueAsDoubleArray ("Transform"));
			headPos = m.GetTranslation ();
			headRot = m.GetRotation ();
		} 
		// Button Controls --------------------------------------------------
		else if (e.Name == "stylus1_btn1_up") {
			// stylys 1: red pen

			if (user_id == 1) {
				// Show Main Menu
				if (MenuManager.CurMenu == null) {
					// Open Main Menu
					Menu mainMenu = null;
					Debug.Log (MenuManager.Menus.Count);
					foreach (var item in MenuManager.Menus) {
						if (item.gameObject.name == "MainMenu") {
							mainMenu = item;
							break;
						}
					}

					if (mainMenu == null) {
						// TODO: catch could not find error
					} else {
						/*
						mainMenu.ShowMenu = true;
						mainMenu.gameObject.SetActive (true);
						MenuManager.CurMenu = mainMenu.gameObject;
						MenuManager.ShowMenu = true;
						Vector3 hpos = GameObject.Find ("User1").transform.position;
						MenuContainer.transform.position = new Vector3 (hpos.x, hpos.y - 70, hpos.z);
						*/

						Vector3 hpos = GameObject.Find ("User1").transform.position;

						MenuManager.OpenMenu (mainMenu, new Vector3 (hpos.x, hpos.y - 2, hpos.z), MenuContainer);
					}

				} else {
					MenuManager.CloseMenu ();
				}
			}

		} else if (e.Name == "stylus1_btn0_down") {
			if (user_id == 1) {
				if (!MenuManager.ShowMenu) {
					paintingComponent.startNewStroke (painting, brushCursor);	
				}
			} else if (user_id == 2) {
				// determine user 1 whether menu is open
			}
		} else if (e.Name == "stylus1_btn0_up") {
			if (user_id == 1) {
				if (MenuManager.ShowMenu) {
					// Call the function when the button is selected
					MenuManager.CurMenu.GetComponent<Menu> ().Clicked (brushCursor.transform.position);
				} else {
					paintingComponent.EndStroke ();
				}
			}
				
		}
		// Client input events -----------------------------------------------
		else if (e.Name == "Type_Change") {
			if (user_id == 1) {
				
			} else if (user_id == 2) {
				
			}
		} else if (e.Name == "Color_Change") {
			if (user_id == 1) {

			} else if (user_id == 2) {

			}
		} else if (e.Name == "Start_Collaboration") {
			if (user_id == 1) {

			} else if (user_id == 2) {

			}
		}
	}



	// This defines a public property of the Paint3D class that you can access in other scripts.
	// So, in any other script, you can write Paint3D.BrushPos in order to access the current
	// position of the brush.
	public Vector3 BrushPos {
		get {
			return brushPos; 
		}
		set {
			brushPos = value;
		}
	}

	private Vector3 brushPos;

	// Same for brush rotation
	public Quaternion BrushRot {
		get {
			return brushRot; 
		}
		set {
			brushRot = value;
		}
	}

	private Quaternion brushRot;

	// Same for hand position
	public Vector3 HandPos {
		get {
			return handPos; 
		}
		set {
			handPos = value;
		}
	}

	private Vector3 handPos;
	private Vector3 lastHandPos;

	// Same for hand rotation
	public Quaternion HandRot {
		get {
			return handRot; 
		}
		set {
			handRot = value;
		}
	}

	private Quaternion handRot;
	private Quaternion lastHandRot;

	// Same for head position
	public Vector3 HeadPos {
		get {
			return headPos; 
		}
		set {
			headPos = value;
		}
	}

	private Vector3 headPos;

	// Same for head rotation
	public Quaternion HeadRot {
		get {
			return headRot; 
		}
		set {
			headRot = value;
		}
	}

	private Quaternion headRot;



}