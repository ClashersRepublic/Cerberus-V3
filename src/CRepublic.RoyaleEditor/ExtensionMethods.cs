﻿using System.Collections.Generic;
using System.Windows.Forms;
using CR.Assets.Editor.ScOld;

namespace CR.Assets.Editor
{
    internal static class ExtensionMethods
    {
        public static void Populate(this TreeView tv, List<ScObject> scd)
        {
            foreach (var data in scd)
            {
                var dataTypeKey = data.GetDataType().ToString();
                var dataTypeName = data.GetDataTypeName();
                var id = data.Id.ToString();
                if (!tv.Nodes.ContainsKey(dataTypeKey))
                {
                    tv.Nodes.Add(dataTypeKey, dataTypeName);
                }
                tv.Nodes[dataTypeKey].Nodes.Add(id, data.GetName());
                tv.Nodes[dataTypeKey].Nodes[id].Tag = data;
                tv.Nodes[dataTypeKey].Nodes[id].PopulateChildren(data);
            }
        }

        public static void PopulateChildren(this TreeNode tn, ScObject sco)
        {
            foreach (var child in sco.Children)
            {
                tn.Nodes.Add(child.Id.ToString(), child.GetName());
                tn.Nodes[child.Id.ToString()].Tag = child;
                PopulateChildren(tn.Nodes[child.Id.ToString()], child);
            }
        }
    }
}
