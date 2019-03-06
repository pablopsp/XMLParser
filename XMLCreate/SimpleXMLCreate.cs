using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XMLCreate
{
    public class SimpleXMLCreate
    {
        /// <summary>
        /// Create a simple XML. Tuple is a Node and his Childs, 
        /// and the dict inside the tuple is the Childs key and value
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodesANDValues"></param>
        /// <param name="path"></param>
        public static void CreateSimpleXML(string root, List<Tuple<string, IDictionary<string, string>>> nodesANDValues, string path)
        {
            XElement xRoot = new XElement(root);

            foreach (Tuple<string, IDictionary<string, string>> tuple in nodesANDValues)
            {
                XElement apoyo = new XElement(tuple.Item1);
                foreach (KeyValuePair<string, string> entry in tuple.Item2)
                {
                    apoyo.Add(new XElement(entry.Key, entry.Value));
                }
                xRoot.Add(apoyo);
            }
            new XDocument(xRoot).Save(path);
        }

        /// <summary>
        /// Create a plane xml on selected path
        /// </summary>
        /// <param name="nodesAndValues"></param>
        /// <param name="filePathWithName"></param>
        public static void FlatXML(IDictionary<string, string> nodesAndValues, string filePathWithName)
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("root"));
  
            foreach (KeyValuePair<string, string> pair in nodesAndValues)
            {
                doc.Root.Add(new XElement(pair.Key, pair.Value));
            }
            doc.Save(filePathWithName);
        }
    }
}