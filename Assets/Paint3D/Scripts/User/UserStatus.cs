using UnityEngine;
using MinVR;
using System.Collections;

// NOT IN USE FOR NOW
using System.Collections.Generic;


public class UserStatus : MonoBehaviour
{
	// Store all user information
	private VRDataIndex table;

	public VRDataIndex Table {
		get {
			if (table == null) {
				table = new VRDataIndex ();
			}

			return table;
		}
	}

	public BrushType CurrentBrush { get; set; }

	public Dictionary<string, object> Options { get; set; }

	public bool isMenuOpen;

	public int user_id;

}
