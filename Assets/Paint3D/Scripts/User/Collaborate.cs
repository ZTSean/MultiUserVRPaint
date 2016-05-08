using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CollaborateVertexInfo
{
	public int user_id;
	public int strokeIndex;
	public int vertexIndex;
	public Vertex v;

	public CollaborateVertexInfo (int id, int si, int vi, Vertex vv)
	{
		user_id = id;
		strokeIndex = si;
		vertexIndex = vi;
		v = vv;
	}
}

public class Collaborate
{
	// -------------------------- user 1
	/// <summary>
	/// Determine whether start collaborate work.
	/// </summary>
	private bool isUser1Start = false;

	public bool IsUser1Start {
		get {
			return isUser1Start;
		}
		set {
			isUser1Start = value;
		}
	}

	/// <summary>
	/// Determine whether it is done with the stroke.
	/// </summary>
	private bool isUser1DrawStart = false;

	public bool IsUser1DrawStart {
		get {
			return isUser1DrawStart;
		}
		set {
			isUser1DrawStart = value;
		}
	}

	// -------------------------- user 2
	/// <summary>
	/// Determine whether start collaborate work.
	/// </summary>
	private bool isUser2Start = false;

	public bool IsUser2Start {
		get {
			return isUser2Start;
		}
		set {
			isUser2Start = value;
		}
	}

	/// <summary>
	/// Determine whether it is done with the stroke.
	/// </summary>
	private bool isUser2DrawStart = false;

	public bool IsUser2DrawStart {
		get {
			return isUser2DrawStart;
		}
		set {
			isUser2DrawStart = value;
		}
	}

	private int user1VertexIndex = -1;

	public int User1VertexIndex {
		get {
			return user1VertexIndex;
		}
		set {
			user1VertexIndex = value;
		}
	}

	/// <summary>
	/// The index of the user1 current stroke.
	/// </summary>
	private int user1StrokeIndex = -1;

	public int User1StrokeIndex {
		get {
			return user1StrokeIndex;
		}
		set {
			user1StrokeIndex = value;
		}
	}

	/// <summary>
	/// The index of the user2 current vertex.
	/// </summary>
	private int user2VertexIndex = -1;

	public int User2VertexIndex {
		get {
			return user2VertexIndex;
		}
		set {
			user2VertexIndex = value;
		}
	}

	private int user2StrokeIndex = -1;

	public int User2StrokeIndex {
		get {
			return user2StrokeIndex;
		}
		set {
			user2StrokeIndex = value;
		}
	}

	/// <summary>
	/// The vertices of strokes.
	/// </summary>
	private List<CollaborateVertexInfo> strokesInfo;

	public List<CollaborateVertexInfo> StrokesInfo {
		get {
			if (strokesInfo == null) {
				strokesInfo = new List<CollaborateVertexInfo> ();
			}

			return strokesInfo;
		}

		set {
			strokesInfo = value;
		}
	}

	private List<GameObject> user1NewStrokes;

	public List<GameObject> User1NewStrokes {
		get {
			if (user1NewStrokes == null) {
				user1NewStrokes = new List<GameObject> ();
			}

			return user1NewStrokes;
		}

		set {
			user1NewStrokes = value;
		}
	}

	private List<GameObject> user2NewStrokes;

	public List<GameObject> User2NewStrokes {
		get {
			if (user2NewStrokes == null) {
				user2NewStrokes = new List<GameObject> ();
			}

			return user2NewStrokes;
		}

		set {
			user2NewStrokes = value;
		}
	}

	public Collaborate ()
	{
		IsUser1Start = false;
		IsUser2Start = false;
		IsUser1DrawStart = false;
		IsUser2DrawStart = false;

		User1StrokeIndex = -1;
		User1VertexIndex = -1;
		User2StrokeIndex = -1;
		User2VertexIndex = -1;

		User1NewStrokes = new List<GameObject> ();
		User2NewStrokes = new List<GameObject> ();
		StrokesInfo = new List<CollaborateVertexInfo> ();
	}

	public void StartNewStroke (GameObject brushObj, GameObject paintingObj, Painting pc, int user_id)
	{
		// Add new stroke to painting
		pc.startNewStroke (paintingObj, brushObj);
		// Add initial vertex
		Vertex v = new Vertex (
			           brushObj.transform.position,
			           brushObj.transform.rotation,
			           1,
			           new Vector3 (0, 0, 0)
		           );
		pc.AddVertex (v);
		if (user_id == 1) {
			User1StrokeIndex++;
			User1VertexIndex = 0;
			AddVertex (user_id, User1StrokeIndex, User1VertexIndex, v);
		} else if (user_id == 2) {
			User2StrokeIndex++;
			User2VertexIndex = 0;
			AddVertex (user_id, User2StrokeIndex, user2VertexIndex, v);
		}


	}

	public void AddVertex (int id, int si, int vi, Vertex v)
	{
		StrokesInfo.Add (new CollaborateVertexInfo (id, si, vi, v));
	}

	/*
	public Vertex GetVertex (int id, int si, int vi)
	{
		Vertex v = null;
		foreach (var item in StrokesInfo) {
			if (item.user_id == id && si == item.strokeIndex && vi == item.vertexIndex) {
				v = item.v;
				break;
			}
		}

		return v;
	}*/

	public void Restart ()
	{
		User1StrokeIndex = -1;
		User2StrokeIndex = -1;
		User1VertexIndex = -1;
		User2VertexIndex = -1;

		strokesInfo.Clear ();
	}

	// Update strokes' vertices position information from events
	public void UpdatePainting ()
	{


	}

	public void UpdateFromSync (Collaborate c)
	{
		IsUser1DrawStart = c.IsUser1DrawStart;
		IsUser2DrawStart = c.IsUser2DrawStart;
		IsUser1Start = c.IsUser1Start;
		IsUser2Start = c.IsUser2Start;


		/*
		if (StrokesInfo.Count < c.StrokesInfo.Count)
		{
			int diff = c.StrokesInfo.Count - StrokesInfo.Count;
			for (int i = c.StrokesInfo.Count; i < diff; i++) {
				int k = User1StrokeIndex < User2StrokeIndex ? User1StrokeIndex : User2StrokeIndex;
				if (c.StrokesInfo[i].strokeIndex > k)
				{
					GameObject cp = GameObject.Find ("Paint3DInterface/CollaboratePainting");
					GameObject tmpBrush = 
				}
			}
		}*/

		StrokesInfo.Clear ();
		foreach (var item in c.strokesInfo) {
			StrokesInfo.Add (item);
		}

		User1StrokeIndex = c.User1StrokeIndex;
		User1VertexIndex = c.User1VertexIndex;
		User2StrokeIndex = c.User2StrokeIndex;
		User2VertexIndex = c.User2VertexIndex;

		// Add new gameobject to collaboratePainting
		//GameObject cp = GameObject.Find ("Paint3DInterface/CollaboratePainting");

		debugConsoleLog ();
	}

	// TODO: add more attributes that will be needed for future advanced implemention of collaboration functionality

	public void debugConsoleLog ()
	{
		string tmp = "us1: " + IsUser1Start.ToString () + " " + IsUser1Start.ToString ();
		tmp += " us2: " + IsUser2Start.ToString () + " " + IsUser2Start.ToString ();

		Debug.Log (tmp);
	}

}
