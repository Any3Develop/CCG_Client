﻿using System.Collections.Generic;
using System.Linq;
using Demo.Core.Abstractions.Game.Collections;
using Demo.Core.Abstractions.Game.Data;

namespace Demo.Core.Game.Collections
{
    public class DataCollection<TData> : List<TData>, IDataCollection<TData> where TData : IData
    {
        public TData Get(string id)
        {
            return this.FirstOrDefault(x => x.Id == id);
        }

        public T Get<T>(string id) where T : TData
        {
            return (T) Get(id);
        }

        public IEnumerable<TData> GetRange(IEnumerable<string> ids)
        {
            return ids.Select(Get);
        }

        public IEnumerable<T> GetRange<T>(IEnumerable<string> ids) where T : TData
        {
            return GetRange(ids).OfType<T>();
        }
    }
}