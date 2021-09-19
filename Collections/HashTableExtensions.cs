using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BorrehSoft.Utilities.Collections
{
    public static class HashtableExtensions
    {
        public static bool TryGetString(this Hashtable ht, string path, out string result)
        {
            var route = path.Split('/');
            object current = ht;

            result = "";

            foreach (var segment in route)
            {
                if (current is Hashtable)
                    current = ((Hashtable)current)[segment];
                else
                    return false;
            }

            result = current as string;
            return current is string;
        }
        public static bool TryGetLong(this Hashtable ht, string path, out long result)
        {
            var route = path.Split('/');
            object current = ht;

            result = -1;

            foreach (var segment in route)
            {
                if (current is Hashtable)
                    current = ((Hashtable)current)[segment];
                else
                    return false;
            }

            result = (long)current;
            return result > -1;
        }
        public static bool TryGetArrayList(this Hashtable ht, string path, out ArrayList result)
        {
            var route = path.Split('/');
            object current = ht;

            result = null;

            foreach (var segment in route)
            {
                if (current is Hashtable)
                    current = ((Hashtable)current)[segment];
                else
                    return false;
            }

            result = current as ArrayList;
            return current is ArrayList;
        }

        public static bool TryGetHashtable(this ArrayList al, int index, out Hashtable result)
        {
            if (index < al.Count)
                result = al[index] as Hashtable;
            else
                result = null;
            return result is Hashtable;
        }

        /// <summary>
        /// this stinky code is only used in tests. maybe lets move this extension to
        /// the tests lib. if you want to use this outside of tests, make it less stinky.
        /// </summary>
        /// <param name="thisHashtable">stinky hashtable</param>
        /// <param name="k">stinky path to traverse</param>
        /// <param name="v">stinky value</param>
        /// <returns>stank</returns>
        public static Hashtable Set(this Hashtable thisHashtable, string k, object v)
        {
            Hashtable workingTable = thisHashtable;
            var route = new Queue<string>(k.Split('/'));

            while (route.Count > 1)
            {
                var dq = route.Dequeue().Split('#');
                var nextKey = dq[0];
                if (workingTable.ContainsKey(nextKey))
                {
                    if (workingTable[nextKey] is Hashtable)
                    {
                        workingTable = (Hashtable)workingTable[nextKey];
                    }
                    else if ((workingTable[nextKey] is ArrayList) && (dq.Length == 2))
                    {
                        var l = (ArrayList)workingTable[nextKey];
                        var ix = int.Parse(dq[1]);

                        if (l.Count <= ix)
                        {
                            workingTable = new Hashtable();
                            l.Insert(ix, workingTable);
                        }
                        else
                        {
                            workingTable = l[ix] as Hashtable;
                        }
                    }
                    else
                    {
                        workingTable.Remove(nextKey);
                        var newHashtable = new Hashtable();
                        workingTable.Add(nextKey, newHashtable);
                        workingTable = newHashtable;
                    }
                }
                else if (dq.Length == 2)
                {
                    var newList = new ArrayList();
                    var newHashtable = new Hashtable();
                    newList.Add(newHashtable);
                    workingTable.Add(nextKey, newList);
                    workingTable = newHashtable;
                }
                else
                {
                    var newHashtable = new Hashtable();
                    workingTable.Add(nextKey, newHashtable);
                    workingTable = newHashtable;
                }
            }

            k = route.Dequeue();
            if (workingTable.ContainsKey(k)) workingTable.Remove(k);
            workingTable.Add(k, v);
            return thisHashtable;
        }
    }
}