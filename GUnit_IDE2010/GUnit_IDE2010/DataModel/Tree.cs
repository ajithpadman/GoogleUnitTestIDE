using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.DataModel
{
    public class TreeElement
    {
        
        object m_value = null;
        List<TreeElement> m_Children = new List<TreeElement>();
       
       
        public TreeElement()
        {

        }
       
        public TreeElement(object value)
        {
            m_value = value;
        }
     
        public List<TreeElement> Children
        {
            get { return m_Children; }
        }
        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
       
        public void  Add(TreeElement child)
        {
            m_Children.Add(child);
        }
        
    }
    public class Tree
    {
        TreeElement m_root;
        public Tree(TreeElement element)
        {
            m_root = element;

        }
        public Tree()
        {
            m_root = new TreeElement();
        }
      
        public Tree(object rootValue)
        {
            m_root = new TreeElement(rootValue);
        }
        public void Add(TreeElement child)
        {
            m_root.Add(child);
        }
        public void Add(object child)
        {
            TreeElement node = new TreeElement(child);
            m_root.Add(node);
        }


    }
}
