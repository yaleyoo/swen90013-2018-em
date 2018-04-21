using UnityEngine;
using System.Collections;

public static class LeafLimit {
    private static bool stopAtLeafLimit = true;
    private static int leafNumberLimit = 1000;

    // Set the number of maximum leaves for the simulation to stop at
    public static void SetLeafNumberLimit(int numberLimit)
    {
        leafNumberLimit = numberLimit;
        stopAtLeafLimit = true;
    }


    // Remove the maximum number of leaves limit, and let the simulation run until call to stop method
    public static void RemoveLeafNumberLimit()
    {
        stopAtLeafLimit = false;
    }

    public static bool IfHasLimit() {
        return stopAtLeafLimit;
    }

    public static int GetLimit() {
        return leafNumberLimit;
    }
}
