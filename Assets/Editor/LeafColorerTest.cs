/*
 * Created by Yudong Gao.
 * Unit Test for LeafColorer class.
 * 
 */



using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class LeafColorerTest {

	LeafColorer lc = new LeafColorer();

	[Test]
	public void ColorIsAssigned(){
		List<Color> presetColor = lc.getPresetColors ();

		Color current_color = lc.GetColor (new LeafData());

		Assert.IsTrue (current_color.Equals (Color.green));
	}

	[Test]
	public void ColorRemoved(){
		
		List<Color> presetColor = lc.getPresetColors ();

		Color removed_color = lc.GetColor (new LeafData());

		Assert.IsFalse (presetColor.Contains (removed_color));
	}

	[Test]
	public void presetColorEmpty(){
		List<Color> presetColor = lc.getPresetColors ();

		// run out of the colors in list
		for (int i = 0; i < 9; i++){
			Color removed_color = lc.GetColor (new LeafData("ALeaf", "Flat", i, i, i, i, i, i));
		}

		Assert.IsTrue (presetColor.Count == 0);

	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator LeafColorerTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
