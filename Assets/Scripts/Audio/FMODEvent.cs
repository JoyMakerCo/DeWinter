using System;
namespace Ambition
{
    [Serializable]
    public struct FMODEventParameterConfig
    {
        public string Name;
        public int Value;
    }

    //The FMOD Music Event for this moment
    [Serializable]
    public struct FMODEvent
    {
        [FMODUnity.EventRef]
        public string Name;
        FMOD.Studio.EventInstance EventInstance;
        //These are the parameters for the FMOD Music Event (Not sure how to display them in IncidentConfig yet)
        public FMODEventParameterConfig[] Parameters;

        public int GetParameterIndexByName(string findString){
            for(int i = 0; i < Parameters.Length; i++)
            {
                if(Parameters[i].Name == findString)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
