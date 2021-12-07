//namespace TwitchAPI.Data
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using Microsoft.EntityFrameworkCore.ChangeTracking;
//    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//    using Newtonsoft.Json;

//    public class CollectionJsonValueConverter<T> : ValueConverter<ICollection<T>, string> where T : Enum
//    {
//        public CollectionJsonValueConverter() : base(
//          v => JsonConvert
//            .SerializeObject(v.Select(e => e).ToList()),
//          v => JsonConvert
//            .DeserializeObject<ICollection<string>>(v)
//            .Select(e => typeof(string), e).ToList())
//        {
//        }
//    }

//    public class CollectionValueComparer<T> : ValueComparer<ICollection<T>>
//    {
//        public CollectionValueComparer() : base((c1, c2) => c1.SequenceEqual(c2),
//          c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), c => (ICollection<T>)c.ToHashSet())
//        {
//        }
//    }
//}

//// KUDOS TO: https://gregkedzierski.com/essays/enum-collection-serialization-in-dotnet-core-and-entity-framework-core/
