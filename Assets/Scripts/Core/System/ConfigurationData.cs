using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Core
{        
    public enum ConfigurationMode
    {
        Debug,
        Release
    }

    public class ConfigurationData : ScriptableObject
    {
        [SerializeField]
        public string title = "Ambition";
        [SerializeField]
        public int majorVersion = 1;
        [SerializeField]
        public int minorVersion = 0;    
        [SerializeField]
        public int buildNumber = 1;
        [SerializeField]
        public ConfigurationMode mode = ConfigurationMode.Debug;
        [SerializeField]
        public string branch = "master";

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
        public ConfigurationMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        public BuildVersionVO Version
        {
            get
            {
                var vers =  new BuildVersionVO( majorVersion, minorVersion, buildNumber );
                return vers;
            }
            set
            {        
                majorVersion = value.Major;
                minorVersion = value.Minor;
                buildNumber = value.Build;
            }
        }

        public string Branch
        {
            get
            {
                return branch;
            }
            set
            {
                branch = value;
            }
        }

        public override string ToString()
        {
            return string.Format( "{0} {1} ({2})", Version.ToString(), mode.ToString(), branch );
        }
    }
}