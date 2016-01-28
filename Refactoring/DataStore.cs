using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class DataStore
    {
        public UserStore Users { get; set; }
        public List<Product> Products { get; set; }

        public DataStore(List<User> users, List<Product> products)
        {
            this.Users = new UserStore(users);
            this.Products = products;
        }

        public void updateUser(TuscUser loggedInUser)
        {
            this.Users.updateUserBalance(loggedInUser);
        }

        public void writeToDisk()
        {
            writeObjectToFile(this.Users.UserList, @"Data\Users.json");
            writeObjectToFile(this.Products, @"Data\Products.json");    
        }

        private void writeObjectToFile(object objectToWrite, string fileName)
        {
            if (objectToWrite != null)
            {
                string json2 = JsonConvert.SerializeObject(objectToWrite, Formatting.Indented);
                File.WriteAllText(fileName, json2);
            }
        }
    }
}
