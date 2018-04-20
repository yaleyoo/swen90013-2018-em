/** Created by Ze Wang 
 * Unit test for the leaf object
**/
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class LeafObjectTest {
	
	[Test]
	public void LeafSetAndGetName() {
		//Arrage
		GameObject gm = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Leaf"), new Vector3(0,10,0), Quaternion.Euler(0,0,0));
		string leafName="";
		//Act
		gm.GetComponent<Leaf>().SetName("LeafName");
		leafName = gm.GetComponent<Leaf> ().GetName ();
		//Assert
		Assert.AreEqual(leafName, "LeafName");
	}
	[Test]
	public void LeafSetAndGetSize() {
		GameObject gm = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Leaf"), new Vector3(0,10,0), Quaternion.Euler(0,0,0));
		gm.GetComponent<Leaf> ().SetSize (0, 10, 0);
		Vector3 v = new Vector3 (10, 0, 0);
		Assert.AreEqual (v, gm.GetComponent<Leaf> ().GetSize ());

	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator LeafObjectTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
