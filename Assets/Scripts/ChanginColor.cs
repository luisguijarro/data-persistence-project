using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChanginColor : MonoBehaviour
{
    private Color[] colors;
    private int indexOfColor;
    private float littleTime;
    [SerializeField] private float colorSpeed = 0.5f;

    private ColorBlock mySelectablecolors;
    // Start is called before the first frame update
    void Start()
    {
        this.indexOfColor = 0;
        this.littleTime = 0;
        this.colors = new Color[] {new Color(1f, 0.4f, 0.4f), new Color(0.4f, 1f, 0.4f), new Color(0.4f, 0.4f, 1f), new Color(1f, 1f, 0.4f) };
        this.mySelectablecolors = ColorBlock.defaultColorBlock;
        this.gameObject.GetComponent<Button>().colors = this.mySelectablecolors;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.littleTime >= 1f)
        {
            this.indexOfColor++;
            this.littleTime = 0f;
        }

        if (this.indexOfColor >= this.colors.Length)
        {
            this.indexOfColor = 0;
        }

        float fr = Mathf.SmoothStep(this.mySelectablecolors.highlightedColor.r, this.colors[this.indexOfColor].r, this.littleTime);
        float fg = Mathf.SmoothStep(this.mySelectablecolors.highlightedColor.g, this.colors[this.indexOfColor].g, this.littleTime);
        float fb = Mathf.SmoothStep(this.mySelectablecolors.highlightedColor.b, this.colors[this.indexOfColor].b, this.littleTime);
        this.mySelectablecolors.highlightedColor  = new Color(fr, fg, fb, 1f);//*= this.colors[this.indexOfColor]; // * Time.deltaTime;
        
        this.littleTime += Time.deltaTime * this.colorSpeed;
        
        this.gameObject.GetComponent<Button>().colors = this.mySelectablecolors;
    }                                                           
}
