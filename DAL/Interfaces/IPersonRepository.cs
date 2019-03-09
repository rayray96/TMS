using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IPersonRepository
    {
        /// <summary>
        /// Creating team and adding a manager to it.     
        /// </summary>
        /// <param name="manager">Manager of a team</param>
        /// <param name="teamName">The name of a team</param>
        void CreateTeam(Person manager, string teamName);
    }
}
