/*
 * Create by Marko Ristic
 * Container class for a leaf shape, used to store leaves from leaf trait csv, and
 * used when directing simulation which leaves and ratios to use
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafShape {

    // Can get all instance variables
    public string Name { get; set; }
    public string LeafForm { get; set; }
    public float ThicknessMean { get; set; }
    public float ThicknessRange { get; set; }
    public float WidthMean { get; set; }
    public float WidthRange { get; set; }
    public float LengthMean { get; set; }
    public float LengthRange { get; set; }

    // Constructor just takes all variables
    public LeafShape(string name,
                    string leafForm,
                    float thicknessMean, 
                    float thicknessRange, 
                    float widthMean,
                    float widthRange, 
                    float lengthMean, 
                    float lengthRange) {
        this.Name = name;
        this.LeafForm = leafForm;
        this.ThicknessMean = thicknessMean;
        this.ThicknessRange = thicknessRange;
        this.WidthMean = widthMean;
        this.WidthRange = widthRange;
        this.LengthMean = lengthMean;
        this.LengthRange = lengthRange;
    }

    // Empty contructor that creates default leaf
    public LeafShape() {
        this.Name = "";
        this.LeafForm = "";
        this.ThicknessMean = 1;
        this.ThicknessRange = 0.5f;
        this.WidthMean = 1;
        this.WidthRange = 0.5f;
        this.LengthMean = 1;
        this.LengthRange = 0.5f;
    }
}
