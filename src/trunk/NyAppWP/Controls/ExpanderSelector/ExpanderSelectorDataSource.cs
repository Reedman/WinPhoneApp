using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection;
using NyAppHelper.Model;

namespace NyAppWP.Controls.ExpanderSelector
{

    public class ExpanderSelectorDataSource
    {

        public ExpanderSelectorDataSource() { }

        #region 私有变量

        private string _Id;
        private string _name;
        private string _parentId;

        #endregion

        #region 公有变量

        public string ID
        {
            set
            {
                if (_Id != value)
                    _Id = value;
            }
            get
            {
                return _Id;
            }
        }

        public string Name
        {
            set
            {
                if (_name != value)
                    _name = value;
            }
            get
            {
                return _name;
            }
        }

        public string ParentID
        {
            set
            {
                if (_parentId != value)
                    _parentId = value;
            }
            get
            {
                return _parentId;
            }
        }

        #endregion
    }

    public class ExpanderSelectorDataContext
    {
        public Object SelectedItem;

        private List<ExpanderSelectorDataSource> _source = new List<ExpanderSelectorDataSource>();

        public List<ExpanderSelectorDataSource> Source
        {
            get
            {
                return _source;
            }
            set
            {
                if (_source != value)
                {
                    _source = value;
                }
            }
        }

        public ExpanderSelectorDataContext(List<object> list)
        {
            if (list[0] is Crop)
            {
                foreach (var item in list)
                {
                    var temp = (Crop)item;
                    _source.Add(new ExpanderSelectorDataSource()
                    {
                        ID = temp.CropID.ToString(),
                        Name = temp.CropName,
                        ParentID = temp.ParentID.HasValue ? temp.ParentID.ToString() : null,
                    });
                }
            }
            else if (list[0] is Region)
            {
                foreach (var item in list)
                {
                    var temp = (Region)item;
                    _source.Add(new ExpanderSelectorDataSource()
                    {
                        ID = temp.RegionID.ToString(),
                        Name = temp.RegionName,
                        ParentID = temp.ParentID.HasValue ? temp.ParentID.ToString() : null,
                    });
                }
            }
            else if (list[0] is Pest)
            {
                foreach (var item in list)
                {
                    var temp = (Pest)item;
                    _source.Add(new ExpanderSelectorDataSource()
                    {
                        ID = temp.PestID.ToString(),
                        Name = temp.PestName,
                        ParentID = temp.ParentID.HasValue ? temp.ParentID.ToString() : null,
                    });
                }
            }
        }


        public List<ExpanderSelectorDataSource> GetRoots()
        {
            return _source.Where(s => s.ParentID == null).ToList();
        }

        public List<ExpanderSelectorDataSource> GetChildNodeById(String Id)
        {
            return _source.Where(s => s.ParentID == Id).ToList();
        }


        public bool IsLeaf(String Id)
        {
            var count = GetChildNodeById(Id).Count;
            if (count == 0)
                return true;
            else
                return false;
        }

    }
}
