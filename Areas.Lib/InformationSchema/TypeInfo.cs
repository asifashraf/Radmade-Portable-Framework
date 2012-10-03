using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.InformationSchema
{
    public enum TypeInfo
    {
        [StringValue("Int64")]
        bigint,
        [StringValue("Int32")]
        Int,
        [StringValue("Int16")]
        smallint,
        [StringValue("Byte")]
        tinyint,
        [StringValue("Boolean")]
        bit,
        [StringValue("Decimal")]
        Decimal,
        [StringValue("Double")]
        numeric,
        [StringValue("Decimal")]
        money,
        [StringValue("Decimal")]
        smallmoney,
        [StringValue("float")]
        Float,
        [StringValue("Double")]
        real,
        [StringValue("DateTime")]
        datetime,
        [StringValue("DateTime")]
        smalldatetime,
        [StringValue("String")]
        Char,
        [StringValue("String")]
        varchar,
        [StringValue("String")]
        text,
        [StringValue("String")]
        nchar,
        [StringValue("String")]
        nvarchar,
        [StringValue("String")]
        ntext,
        [StringValue("Byte[]")]
        binary,
        [StringValue("Byte[]")]
        varbinary,
        [StringValue("Byte[]")]
        image,
        [StringValue("Guid")]
        uniqueidentifier,
        [StringValue("null")]
        Null,
    }
}
