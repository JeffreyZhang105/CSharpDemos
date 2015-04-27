using System;
using System.Collections.Generic;
using System.Reflection;

namespace demos.Reflaction
{
    public class ValueBitAttribute : Attribute
    {
        public int ValueBit { get; private set; }

        public ValueBitAttribute(int valueBit)
        {
            this.ValueBit = valueBit;
        }
    }

    public class PasswordCreator
    {
        [ValueBit(4)] private List<int> markerList = new List<int>();

        [ValueBit(2)] private List<int> numberList = new List<int>();

        [ValueBit(1)] private List<int> lowerList = new List<int>();

        [ValueBit(3)] private List<int> upperList = new List<int>();

        private  List<int> characterList=new List<int>();

        public PasswordCreator()
        {
            Func<int, int, List<int>> toRange =
                (int from, int to) =>
                {
                    var result = new List<int>();
                    for (int i = from; i <= to; i++)
                        result.Add(i);
                    return result;
                };

            markerList.Add(0x21);
            markerList.AddRange(toRange(0x23, 0x26));
            markerList.AddRange(toRange(0x2a, 0x2b));
            markerList.AddRange(toRange(0x3f, 0x40));
            numberList.AddRange(toRange(0x30, 0x39));
            lowerList.AddRange(toRange(0x41, 0x5a));
            upperList.AddRange(toRange(0x61, 0x7a));
        }

        public string CreatePassword(int type, int length)
        {
            var result = string.Empty;

            var t = GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var members = t.GetMembers(flags);

            foreach (var member in members)
            {
                var bitAttr = member.GetCustomAttribute(typeof (ValueBitAttribute)) as ValueBitAttribute;
                if (bitAttr != null && ((1 << bitAttr.ValueBit - 1 | type) == type))
                {
                    var fieldInfo = t.GetField(member.Name, flags);
                    if (fieldInfo != null)
                    {
                        var list = fieldInfo.GetValue(this) as List<int>;
                        if (list != null)
                        {
                            characterList.AddRange(list);
                        }
                    }
                }
            }

            var random = new Random();
            while (result.Length < length)
            {
                var index = random.Next(0, characterList.Count);
                result += (char)characterList[index];
            }

            return result;
        }
    }
}