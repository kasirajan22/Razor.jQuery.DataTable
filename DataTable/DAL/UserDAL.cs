using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataTable.Interface;
using DataTable.Model;
using Newtonsoft.Json.Linq;

namespace DataTable.DAL
{
    public class UserDAL : IUser
    {
        public string GetUserData()
        {
           var webClient = new WebClient();            
            string data = webClient.DownloadString("wwwroot/data.json");            
            return data;                        
        }
    }
}
