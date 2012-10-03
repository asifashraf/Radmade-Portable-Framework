using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


    public static class ExXNode
    {
		#region Methods (24) 

		// Public Methods (24) 

        public static void AddAttribute(this XNode node, IEnumerable<XAttribute> attributes)
        {
            foreach (XAttribute a in attributes)
            {
                ((XElement)node).Add(a);
            }
        }

        public static void AddAttribute(this XNode node, XAttribute attribute)
        {            
            ((XElement)node).Add(attribute);            
        }

        public static void ChangeAttribute(this XNode node, string attributeName, string newValue)
        {
            try
            {
                IEnumerable<XAttribute> attribs = node.GetAttributes();
                attribs = attribs.Change(attributeName, newValue);
                ((XElement)node).RemoveAttributes();
                foreach (XAttribute a in attribs)
                    ((XElement)node).Add(a);
            }
            catch { }
        }

        public static XNode Clone(this XNode node)
        {
            XElement e = new XElement(node.Name(), node.GetAttributes().ToArray<XAttribute>(),
                node.Nodes());
            return e;
        }

        public static XNode FindFirst(this XNode node, string elementName)
        {
            XNode n = null;
            if (node.IsNamed(elementName))
                return node;

            XNode nextOne = node.FirstChild();
            if (nextOne != null)
                n = nextOne.FindFirst(elementName);

            if (null != n)
                return n;

            if (node.NextNode != null)
                n = node.NextNode.FindFirst(elementName);

            return n;
        }

        public static XNode FindUnique(this XNode node, 
            string elementName, 
            string attrib, 
            string value)
        {
            XNode n = null;
            if (node.IsNamed(elementName))
            {
                if(node.GetAttribute(attrib, value).Count<XAttribute>() > 0)
                return node;
            }

            XNode nextOne = node.FirstChild();
            if (nextOne != null)
                n = nextOne.FindUnique(elementName, attrib, value);

            if (null != n)
                return n;

            if (node.NextNode != null)
                n = node.NextNode.FindUnique(elementName, attrib, value);

            return n;

        }

        public static XNode FirstChild(this XNode node)
        {
            try
            {
                return ((XElement)node).FirstNode;
            }
            catch
            {
                return null;
            }
        }

        public static XAttribute GetAttribute(this XNode node, string name)
        {
            try
            {
                return (from a in node.GetAttributes()
                        where a.Name.LocalName.ToLower() == name.ToLower()
                        select a).First<XAttribute>();
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<XAttribute> GetAttribute(this XNode node, string name, string value)
        {
            IEnumerable<XAttribute> attribs = from a in node.GetAttributes()
                                            where a.Name.LocalName.ToLower() == name.ToLower()
                                            && a.Value.ToLower() == value.ToLower()
                                            select a;
            return attribs;
            
        }

               public static IEnumerable<XAttribute> GetAttributes(this XNode node)
        {
            try
            {
                return ((XElement)node).Attributes();
            }
            catch
            {
                return new List<XAttribute>();
            }
        }

        public static string GetAttributeValue(this XNode node, string name)
        {
            try
            {
                return (from a in node.GetAttributes()
                        where a.Name.LocalName.ToLower() == name.ToLower()
                        select a).First<XAttribute>().Value;
            }
            catch
            {
                return null;
            }
        }

        public static XAttribute GetOrCreateAttribute(this XNode node, string attributeName, string value)
        {
            IEnumerable<XAttribute> atts = node.GetAttribute(attributeName, value);
            XAttribute result = new XAttribute(attributeName, value);
            if (atts.Count() > 0)
            {
                return atts.First<XAttribute>();
            }
            else
            {
                node.AddAttribute(result);
                return result;
            }
        }

        public static XNode GetOrCreateChild(this XNode node, string childName)
        {
            XNode childNode;
            IEnumerable<XNode> children = node.Nodes(childName);
            if (children.Count() < 1)
            {
                childNode = new XElement(childName);
                ((XElement)node).Add(childNode);
            }
            else
            {
                childNode = children.First<XNode>();
            }
            return childNode;
        }

        public static XNode GetOrCreateChild(this XNode node, string childName,
            string attribute, string value, bool atFirstPosition)
        {
            XNode childNode = node.FindUnique(childName, attribute, value);
            if (null == childNode)
            {
                XNode newNode = new XElement(childName);
                XAttribute att = new XAttribute(attribute, value);
                newNode.AddAttribute(att);
                if(atFirstPosition) ((XElement)node).AddFirst(newNode);
                else ((XElement)node).Add(newNode);
                return newNode;
            }
            else
            {
                return childNode;
            }
        }

        public static bool HasAttribute(this XNode node, string attributeName)
        {
            return (from a in node.GetAttributes()
                    where a.Name.LocalName.ToLower() == attributeName.ToLower()
                    select a).Count() > 0;
        }

        public static bool IsNamed(this XNode node, string name)
        {
            try
            {
                return node.NameLower() == name.ToLower();
            }
            catch { return false; }
        }

        public static string Name(this XNode node)
        {
            try
            {
                return ((XElement)node).Name.LocalName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string NameLower(this XNode node)
        {
            try
            {
                return ((XElement)node).Name.LocalName.ToLower();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static XNode Node(this XNode node, 
            string elementName, 
            string attrib, 
            string value)
        {
            return node.FindUnique(elementName, attrib, value);
        }

        public static IEnumerable<XNode> Nodes(this XNode node)
        {
            try
            {
                return ((XElement)node).Nodes();
            }
            catch
            {
                return new List<XNode>();
            }
        }

        public static IEnumerable<XNode> Nodes(this XNode node, string elementName)
        {
            try
            {
                IEnumerable<XNode> nodes = ((XElement)node).Nodes();
                return from cn in nodes
                       where cn.NameLower() == elementName.ToLower()
                       select cn;
            }
            catch
            {
                return new List<XNode>();
            }
        }

        public static void RemoveAttribute(this XNode node, string attributeName)
        {
            try
            {
                IEnumerable<XAttribute> attribs = node.GetAttributes();
                attribs = attribs.Remove(attributeName);
                ((XElement)node).RemoveAttributes();
                foreach (XAttribute a in attribs)
                    ((XElement)node).Add(a);
            }
            catch { }
        }

        public static XAttribute SetAttribute(this XNode node, string attrib, string value)
        {
            XAttribute att = node.GetAttribute(attrib);
            if (null != att)
            {
                att.Value = value;
            }
            else
            {
                att = new XAttribute(attrib, value);
                node.AddAttribute(att);
            }
            return att;
        }

        public static T Value<T>(this XNode node)
        {
            try
            {
                return (T)(object)(((XElement)node).Value);
            }
            catch { }
            return default(T);
        }

		#endregion Methods 
    }
