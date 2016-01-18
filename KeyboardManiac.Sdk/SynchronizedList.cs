using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardManiac.Sdk
{
    public class SynchronizedList<T>
    {
        private readonly List<T> m_Items = new List<T>();
        private readonly object m_SyncRoot = new object();

        public void AddRange(IEnumerable<T> newItems)
        {
            lock (m_SyncRoot)
            {
                m_Items.AddRange(newItems);
            }
        }

        public void Add(T newItem)
        {
            lock (m_SyncRoot)
            {
                m_Items.Add(newItem);
            }
        }

        public void Clear()
        {
            lock (m_SyncRoot)
            {
                m_Items.Clear();
            }
        }

        public List<T> ToList()
        {
            List<T> list;
            lock (m_SyncRoot)
            {
                list = new List<T>(m_Items);
            }

            return list;
        }
    }
}
