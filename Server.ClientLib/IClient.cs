using System;
using Server.DataLib.Model;
using Server.DataLib.Filter;
using System.Collections.Generic;

namespace Server.ClientLib {

    public class EClientItemNotFound : Exception {}

    public interface IClient {

        public const int DEFAULT_LIMIT = 1000;
        public const int NO_OFFSET = 0;
        /**
        @return id of added item
        */
        public abstract int add<T>(T item) where T : BaseItem;
        public abstract List<T> getList<T>(int limit = DEFAULT_LIMIT, int offset = NO_OFFSET, BookFilter filter = null) where T : IItem;
        public abstract T get<T>(int id) where T : BaseItem;
        public abstract void update<T>(T item) where T : BaseItem;
        public abstract void remove<T>(int id) where T : IItem;
    }
}