using System;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IPersonManager : IDisposable
    {
        void Create(Person person, string teamName);
        void Create(Person person);
    }
}
