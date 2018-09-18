﻿using UnityEngine;
using NUnit.Framework;

public class DensityCalculationCylinderTest {

	[Test]
	public void DensityCalculationCylinderCylinderHeightTest() {
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

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);
        float height = cylinder.ComputeCylinderHeightToUse();

        Assert.IsTrue(height >= 50 && height <= 52);
    }

    [Test]
    public void DensityCalculationCylinderRandomPointTest() {
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

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);

        Vector3 point = cylinder.RandomPointInCylinderSlice(5, 2);

        Assert.IsTrue(point.x >= -100 && point.x <= 100);
        Assert.IsTrue(point.z >= -100 && point.z <= 100);
        Assert.IsTrue(point.y >= 20 && point.y <= 30);
    }

    [Test]
    public void DensityCalculationCylinderPointInObjectsTest() {
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

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);

        // Test if point is in leaves
        Vector3 point = new Vector3(0, 20, 1);
        bool test = cylinder.IsPointInObjects(point);
        Assert.IsTrue(test);

        // Test if point is not in leaves
        point = new Vector3(10, 10, 0);
        test = cylinder.IsPointInObjects(point);
        Assert.IsFalse(test);
    }
}
