namespace Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DataStore
    {
        // TODO: Make Store a generic and remove generic info from DataStore
        private Dictionary<Type, Store> _lookup;

        private static DataStore _instance = new DataStore();

        public static DataStore Instance
        {
            get { return _instance ?? (_instance = new DataStore()); }
        }

        private DataStore()
        {
            _lookup = new Dictionary<Type, Store>();
        }

        private Store GetStore<T>() where T : BaseData
        {
            Type type = typeof(T);
            if (!_lookup.ContainsKey(type))
            {
                _lookup.Add(type, new Store());
            }

            return _lookup[type];
        }

        public void AddData<T>(BaseData inData) where T : BaseData
        {
            GetStore<T>().AddData(inData);
        }

        public List<BaseData> GetDataOfType<T>() where T : BaseData
        {
            return GetStore<T>().data;
        }

        public IEnumerable<T> GetTypedDataList<T>() where T : BaseData
        {
            return GetStore<T>().data.Cast<T>();
        }

        public T GetData<T>(string dataKey) where T : BaseData
        {
            return (T)GetStore<T>().GetData(dataKey);
        }

        public T GetData<T>(int id) where T : BaseData
        {
            return (T)GetStore<T>().GetData(id);
        }

        public void ReinitializeStores()
        {
            foreach (var store in _lookup.Values)
            {
                store.Dispose();
            }
            _lookup = new Dictionary<Type, Store>();
        }

        private class Store : IDisposable
        {
            internal List<BaseData> data = new List<BaseData>();
            bool _isDisposed;

            public void AddData(BaseData inData)
            {
                inData.Initialize();
                data.Add(inData);
            }

            public BaseData GetData(string dataKey)
            {
                var results = this.data.Where(p => p.InternalName() == dataKey).ToList();
                if (results.Count != 0)
                    return results[0];
                return null;
            }

            public BaseData GetData(int id)
            {
                // Potential error if multiple data with the same ID is provided
                var results = this.data.Where(p => p.GetID() == id).ToList();
                return results.Count != 0 ? results[0] : null;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_isDisposed)
                {
                    return;
                }

                if (disposing)
                {
                    data.Clear();
                }

                // Should this be in the disposing block?
                _isDisposed = true;
            }
        }
    }
}
