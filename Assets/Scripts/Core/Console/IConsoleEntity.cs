using System;
namespace Core
{
    public interface IConsoleEntity
    {
        void Invoke(string[] args);
        string[] Dump();
    }
}
