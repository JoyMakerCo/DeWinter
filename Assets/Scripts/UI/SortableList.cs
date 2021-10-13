using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public abstract class SortableItem<T> : MonoBehaviour
    {
        public virtual T Data { get; set; }
    }

    public abstract class SortableList<T> : MonoBehaviour, IDisposable
    {
        /// PUBLIC DATA ///////////////////////////
        public SortableItem<T> ListItem;
        public string[] SortParams;
        public Dropdown SortDropdown;

        /// PRIVATE/PROTECTED DATA ///////////////////////////
        private Comparison<T> _comparer;
        private List<SortableItem<T>> _pool = null;
        private T[] _data = null;

        /// PROPERTIES ///////////////////////////
        public int Count { get; private set; }
        
        /// PUBLIC METHODS ///////////////////////////
        public void Refresh()
        {
            _data = RefreshListData(_data);
            Sort();
        }

        public virtual void Dispose() {}
        public virtual void Initialize() {}

        /// PRIVATE/PROTECTED METHODS ///////////////////////////

        protected void OnEnable()
        {
            if (_pool == null)
            {
                _pool = new List<SortableItem<T>>() { ListItem };
                if (SortDropdown != null)
                {
                    List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
                    Dropdown.OptionData option;
                    string loc;
                    SortDropdown.ClearOptions();
                    foreach (string sortParam in SortParams)
                    {
                        loc = AmbitionApp.Localize(sortParam);
                        option = new Dropdown.OptionData(loc);
                        options.Add(option);
                    }
                    SortDropdown.AddOptions(options);
                    SortDropdown.onValueChanged.AddListener(Sort);
                    if (SortDropdown.options.Count > 0) SortDropdown.value = 0;
                    _comparer = GetComparer(0);
                }
            }
            if (_data == null)
            {
                _data = FetchListData();
            }
            else
            {
                _data = RefreshListData(_data);
            }
            Initialize();
            Sort();
        }

        protected void OnDestroy()
        {
            Dispose();
            Transform x = ListItem.transform.parent;
            for (int i = x.childCount - 1; i >= 1; --i)
            {
                Destroy(x.GetChild(i)?.gameObject);
            }
            _pool?.ForEach(i => (i as IDisposable)?.Dispose());
            _pool?.Clear();
            _pool = null;
            _comparer = null;
            _data = null;
            SortDropdown?.onValueChanged.RemoveListener(Sort);
        }

        private void Sort(int sortIndex)
        {
            _comparer = (sortIndex >= 0 && sortIndex < SortParams?.Length)
                ? GetComparer(sortIndex)
                : null;
            Sort();
        }


        private void Sort()
        {
            if (_data != null && _pool != null)
            {
                GameObject obj;
                if (_comparer != null) Array.Sort(_data, _comparer);
                Count = _data.Length;
                for (int i = _pool.Count - 1; i >= Count; --i)
                {
                    _pool[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < Count; ++i)
                {
                    if (i == _pool.Count)
                    {
                        obj = Instantiate<GameObject>(ListItem.gameObject, ListItem.gameObject.transform.parent);
                        _pool.Add(obj.GetComponent<SortableItem<T>>());
                    }
                    _pool[i].Data = _data[i];
                    _pool[i].gameObject.SetActive(true);
                }
            }
        }

        protected virtual Comparison<T> GetComparer(int sortIndex) => (x, y) => 0;

        protected abstract T[] FetchListData();
        protected virtual T[] RefreshListData(T[] currentData) => FetchListData();
    }
}
