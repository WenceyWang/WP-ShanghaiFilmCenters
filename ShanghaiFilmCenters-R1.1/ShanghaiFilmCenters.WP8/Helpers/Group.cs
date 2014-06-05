using System;
using System.Collections.Generic;
using System.Linq;

namespace ShanghaiFilmCenters.WP8.Helpers
{
    public class Group<T> : List<T>
    {
        public Group(string name, IEnumerable<T> items)
            : base(items)
        {
            this.Title = name;
        }

        public string Title
        {
            get;
            set;
        }

        public static List<Group<T>> GetItemGroups<T>(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
        {
            IEnumerable<Group<T>> groupList = from item in itemList
                                              group item by getKeyFunc(item) into g
                                              orderby g.Key
                                              select new Group<T>(g.Key, g);

            return groupList.ToList();
        }
    }
}