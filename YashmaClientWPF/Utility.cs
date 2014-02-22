using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Data;
using System.Xml;

namespace YashmaClientWPF
{
    class Utility
    {
        public static List<TreeItem> tree_items = new List<TreeItem>();

        public static List<TreeItem> SelectItems(string path)
        {
            List<TreeItem> new_tree_items = new List<TreeItem>();
            foreach (TreeItem item in tree_items)
            {
                if (item.Tag.IndexOf(path) > -1)
                {
                    new_tree_items.Add(item);
                }
            }
            return new_tree_items;
        }

        public static void LoadItems()
        {
            XmlTextReader reader = new XmlTextReader(Environment.CurrentDirectory + "\\data\\tree_menu.xml");
            reader.WhitespaceHandling = WhitespaceHandling.None;

            while (reader.Read())
            {
                if (reader.GetAttribute("Weight") != null)
                {
                    TreeItem treeItem = new TreeItem();
                    treeItem.Name = reader.GetAttribute("Name");
                    treeItem.Tag = reader.GetAttribute("Tag");
                    treeItem.Weight = reader.GetAttribute("Weight");
                    treeItem.Size = reader.GetAttribute("Size");
                    treeItem.Sample = reader.GetAttribute("Сontent");
                    treeItem.Image = reader.GetAttribute("Image");
                    treeItem.Description = reader.GetAttribute("Description");

                    tree_items.Add(treeItem);
                }
            }
            reader.Close();
        }
    }

    class TreeItem
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }
        public string Sample { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}