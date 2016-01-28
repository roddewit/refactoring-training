using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class UserStore
    {
        public List<User> UserList { get; set; }

        public UserStore(List<User> userList)
        {
            this.UserList = userList;
        }

        public void updateUserBalance(TuscUser loggedInUser)
        {
            if (loggedInUser != null)
            {
                foreach (var user in this.UserList)
                {
                    if (user.Name == loggedInUser.Name && user.Pwd == loggedInUser.Password)
                    {
                        user.Bal = loggedInUser.Balance;
                    }
                }
            }
        }

        public TuscUser getValidUser(string userName, string password)
        {
            TuscUser tuscUser = null;
            foreach (var user in this.UserList)
            {                
                if (user.Name == userName && user.Pwd == password)
                {
                    tuscUser = new TuscUser();
                    tuscUser.Name = userName;
                    tuscUser.Password = password;
                    tuscUser.Balance = user.Bal;
                }
            }
            return tuscUser;
        }
    }
}
