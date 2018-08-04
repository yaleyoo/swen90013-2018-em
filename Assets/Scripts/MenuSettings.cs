/*
 * Created by Yuanyu Guo.
 * Class to store static variables.
 */

public class MenuSettings {
    private static bool isVisualize;// Get visualization value from user input
    //private static int leafLimitation;

	private static float volumeRatio;// Density of the simulation

    // Public method to set isVisualize
    public static void SetIsVisualize(bool isVisualize)
    {
        MenuSettings.isVisualize = isVisualize;
    }

    // Public method to get isVisualize
    public static bool GetIsVisualize()
    {
        return MenuSettings.isVisualize;
    }

	public static void SetVolumeRatio(float volumeRatio)
	{
		MenuSettings.volumeRatio = volumeRatio;
	}

	public static float GetVolumeRatio()
	{
		return MenuSettings.volumeRatio;
	}

}
