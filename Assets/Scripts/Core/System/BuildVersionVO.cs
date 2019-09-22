using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildVersionVO
{
    public int Major;
    public int Minor;
    public int Build;

    public BuildVersionVO(int major, int minor, int build)
    {
        Major = major;
        Minor = minor;
        Build = build;
    }

    public override string ToString()
    {
        return string.Format("{0}.{1:00}({2})", Major, Minor, Build);
    }
};