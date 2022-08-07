using System;
using System.Collections.Generic;
using Server.DataLib.Model;
using Server.DataLib.Filter;
using Common;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;
using System.Web;


namespace Server.ClientLib {
    public class Client : IClient {
        public string Url { get; set; }

        /**
        @param string collection - collection to edit
        @param IItem item - item to add, passed by reference, because we need to modify Id 
        */
        public int add<T>(T item) where T : BaseItem {
            string result = Http.post( getCollectionUrl<T>() , item.ToJsonString() );
            T addedItem = JsonConvert.DeserializeObject<T>( result );
            return addedItem.Id;
        }
        public List<T> getList<T>(int limit = IClient.DEFAULT_LIMIT, int offset = IClient.NO_OFFSET, BookFilter filter = null ) where T : IItem {
            string query = "";
            if( filter != null ) {
                IEnumerable<string> properties = from p in filter.GetType().GetProperties()
                                where p.GetValue(filter, null) != null
                                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(filter, null).ToString());
                query = "?"+String.Join("&", properties.ToArray());    
            }
            string result = Http.get( getCollectionUrl<T>() + query );
            return JsonConvert.DeserializeObject<List<T>>( result );
        }
        public T get<T>(int id) where T : BaseItem {
            try {
                string result = Http.get( getCollectionUrl<T>() + "/"+id.ToString() );
                return JsonConvert.DeserializeObject<T>( result );
            } catch (EHttp404Error) {
                throw new EClientItemNotFound();
            }
        }
        public void update<T>(T item) where T : BaseItem {
            string result = Http.put( getCollectionUrl<T>()+"/"+item.Id.ToString(), item.ToJsonString() );
        }
        public void remove<T>(int id) where T : IItem {
            string result = Http.delete( getCollectionUrl<T>()+"/"+id.ToString() );
        }

        // private
        private string getCollectionUrl<T>() where T : IItem {
            return  Url + typeof(T).Name.ToString().ToLower(); 
        }
    }
}