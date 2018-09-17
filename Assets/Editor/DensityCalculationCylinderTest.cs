using UnityEngine;
using NUnit.Framework;

public class DensityCalculationCylinderTest {

	[Test]
	public void DensityCalculationCylinderHighestLeafTest() {
        GameObject[] leaves = new GameObject[5];

        // Last leaf will be the highest
        for(int i = 0; i < 5; i++) {
            GameObject leaf = new GameObject();
            leaf.transform.position = new Vector3(0, i, 0);
            leaves[i] = leaf;
        }

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);
        GameObject highestLeaf = cylinder.GetHighestObject();

        Assert.AreEqual(highestLeaf, leaves[4]);
	}

    [Test]
    public void DensityCalculationCylinderCalcHeightDoesNotReturnNegativeTest() {
        GameObject[] leaves = new GameObject[5];
        GameObject leaf = new GameObject();

        leaf.transform.position = new Vector3(0, 0, 0);
        leaf.AddComponent<BoxCollider>();

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);

        float height = cylinder.CalcHeight(leaf);

        Assert.IsTrue(height >= 0f);
    }

    [Test]
    public void DensityCalculationCylinderRandomPointTest() {
        GameObject[] leaves = new GameObject[5];

        // Last leaf will be the highest
        for (int i = 0; i < 5; i++) {
            GameObject leaf = new GameObject();
            leaf.transform.position = new Vector3(0, i, 0);
            leaf.AddComponent<BoxCollider>();
            leaves[i] = leaf;
        }

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);

        Vector3 point = cylinder.RandomPointInCylinder();

        Assert.IsTrue(point.x >= -100 && point.x <= 100);
        Assert.IsTrue(point.z >= -100 && point.z <= 100);
    }

    [Test]
    public void DensityCalculationCylinderPointInObjectsTest() {
        GameObject[] leaves = new GameObject[5];

        // Last leaf will be the highest
        for (int i = 0; i < 5; i++) {
            GameObject leaf = new GameObject();
            leaf.transform.position = new Vector3(0, i, 0);
            leaf.AddComponent<BoxCollider>();
            leaves[i] = leaf;
        }

        DensityCalculationCylinder cylinder = new DensityCalculationCylinder(leaves, 100, 100);

        // Test if point is in leaves
        Vector3 point = new Vector3(0, 1, 0);
        bool test = cylinder.IsPointInObjects(point);
        Assert.IsTrue(test);

        // Tets if point is not in leaves
        point = new Vector3(0, 6, 0);
        test = cylinder.IsPointInObjects(point);
        Assert.IsFalse(test);
    }
}
