
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCNoJ.Minecraft
{
    public abstract class NBT
    {
        protected object value;
        public NBT(object value)
        {
            this.value = value;
        }
        public override abstract string ToString();
    }
    abstract public class ANBTList : NBT
    {
        public ANBTList(object value) : base(value) { }
        public string GetListString<InnerType>(string prefix)
        {
            List<string> items = new List<string>();
            foreach (InnerType item in (List<InnerType>)value)
            {
                items.Add(item.ToString());
            }
            return $"[{prefix}{string.Join(",", items)}]";
        }
    }
    /****************
    [ ] 0	TAG_End
    [x] 1	TAG_Byte	
    [x] 2	TAG_Short	
    [x] 3	TAG_Int	
    [x] 4	TAG_Long	
    [x] 5	TAG_Float	
    [x] 6	TAG_Double	
    [x] 7	TAG_Byte_Array
    [x] 8	TAG_String	
    [x] 9	TAG_List	
    [x] 10	TAG_Compound
    [x] 11	TAG_Int_Array
    [x] 12  TAG_Long_Array
    ****/
    public class NBTByte : NBT
    {
        public NBTByte(sbyte value) : base(value) { }
        public override string ToString()
        {
            return value.ToString() + "b";
        }
    }
    public class NBTShort : NBT
    {
        public NBTShort(short value) : base(value) { }
        public override string ToString()
        {
            return value.ToString() + "s";
        }
    }
    public class NBTInt : NBT
    {
        public NBTInt(int value) : base(value) { }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    public class NBTLong : NBT
    {
        public NBTLong(long value) : base(value) { }
        public override string ToString()
        {
            return value.ToString() + "l";
        }
    }
    public class NBTFloat : NBT
    {
        public NBTFloat(float value) : base(value) { }
        public override string ToString()
        {
            return value.ToString() + "f";
        }
    }
    public class NBTDouble : NBT
    {
        public NBTDouble(double value) : base(value) { }
        public override string ToString()
        {
            return value.ToString() + "d";
        }
    }
    public class NBTByteArray : ANBTList
    {
        public NBTByteArray(List<NBT> value) : base(value) { }
        override public string ToString()
        {
            return base.GetListString<NBT>("B;");
        }
    }
    public class NBTString : NBT
    {
        public NBTString(string value) : base(value) { }
        public override string ToString()
        {
            return "\"" + value + "\"";
        }
    }
    public class NBTList : ANBTList
    {
        public NBTList(List<NBT> value) : base(value) { }
        public override string ToString()
        {
            return base.GetListString<NBT>("");
        }
    }
    public class NBTCompund : NBT
    {
        public static readonly NBTCompund Empty = new NBTCompund(new Dictionary<string, NBT>());
        public NBTCompund(Dictionary<string, NBT> value) : base(value) { }
        public override string ToString()
        {
            List<string> items = new List<string>();
            foreach (KeyValuePair<string, NBT> item in (Dictionary<string, NBT>)value)
            {
                items.Add(item.Key + ":" + item.Value.ToString());
            }
            return "{" + string.Join(",", items) + "}";
        }
    }
    public class NBTIntArray : ANBTList
    {
        public NBTIntArray(List<NBT> value) : base(value) { }
        override public string ToString()
        {
            return base.GetListString<NBT>("I;");
        }
    }
    public class NBTLongArray : ANBTList
    {
        public NBTLongArray(List<NBT> value) : base(value) { }
        override public string ToString()
        {
            return base.GetListString<NBT>("L;");
        }
    }
}
