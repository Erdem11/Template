using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Template.Common.Structs
{
    public struct MyKey : IEquatable<MyKey>
    {
        public Guid Id { get; set; }
        public Guid ToPrimitive() => Id;
        public bool Equals(MyKey other) => Id.Equals(other.Id);

        public override bool Equals(object obj) => obj is MyKey other && Equals(other);

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(MyKey a, MyKey b) => a.Id == b.Id;
        public static bool operator !=(MyKey a, MyKey b) => !(a == b);

        public static implicit operator MyKey(Guid val)
        {
            return new()
            {
                Id = val
            };
        }

        public static explicit operator Guid(MyKey val)
        {
            return val.Id;
        }
        public static implicit operator MyKey(string val)
        {
            return new()
            {
                Id = Guid.Parse(val)
            };
        }

        public static explicit operator string(MyKey val)
        {
            return val.ToString();
        }

        private static ValueConverter<MyKey, Guid> _converter;

        public static ValueConverter<MyKey, Guid> Converter
        {
            get
            {
                return _converter ??= new ValueConverter<MyKey, Guid>(v => v.Id, v => new MyKey
                {
                    Id = v
                });
            }
        }

        public override string ToString() => Id.ToString();
        public static MyKey? Parse(string s)
        {
            return Guid.TryParse(s, out var myKey) ? myKey : default(MyKey?);
        }
        public bool IsDefault() => Id == default;
        public MyKey GenerateNewId() => Id = Guid.NewGuid();
    }
}