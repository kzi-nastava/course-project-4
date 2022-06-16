using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;

namespace Hospital.Users.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        List<DoctorUser> DoctorUsers { get; set; }
    }
}
