using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NyAppHelper;
using NyAppHelper.Model;
using NyAppHelper.Data;
using System.Collections.ObjectModel;

namespace NyAppWP.DataContext
{
    public class UserDataContext : ObservableObjectBase
    {
        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get
            {
                if(_users==null)
                {
                    _users = new ObservableCollection<User>();
                }
                return _users;
            }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    this.NotifyPropertyChanged("Users");
                }
            }
        }
    }
}
