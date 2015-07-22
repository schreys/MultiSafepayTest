using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MultiSafePayTest.Business.Domain
{
    public class Entity : EntityWithTypedId<string>
    {

    }

    public class EntityWithTypedId<T>
    {
        [Key]
        public T Id { get; set; }

       
    }

    public class ValidationPatterns
    {
        public const string URL = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?";
        public const string EMAIL = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        public const string PNG = @"^.*\.png$";
        public const string SNAPSHOTFILE = @"^.*\.tse$";
        public const string ZIPFILE = @"^.*\.zip$";
        public const string TYRE_SIZE = @"^(P|LT|ST|T)?\d{3}(/\d{2,3})?(B|D|R)(?<size>\d+\.?\d*).*$";
        public const string DECIMAL = @"^\d+\.?\d*$";
    }
}
