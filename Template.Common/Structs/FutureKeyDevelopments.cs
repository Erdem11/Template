using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Template.Common.Structs
{
    #region FutureKeyDevelopments

    public class MyClass
    {
        void A()
        {
            var a = new MyKegGen<(Guid, int)>();
            a.Id = (Guid.NewGuid(), 111);

            var b = new MyKegGen<MyGenKeyStruct>();
            b.Id = new MyGenKeyStruct()
            {
                IdKey1 = Guid.NewGuid(),
                IdKey2 = Guid.Empty
            };
            b = new MyGenKeyStruct();
        }
    }

    public struct MyGenKeyStruct : IEquatable<MyGenKeyStruct>
    {
        public Guid IdKey1 { get; set; }
        public Guid IdKey2 { get; set; }
        public bool Equals(MyGenKeyStruct other)
        {
            return IdKey1.Equals(other.IdKey1) && IdKey2.Equals(other.IdKey2);
        }
        public override bool Equals(object obj)
        {
            return obj is MyGenKeyStruct other && Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(IdKey1, IdKey2);
        }
    }

    public struct MyKegGen<T> : IEquatable<MyKegGen<T>> where T : IEquatable<T>
    {
        public T Id { get; set; }
        public T ToPrimitive() => Id;
        public bool Equals(MyKegGen<T> other) => Id.Equals(other.Id);

        public override bool Equals(object obj) => obj is MyKegGen<T> other && Equals(other);

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(MyKegGen<T> a, MyKegGen<T> b) => a.Id.Equals(b.Id);
        public static bool operator !=(MyKegGen<T> a, MyKegGen<T> b) => !(a == b);

        public static implicit operator MyKegGen<T>(T val)
        {
            return new()
            {
                Id = val
            };
        }

        public static explicit operator T(MyKegGen<T> val)
        {
            return val.Id;
        }

        private static ValueConverter<MyKegGen<T>, T> _converter;

        public static ValueConverter<MyKegGen<T>, T> Converter
        {
            get
            {
                return _converter ??= new ValueConverter<MyKegGen<T>, T>(v => v.Id, v => new MyKegGen<T>
                {
                    Id = v
                });
            }
        }

        public override string ToString() => JsonSerializer.Serialize(this);
        public static MyKegGen<T>? Parse(string s)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(s);
            }
            catch
            {
                return default;
            }
        }
    }

    public struct MyKeyIntInt : IEquatable<MyKeyIntInt>
    {
        public int Key1 { get; set; }
        public int Key2 { get; set; }
        public (int Key1, int Key2) ToPrimitive() => (Key1, Key2);
        public bool Equals(MyKeyIntInt other) => Key1 == other.Key1 && Key2 == other.Key2;

        public override bool Equals(object obj)
        {
            return obj is MyKeyIntInt other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<(int Key1, int Key2)>.Default.GetHashCode(ToPrimitive());
        }

        public static bool operator ==(MyKeyIntInt a, MyKeyIntInt b) => a.Key1 == b.Key1 && a.Key2 == b.Key2;
        public static bool operator !=(MyKeyIntInt a, MyKeyIntInt b) => !(a.Key1 == b.Key1 && a.Key2 == b.Key2);

        public static implicit operator MyKeyIntInt((int Key1, int Key2) val)
        {
            return new()
            {
                Key1 = val.Key1,
                Key2 = val.Key2
            };
        }

        public static explicit operator (int Key1, int Key2)(MyKeyIntInt val)
        {
            return (val.Key1, val.Key2);
        }

        private static ValueConverter<MyKeyIntInt, (int Key1, int Key2)> _converter;

        public static ValueConverter<MyKeyIntInt, (int Key1, int Key2)> Converter
        {
            get
            {
                return _converter ??= new ValueConverter<MyKeyIntInt, (int Key1, int Key2)>(v => v.ToPrimitive(), v => new MyKeyIntInt
                {
                    Key1 = v.Key1,
                    Key2 = v.Key2,
                });
            }
        }

        public override string ToString() => ToPrimitive().ToString();
        public static MyKeyIntInt? Parse(string s)
        {
            var removeParentheses = s.Trim('(').Trim(')');
            var parts = removeParentheses.Split(',');

            if (int.TryParse(parts[0].Trim(), out var key1))
            {
                if (int.TryParse(parts[1].Trim(), out var key2))
                {
                    return (key1, key2);
                }
            }

            return default;
        }
    }

    public struct MyKeyInt : IEquatable<MyKeyInt>
    {
        public int Id { get; set; }
        public int ToPrimitive() => Id;
        public bool Equals(MyKeyInt other) => Id.Equals(other.Id);

        public override bool Equals(object obj)
        {
            return obj is MyKeyInt other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<int>.Default.GetHashCode(Id);
        }

        public static bool operator ==(MyKeyInt a, MyKeyInt b) => a.Id == b.Id;
        public static bool operator !=(MyKeyInt a, MyKeyInt b) => !(a == b);

        public static implicit operator MyKeyInt(int val)
        {
            return new()
            {
                Id = val
            };
        }

        public static explicit operator int(MyKeyInt val)
        {
            return val.Id;
        }

        private static ValueConverter<MyKeyInt, int> _converter;

        public static ValueConverter<MyKeyInt, int> Converter
        {
            get
            {
                return _converter ??= new ValueConverter<MyKeyInt, int>(v => v.Id, v => new MyKeyInt
                {
                    Id = v
                });
            }
        }

        public override string ToString() => Id.ToString();
        public static MyKeyInt? Parse(string s)
        {
            return int.TryParse(s, out var myKey) ? myKey : default(MyKeyInt?);
        }
    }

    public struct MyKeyGuid : IEquatable<MyKeyGuid>
    {
        public Guid Id { get; set; }
        public Guid ToPrimitive() => Id;
        public bool Equals(MyKeyGuid other) => Id.Equals(other.Id);

        public override bool Equals(object obj)
        {
            return obj is MyKeyGuid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<Guid>.Default.GetHashCode(Id);
        }

        public static bool operator ==(MyKeyGuid a, MyKeyGuid b) => a.Id == b.Id;
        public static bool operator !=(MyKeyGuid a, MyKeyGuid b) => !(a == b);

        public static implicit operator MyKeyGuid(Guid val)
        {
            return new()
            {
                Id = val
            };
        }

        public static explicit operator Guid(MyKeyGuid val)
        {
            return val.Id;
        }

        private static ValueConverter<MyKeyGuid, Guid> _converter;

        public static ValueConverter<MyKeyGuid, Guid> Converter
        {
            get
            {
                return _converter ??= new ValueConverter<MyKeyGuid, Guid>(v => v.Id, v => new MyKeyGuid
                {
                    Id = v
                });
            }
        }

        public override string ToString() => Id.ToString();
        public static MyKeyGuid? Parse(string s)
        {
            return Guid.TryParse(s, out var myKey) ? myKey : default(MyKeyGuid?);
        }
    }

    #endregion
}