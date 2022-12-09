using Kabuki_Desktop.Models;
using Kabuki_Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kabuki_Desktop.EventArgsClasses
{
    public class ChangeUCEventArgs : EventArgs
    {
        public User User { get; }
        public string UserControlName { get; }
        public string NewUCName { get; }
        public BaseVM NewUCObject { get; }

        public ChangeUCEventArgs(string userControlName, User user = null, string newUCName = null, BaseVM newUCObject=null)
        {
            User = user;
            UserControlName = userControlName;
            NewUCName = newUCName;
            NewUCObject = newUCObject;
        }
    }
}
