/**
 * Created by Chao Li
 * Unit test for plane, check if leaf will be destroyed when colliding with the plane
 * Before running this test, move plane object to Resources folder to make it a prefab.
 * PlayMode test should also be enabled to run this test.
**/
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class LeafDestroyTest {

    [UnityTest]
    public IEnumerator LeafDestroy() {
        var leaf = GameObject.Instantiate(Resources.Load("Leaf"), new Vector3(30, 30, 120), new Quaternion());

        yield return new WaitForSecondsRealtime(5);
        Assert.IsTrue(leaf == null);
    }
}
