/*
 * Created by Yudong Gao.
 * Unit tests for DensityCalculator
 */

using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DensityCalculatorTest{

	private float dropAreaX = 50;
	private float dropAreaY = 50;
	private float height = 100;
	private float densityIgnoreBorder = 10;
	private int sliceSampleSize = 1000;
	private int numSlices = 10;
	DensityCalculationCylinder calcArea;


	[OneTimeSetUp]
	public void Init(){
		
		GameObject[] leaves = new GameObject[5];

		// Last leaf will be the highest
		for (int i = 0; i < 5; i++) {
			GameObject leaf = new GameObject();
			leaf.transform.position = new Vector3(0, i * 10, 0);
			leaf.AddComponent<Leaf>();

			GameObject leftCollider = new GameObject();
			leftCollider.name = "leftCollider";
			leftCollider.AddComponent<BoxCollider>();
			leftCollider.transform.parent = leaf.transform;
			leftCollider.transform.localPosition = new Vector3(0, 0, -0.5f);

			GameObject rightCollider = new GameObject();
			rightCollider.name = "rightCollider";
			rightCollider.AddComponent<BoxCollider>();
			rightCollider.transform.parent = leaf.transform;
			rightCollider.transform.localPosition = new Vector3(0, 0, 0.5f);

			leaves[i] = leaf;
		}

		// set up the calculation area
		calcArea = new DensityCalculationCylinder(leaves, (this.dropAreaX - this.densityIgnoreBorder),
			(this.dropAreaY - this.densityIgnoreBorder) );
	}

	/// <summary>
	/// No points will be generated,
	/// the return value shoule be 0.
	/// </summary>
	[Test]
	public void NoSliceSampleSize() {
		DensityCalculator dc = new DensityCalculator ();
		float valueReturn = dc.CalculateDensity (this.calcArea, 0, this.numSlices);

		Assert.IsTrue (0==valueReturn);
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator DensityCalculatWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
