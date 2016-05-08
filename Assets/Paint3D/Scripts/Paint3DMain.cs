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
	// collaborate painting work shared by all users
	public GameObject collaboratePainting;

	// painting script class attached to painting gameobject
	private Painting paintingComponent;
	private Painting paintingComponent2;
	private Painting paintingComponentCollaborate;

	private bool user1StartDrawing = false;
	private bool user2StartDrawing = false;

	// For fake line drawing test use
	private bool isMousePressed;

	private int user_id;

	public Text hPos;
	public Text mPos;
	public Text bPos;
	public Text drawingDebug;

	private bool otherUserMenu = false;

	void Start ()
	{
		VRMain.VREventHandler += OnVREvent;
		isMousePressed = false;
		paintingComponent = painting.GetComponent <Painting> ();
		paintingComponent2 = painting2.GetComponent <Painting> ();
		paintingComponentCollaborate = collaboratePainting.GetComponent<Painting> ();
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


			// Test: Collaborate work
			VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
			//vrm.AddClientEvent ("TestSyncCollaborate", 1);
			vrm.AddClientEvent ("user1ButtonDown", 1);*/

		}

		if (Input.GetMouseButtonUp (0)) {
			/*
			isMousePressed = false;
			paintingComponent.EndStroke ();
			*/

			// Test: Collaborate work
			//	VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
			//vrm.AddClientEvent ("user1ButtonUp", 1);
		}
		// ------------------------------------------------------------

		// If the hand button is down, then grab on to the painting and move it about
		if (Input.GetMouseButtonDown (1)) {
			/*
			Vector3 posChange = handPos - lastHandPos;
			Quaternion rotChange = Quaternion.Inverse (handRot) * lastHandRot;
			GameObject painting = GameObject.Find ("Painting");
			painting.transform.position += posChange;
			painting.transform.rotation *= rotChange;
			*/

			//MenuManager.CurMenu.GetComponent<Menu> ().Clicked (brushCursor.transform.position);

			/*
			// Test: Collaborate work
			paintingComponent2.startNewStroke (painting2, brushCursor2);
			Vertex v = new Vertex (
				           brushCursor2.transform.position,
				           brushCursor2.transform.rotation,
				           1,
				           new Vector3 (0, 0, 0)
			           );
			paintingComponent2.AddVertex (v);
			paintingComponent2.EndStroke ();

			paintingComponent2.startNewStroke (painting2, brushCursor2);
			paintingComponent2.AddVertex (v);
			paintingComponent2.EndStroke ();

			VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
			//vrm.AddClientEvent ("user2ButtonDown", 2);
			vrm.AddClientEvent ("TestSyncCollaborate", 2);
			*/
		}

		if (Input.GetMouseButtonUp (1)) {
			// Test: Collaborate work
			//VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
			//vrm.AddClientEvent ("user2ButtonUp", 2);
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
			Vertex v = new Vertex (
				           brushCursor.transform.position,
				           brushCursor.transform.rotation,
				           1,
				           new Vector3 (0, 0, 0)
			           );
			paintingComponent.AddVertex (v);
		}

		if (user2StartDrawing) {
			Vertex v = new Vertex (
				           brushCursor2.transform.position,
				           brushCursor2.transform.rotation,
				           1,
				           new Vector3 (0, 0, 0)
			           );
			paintingComponent2.AddVertex (v);
		}
		//====================================================================

		// Save the current state of the hand
		lastHandPos = handPos;
		lastHandRot = handRot;

		// test
		//MenuManager.drawDebugLine (GameObject.Find ("MenuContainer").transform.position, GameObject.Find ("MenuContainer").transform.position + new Vector3 (0, -10, 0), Color.blue);
		//MenuManager.isHover (brushCursor.transform.position);
	}


	// This function gets called every time a new VREvent is generated.  Typically, VREvents will come
	// from the MinVR server, which polls trackers, buttons, and other input devices for input.  When
	// debugging on your laptop, you can also generate 'fake' VREvents using the VRMain script.
	void OnVREvent (VREvent e)
	{
		
		//Debug.Log (e.Name);
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

			// Move current stroke point: for both user 1 and user 2
			else if (MenuManager.Collaboration.IsUser2Start && MenuManager.Collaboration.IsUser1Start) {
				if (MenuManager.Collaboration.IsUser2DrawStart) {
					


					// Test if it is in correct collaborate status => User2StrokeIndex, User2VertexIndex
					if (MenuManager.Collaboration.User2StrokeIndex != -1 && MenuManager.Collaboration.User2VertexIndex != -1) {
						// Find vertex corresponding to current user
						paintingComponentCollaborate.CurStroke.Brush.UpdateVertex (MenuManager.Collaboration.User2VertexIndex, brushCursor2.transform.position);
					}

					/*
					// Sync collaborate class with the other user(*Do not need*)
					VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
					vrm.AddClientEvent ("SyncCollaborate", user_id);
					*/
				}
			}

			// TODO: if Collaborate start, sync collaborate class among different users
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

				VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
				vrm.AddClientEvent ("MenuStatusChange", user_id);
			}



		} else if (e.Name == "stylus0_btn0_down") {
			if (user_id == 2) {
				// Menu Open
				if (!MenuManager.ShowMenu) {
					// Collaborate work
					if (MenuManager.Collaboration.IsUser2Start) {
						// Set position of vertex that you draw following the position of brush
						drawingDebug.text = "isuser2start";

						// If both users are ready, start collaboration
						if (MenuManager.Collaboration.IsUser1Start == true) {
							MenuManager.Collaboration.IsUser2DrawStart = true;
							drawingDebug.text = "isuser1start";

							// Case 1: User 1 start draw and still hold the button => User1StrokeIndex = User2StrokeIndex + 1, isUser1StartDraw = true, stroke has been created
							// User 2 TODO: IsUser2DrawStart = true, User2StrokeIndex + 1

							// Case 2: User 1 end draw and release the button => User1StrokeIndex = User2StrokeIndex + 1, isUser1StartDraw = false
							// User 2 TODO: IsUser2DrawStart = true, User2StrokeIndex + 1

							// Case 3: User 1 haven't start draw => isUser1StartDraw = false, User1StrokeIndex == User2StrokeIndex
							// User 2 TODO: Create new stroke(along with User2StrokeIndex + 1), IsUser2DrawStart = true

							// Case 4: User 1 end draw and release the button => User1StrokeIndex > User2StrokeIndex + 1, isUser1StartDraw = false or true
							// User 2 TODO: clear all collaborate work and restart => IsUser2DrawStart = true

							// Case 5: User 1 start or end draw but user 2 draw much more => User1StrokeIndex + 1 < User2StrokeIndex, isUser1Start = false or true
							// User 2 TODO: clear all collaborate work and restart => isUser2DrawStart = true


							if (MenuManager.Collaboration.User1StrokeIndex == MenuManager.Collaboration.User2StrokeIndex + 1) {
								if (paintingComponentCollaborate.transform.childCount != MenuManager.Collaboration.User1StrokeIndex + 1) {
									// TODO: no corresponding strokes created error because no in time sync with Collaborate class from different users
									Debug.Log ("Error: Number of strokes is not corresponding to index!");
									drawingDebug.text = "Error: # not correct";
								} else {
									// Get most recent stroke created by user 1
									// Case 1 and Case 2
									GameObject s = collaboratePainting.transform.GetChild (MenuManager.Collaboration.User1StrokeIndex).gameObject;
									// Add new vertex to this stroke
									paintingComponentCollaborate.CurStroke = s.GetComponent<Stroke> ();
									Vertex v = new Vertex (
										           brushCursor2.transform.position,
										           brushCursor2.transform.rotation,
										           1,
										           new Vector3 (0, 0, 0)
									           );
									paintingComponentCollaborate.AddVertex (v);
									// Add this new vertex to collaborate vertices
									if (paintingComponentCollaborate.CurStroke.vertices.Count > 0) {
										MenuManager.Collaboration.User2VertexIndex = paintingComponentCollaborate.CurStroke.vertices.Count - 1 + 1;
									} else {
										// TODO: number of vertex error (Current stroke)
									}

									MenuManager.Collaboration.AddVertex (user_id, MenuManager.Collaboration.User2StrokeIndex, MenuManager.Collaboration.User2VertexIndex, v);
									drawingDebug.text = "case 1 or 2";
								}



							}
							// Case 3
							else if (MenuManager.Collaboration.User1StrokeIndex == MenuManager.Collaboration.User2StrokeIndex) {
								if (MenuManager.Collaboration.User1StrokeIndex > -1) {
									// Initilize painting object and class to be ready for painting
									paintingComponentCollaborate.CurStroke = null;
									paintingComponentCollaborate.SelectedBrush = BrushType.LineBrush;
								}

								// Add new stroke to painting
								MenuManager.Collaboration.StartNewStroke (brushCursor2, collaboratePainting, paintingComponentCollaborate, user_id);
								drawingDebug.text = "case 3";

							}
							// Case 4 and Case 5
							else if ((MenuManager.Collaboration.User1StrokeIndex > MenuManager.Collaboration.User2StrokeIndex + 1)
							         || (MenuManager.Collaboration.User1StrokeIndex + 1 < MenuManager.Collaboration.User2StrokeIndex)) {
								// Reset the painting class for collaborate work
								paintingComponentCollaborate.Restart ();
								// Reset collaboration Work
								MenuManager.Collaboration.Restart ();
								MenuManager.Collaboration.StartNewStroke (brushCursor2, collaboratePainting, paintingComponentCollaborate, user_id);

								drawingDebug.text = "case 4 and 5";
							}

							// Sync collaborate class with the other user
							VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
							vrm.AddClientEvent ("SyncCollaborate", user_id);
						}
						// Do nothing when the other user is not ready
					} else {
						// Single user drawing mode
						paintingComponent2.startNewStroke (painting2, brushCursor2);
						Vertex v = new Vertex (
							           brushCursor2.transform.position,
							           brushCursor2.transform.rotation,
							           1,
							           new Vector3 (0, 0, 0)
						           );
						paintingComponent2.AddVertex (v);

						user2StartDrawing = true;
					}

				} else {
					// Menu is open => do nothing
				}


			} else if (user_id == 1) {
				// determine user 2 whether menu is open
				if (!otherUserMenu && !(MenuManager.Collaboration.IsUser2Start)) {
					// Single user drawing mode
					paintingComponent2.startNewStroke (painting2, brushCursor2);
					Vertex v = new Vertex (
						           brushCursor2.transform.position,
						           brushCursor2.transform.rotation,
						           1,
						           new Vector3 (0, 0, 0)
					           );
					paintingComponent2.AddVertex (v);

					user2StartDrawing = true;
				}
			}

		} else if (e.Name == "stylus0_btn0_up") {
			if (user_id == 2) {
				if (MenuManager.ShowMenu) {
					// Call the function when the button is selected
					MenuManager.CurMenu.GetComponent<Menu> ().Clicked (brushCursor2.transform.position);

					MenuManager.CloseMenu ();
				} else {
					if (MenuManager.Collaboration.IsUser2Start) {
						if (MenuManager.Collaboration.IsUser2DrawStart) {
							paintingComponentCollaborate.EndStroke ();
							// Reset count for each user
							MenuManager.Collaboration.User2VertexIndex = -1;

							MenuManager.Collaboration.IsUser2DrawStart = false;
							drawingDebug.text = "Coll end";

						}
					} else {
						paintingComponent2.EndStroke ();
						user2StartDrawing = false;
					}
				}
			} else if (user_id == 1) {
				if (!otherUserMenu && !(MenuManager.Collaboration.IsUser2Start)) {
					// Single user drawing mode
					paintingComponent2.EndStroke ();
					user2StartDrawing = false;
				}
			}

		} else if (e.Name == "User2_Menu_Change") {
			if (user_id == 1) {
				otherUserMenu = (bool)e.DataIndex.GetValue ("User2_Menu_Change");
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

			// Move current stroke point: for both user 1 and user 2
			if (MenuManager.Collaboration.IsUser2Start && MenuManager.Collaboration.IsUser1Start) {
				if (MenuManager.Collaboration.IsUser1DrawStart) {



					// Test if it is in correct collaborate status => User1StrokeIndex, User1VertexIndex
					if (MenuManager.Collaboration.User1StrokeIndex != -1 && MenuManager.Collaboration.User1VertexIndex != -1) {
						// Find vertex corresponding to current user
						paintingComponentCollaborate.CurStroke.Brush.UpdateVertex (MenuManager.Collaboration.User1VertexIndex, brushCursor.transform.position);
					}

					/*
					// Sync collaborate class with the other user(*Do not need*)
					VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
					vrm.AddClientEvent ("SyncCollaborate", user_id);
					*/
				}
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
				// Menu Open
				if (!MenuManager.ShowMenu) {
					// Collaborate work

					if (MenuManager.Collaboration.IsUser1Start) {
						// Set position of vertex that you draw following the position of brush
						drawingDebug.text = "isuser1start";

						// If both users are ready, start collaboration
						if (MenuManager.Collaboration.IsUser2Start == true) {
							MenuManager.Collaboration.IsUser1DrawStart = true;
							drawingDebug.text = "isuser2start";

							if (MenuManager.Collaboration.User2StrokeIndex == MenuManager.Collaboration.User1StrokeIndex + 1) {
								if (paintingComponentCollaborate.transform.childCount != MenuManager.Collaboration.User2StrokeIndex + 1) {
									// TODO: no corresponding strokes created error because no in time sync with Collaborate class from different users
									Debug.Log ("Error: Number of strokes is not corresponding to index!");
									drawingDebug.text = "Error: # not correct";
								} else {
									// Get most recent stroke created by user 2
									// Case 1 and Case 2
									GameObject s = collaboratePainting.transform.GetChild (MenuManager.Collaboration.User2StrokeIndex).gameObject;
									// Add new vertex to this stroke
									paintingComponentCollaborate.CurStroke = s.GetComponent<Stroke> ();
									Vertex v = new Vertex (
										           brushCursor.transform.position,
										           brushCursor.transform.rotation,
										           1,
										           new Vector3 (0, 0, 0)
									           );
									paintingComponentCollaborate.AddVertex (v);
									// Add this new vertex to collaborate vertices
									if (paintingComponentCollaborate.CurStroke.vertices.Count > 0) {
										MenuManager.Collaboration.User1VertexIndex = paintingComponentCollaborate.CurStroke.vertices.Count - 1 + 1;
									} else {
										// TODO: number of vertex error (Current stroke)
									}

									MenuManager.Collaboration.AddVertex (user_id, MenuManager.Collaboration.User1StrokeIndex, MenuManager.Collaboration.User1VertexIndex, v);

									drawingDebug.text = "Case 1 or 2";

								}



							}
							// Case 3
							else if (MenuManager.Collaboration.User1StrokeIndex == MenuManager.Collaboration.User2StrokeIndex) {
								if (MenuManager.Collaboration.User2StrokeIndex > -1) {
									// Initilize painting object and class to be ready for painting
									paintingComponentCollaborate.CurStroke = null;
									paintingComponentCollaborate.SelectedBrush = BrushType.LineBrush;
								}

								// Add new stroke to painting
								MenuManager.Collaboration.StartNewStroke (brushCursor, collaboratePainting, paintingComponentCollaborate, user_id);

								drawingDebug.text = "case 3";


							}
							// Case 4 and Case 5
							else if ((MenuManager.Collaboration.User1StrokeIndex > MenuManager.Collaboration.User2StrokeIndex + 1)
							         || (MenuManager.Collaboration.User1StrokeIndex + 1 < MenuManager.Collaboration.User2StrokeIndex)) {
								// Reset the painting class for collaborate work
								paintingComponentCollaborate.Restart ();
								// Reset collaboration Work
								MenuManager.Collaboration.Restart ();
								MenuManager.Collaboration.StartNewStroke (brushCursor, collaboratePainting, paintingComponentCollaborate, user_id);
								drawingDebug.text = "case 4 and 5";


							}

							// Sync collaborate class with the other user
							VRMain vrm = GameObject.Find ("MinVRUnityClient/VRMain").GetComponent<VRMain> ();
							vrm.AddClientEvent ("SyncCollaborate", user_id);
						}
						// Do nothing when the other user is not ready
					} else {
						// Single user drawing mode
						paintingComponent.startNewStroke (painting, brushCursor);
						Vertex v = new Vertex (
							           brushCursor.transform.position,
							           brushCursor.transform.rotation,
							           1,
							           new Vector3 (0, 0, 0)
						           );
						paintingComponent.AddVertex (v);

						user1StartDrawing = true;
					}
				}


			} else if (user_id == 2) {
				// determine user 1 whether menu is open
				if (!otherUserMenu && !(MenuManager.Collaboration.IsUser1Start)) {
					// Single user drawing mode
					paintingComponent.startNewStroke (painting, brushCursor);
					Vertex v = new Vertex (
						           brushCursor.transform.position,
						           brushCursor.transform.rotation,
						           1,
						           new Vector3 (0, 0, 0)
					           );
					paintingComponent.AddVertex (v);

					user1StartDrawing = true;
				}
			} 
		} else if (e.Name == "stylus1_btn0_up") {
			if (user_id == 1) {
				if (MenuManager.ShowMenu) {
					// Call the function when the button is selected
					MenuManager.CurMenu.GetComponent<Menu> ().Clicked (brushCursor.transform.position);

					MenuManager.CloseMenu ();
				} else {
					if (MenuManager.Collaboration.IsUser1Start) {
						if (MenuManager.Collaboration.IsUser1DrawStart) {
							paintingComponentCollaborate.EndStroke ();
							// Reset count for each user
							MenuManager.Collaboration.User1VertexIndex = -1;

							MenuManager.Collaboration.IsUser1DrawStart = false;
							drawingDebug.text = "Coll end";

						}
					} else {
						paintingComponent.EndStroke ();
						user1StartDrawing = false;
					}
				}
			} else if (user_id == 2) {
				if (!otherUserMenu && !(MenuManager.Collaboration.IsUser1Start)) {
					// Single user drawing mode
					paintingComponent.EndStroke ();
					user1StartDrawing = false;
				}
			}
				
		} else if (e.Name == "User1_Menu_Change") {
			if (user_id == 2) {
				otherUserMenu = (bool)e.DataIndex.GetValue ("User1_Menu_Change");
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
		} else if (e.Name == "Collaborate") {
			int id = (int)e.DataIndex.GetValue ("ID");
			// Prevent sync events sent out by this client
			if (id != user_id) {
				MenuManager.Collaboration.UpdateFromSync ((Collaborate)e.DataIndex.GetValue ("CollaborateObject"));
				//MenuManager.Collaboration = (Collaborate)e.DataIndex.GetValue ("CollaborateObject");


				// Update gameobject in the scene
				foreach (Transform child in collaboratePainting.transform) {
					Destroy (child.gameObject);
				}

				List<GameObject> strokes = (List<GameObject>)e.DataIndex.GetValue ("Strokes");
				foreach (var s in strokes) {
					s.transform.SetParent (collaboratePainting.transform, false);
				}
			}
		}
		// Test client share input 
		else if (e.Name == "TestInput") {
			String txt = e.DataIndex.GetValueAsString ("TestInputString");

			Text temp = GameObject.Find ("MinVRUnityClient/VRCameraPair/DrawingDebug").GetComponent<Text> ();
			temp.GetComponent<Text> ().text = txt;
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