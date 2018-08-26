/*
 * Created by Michael Lumley.
 * Get or assigns a color to a leaf
 */

using System.Collections.Generic;
using UnityEngine;

public class LeafColorer {

    private List<Color> presetColors = new List<Color>{Color.green, Color.red, Color.cyan, Color.yellow,
        Color.magenta, Color.blue,Color.gray, Color.white, Color.black };

    private Dictionary<string, Color> nameAndColors = new Dictionary<string, Color>();

 
    public Color GetColor(LeafData shape) {

        string leafName = shape.Name;
        bool colourAlreadyAssigned = nameAndColors.ContainsKey(leafName);

        // If colour not assigned and presets available
        if (!colourAlreadyAssigned && presetColors.Count > 0) {
            return this.SelectPresetColor(leafName);
        }
        // If colour not assigned and presets not available
        else if (!colourAlreadyAssigned && presetColors.Count == 0) {
            return this.GetRandomColor(leafName);
        }
        // Colour already assigned
        else {
            return nameAndColors[leafName];
        }
    }

    private Color SelectPresetColor(string leafName) {
        Color selectedColor = presetColors[0];

        nameAndColors.Add(leafName, selectedColor);
        presetColors.Remove(selectedColor);

        return selectedColor;
    }

    private Color GetRandomColor(string leafName) {
        while(true) {
            // Random a new color 
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            Color randomColor = new Color(r, g, b);

            // If the random color is not used, change the color then break the loop
            if (!nameAndColors.ContainsValue(randomColor)) {
                // Pair the leaf name and the new random color
                nameAndColors.Add(leafName, randomColor);
                return randomColor;
            }
        }
    }
}
