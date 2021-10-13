using System.Collections.Generic;
namespace Ambition
{
    public struct ErrorEventArgs
    {
        public ErrorType Type;
        public Dictionary<string, string> Substitutions;
        public string[] Data;

        public ErrorEventArgs(ErrorType type, params string[] data) : this(type, null, data) {}
        public ErrorEventArgs(ErrorType type, Dictionary<string, string> substitutions, params string[] data)
        {
            Type = type;
            Substitutions = substitutions;
            Data = data;
        }

        public enum ErrorType
        {
            save_file
        }
    }
}
