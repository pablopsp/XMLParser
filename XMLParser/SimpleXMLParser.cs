using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Reflection;
using System.Xml;

namespace XMLParser
{
    public static class SimpleXMLParser 
    {
        private static XmlDocument _xmlDoc = null;
        private static XmlNodeList _nodeList = null;
        private static List<ExpandoObject> _expandoList = null;
        private static IDictionary<string, object> _dynamicDictionary = null;
        private static dynamic _obj = null;

        /// <summary>
        /// Try to get the data from a XML passing a string with XML as "dataXML" 
        /// and a simple and specific node as "tag".
        /// Returns a list of objects.
        /// To get the data the requested object have to have the same property name as the childnode of the XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static List<T> GetXMLDataInStringByTag<T>(string dataXML, string tag) where T : class, new()
        {
            List<T> _finalList = null;
            try
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(dataXML);
                _nodeList = _xmlDoc.GetElementsByTagName(tag);

                _expandoList = new List<ExpandoObject>();
                foreach (XmlNode node in _nodeList)
                {
                    int numbOfProps = node.ChildNodes.Count;
                    _obj = new ExpandoObject();
                    _dynamicDictionary = (IDictionary<string, object>)_obj;

                    for (int i = 0; i < numbOfProps; i++)
                    {
                        _dynamicDictionary.Add(node.ChildNodes[i].Name, node.ChildNodes[i].InnerXml);
                    }

                    _expandoList.Add(_obj);
                }

                _finalList = new List<T>();
                foreach (ExpandoObject expObj in _expandoList)
                {
                    _finalList.Add(Convert<T>(expObj));
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return _finalList;
        }


        /// <summary>
        /// Try to get the data from a XML passing the path as "path" 
        /// and a simple and specific node as "tag".
        /// Returns a list of the object passed.
        /// To get the data the requested object have to have the same property name as the childnode of the XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static List<T> GetXMLDataInPathByTag<T>(string path, string tag) where T : class, new()
        {
            List<T> _finalList = null;
            try
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(path);
                _nodeList = _xmlDoc.GetElementsByTagName(tag);

                _expandoList = new List<ExpandoObject>();
                foreach (XmlNode node in _nodeList)
                {
                    int numbOfProps = node.ChildNodes.Count;
                    _obj = new ExpandoObject();
                    _dynamicDictionary = (IDictionary<string, object>)_obj;

                    for (int i = 0; i < numbOfProps; i++)
                    {
                        _dynamicDictionary.Add(node.ChildNodes[i].Name, node.ChildNodes[i].InnerXml);
                    }

                    _expandoList.Add(_obj);
                }

                _finalList = new List<T>();
                foreach (ExpandoObject expObj in _expandoList)
                {
                    _finalList.Add(Convert<T>(expObj));
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return _finalList;
        }



        /// <summary>
        /// Transform an Expando Object to the type of object that has been passed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        private static T Convert<T>(this IDictionary<string, object> src,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class, new()
        {
            Contract.Requires(src != null);
            T targetObject = new T();
            Type targetObjectType = typeof(T);

            foreach (PropertyInfo property in targetObjectType.GetProperties(bindingFlags))
            {
                if (src.ContainsKey(property.Name) && property.PropertyType == src[property.Name].GetType())
                {
                    property.SetValue(targetObject, src[property.Name]);
                }
            }

            return targetObject;
        }
    }
}
