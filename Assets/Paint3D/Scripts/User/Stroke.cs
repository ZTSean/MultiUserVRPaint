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
			if (mBrush != value) {
				mBrush.Dispose ();
			}
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
		// TODO: try catch "new vertex is empty exception"
		vertices.Add (v);
	}

	// Initialize the vertices
	void Start ()
	{
		// TODO: try catch "could not create exception"
		vertices = new List<Vertex> ();
	}


	void Update ()
	{
	}


}
