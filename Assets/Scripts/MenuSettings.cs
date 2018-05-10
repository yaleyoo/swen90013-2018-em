/*
 * Created by Yuanyu Guo.
 * Class to store static variables.
 */

public class MenuSettings {
    private static bool isVisualize;// Get visualization value from user input
    //private static int leafLimitation;

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

}
