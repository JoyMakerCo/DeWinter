using UnityEngine;
using System.Collections;

public class Disposition {

    public string name;
    public string like;
    public string dislike;
    public Color color;

	public Disposition(string n, Color c, string l, string d)
    {
        name = n;
        color = c;
        like = l;
        dislike = d;
    }
}
