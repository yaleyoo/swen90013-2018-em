/*
 * Created by Marko Ristic.
 * Class to store static variables between simulation and output scenes.
 */


public class Results {
    // Single calculated density from the simulation
    private static float density;
    
    // Get the density of the simulation
    public static float GetDensity()
    {
        return density;
    }

    // Set the density of the simulation
    public static void SetDensity(float density)
    {
        Results.density = density;
    }
    
}
