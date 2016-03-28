using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Vertex
{
	// position of the vertex
	public Vector4 position;
	// orientation of the vertex
	public Quaternion orientation;
	// the pressure of brush
	public float pressure;
	// must be multiple of 128-bits to use shader buffers efficiently
	public Vector3 padding;

	public Vertex (Vector4 p, Quaternion ori, float pre, Vector3 pad)
	{
		this.position = p;
		this.orientation = ori;
		this.pressure = pre;
		this.padding = pad;
	}
}

public class Stroke : MonoBehaviour
{

	public IBrush mBrush;
	//

	public IBrush Brush {
		get {
			return mBrush;
		}
		set {
			mBrush = value;
			mBrush.Refresh ();
		}
	}

	/// <summary>
	/// The vertices of the stroke.
	/// </summary>
	public List<Vertex> vertices;

	/// <summary>
	/// Adds the vertex to vertices.
	/// </summary>
	/// <param name="v">V.</param>
	public void AddVertex (Vertex v)
	{
		// TODO: try catch "new vertex is empty exception and v already exist"

		if (!vertices.Contains (v)) {
			Debug.Log ("stroke add vertex");
			vertices.Add (v);
			Debug.Log (vertices.Count);
			Brush.AddVertex (v);
		}

	}

	// Initialize the vertices
	public void Initialize ()
	{
		// TODO: try catch "could not create exception"
		vertices = new List<Vertex> ();
	}



}
