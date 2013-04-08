using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Areas.DotNetExtensions
{
    public class ManualConvert
    {
        object Value { get; set; }  
        public ManualConvert(object value)
        {
            this.Value = value;
        }

        public Object ConvertType(Type targetType)
        {
            if (targetType.GenericTypeArguments.Count() == 0)
            {
                return Convert.ChangeType(this.Value, targetType);
            }
            else
            {
                Type basicType = Type.GetType(targetType.GenericTypeArguments[0].FullName);
                return Convert.ChangeType(this.Value, basicType);
            }
        }

        public Object ConvertType(string typeFullName)
        {

            var targetType = Type.GetType(typeFullName);

            return ConvertType(targetType);
        }

        //public object ConvertByType(Type targetType)
        //{
        //    var targetTypeFullName = targetType.GenericTypeArguments[0].FullName;
        //    bool isNullable = targetType.FullName.Contains("Nullable");
        //    {
        //        if (targetTypeFullName == "System.String")
        //        {
        //            return this.Value;
        //        }
        //    }

        //    //Int32
        //    if(targetTypeFullName == "System.Int32"){
        //        System.Int32 result = default(System.Int32);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        result = Convert.ToInt32(this.Value);
        //    }
        //}

        //public Object ToType(string targetTypeFullName)
        //{
        //    //String
        //    {
        //        System.String result = default(System.String);
        //        if (targetTypeFullName == "System.String")
        //        {
        //            return this.Value;
        //        }
        //    }

        //    //Int32
        //    {
        //        System.Int32 result = default(System.Int32);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToInt32(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Int32?
        //        System.Int32? result = default(System.Int32?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                return result = Convert.ToInt32(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Int64
        //        System.Int64 result = default(System.Int64);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToInt64(this.Value);
        //            } 
        //            return result;
        //        }
        //    }

        //    {
        //        //Int64?
        //        System.Int64? result = default(System.Int64?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToInt64(this.Value);
        //            }
        //        }
        //    }

        //    {
        //        //Int16
        //        System.Int16 result = default(System.Int16);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result =  Convert.ToInt16(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Int16
        //        System.Int16? result = default(System.Int16?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToInt16(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Boolean
        //        System.Boolean result = default(System.Boolean);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToBoolean(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Boolean?
        //        System.Boolean? result = default(System.Boolean?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToBoolean(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Byte
        //        System.Byte result = default(System.Byte);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToByte(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Byte?
        //        System.Byte? result = default(System.Byte?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToByte(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Char
        //        System.Char result = default(System.Char);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToChar(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Char?
        //        System.Char? result = default(System.Char?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToChar(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //DateTime
        //        System.DateTime result = default(System.DateTime);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToDateTime(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //DateTime?
        //        System.DateTime? result = default(System.DateTime?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToDateTime(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Decimal
        //        System.Decimal result = default(System.Decimal);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToDecimal(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Decimal
        //        System.Decimal? result = default(System.Decimal?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToDecimal(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Double
        //        System.Double result = default(System.Double);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToDouble(this.Value);
        //            }
        //            return result;
        //        }
        //    }

        //    {
        //        //Double?
        //        System.Double? result = default(System.Double?);
        //        if (this.Value.IsNullOrEmpty())
        //        {
        //            return result;
        //        }
        //        if (targetTypeFullName == result.GetType().FullName)
        //        {
        //            if (this.Value.IsNotNullOrEmpty())
        //            {
        //                result = Convert.ToDouble(this.Value);
        //            }
        //            return result;
        //        }
        //    }

            

        //    return this.Value;
        //}
    }
}
