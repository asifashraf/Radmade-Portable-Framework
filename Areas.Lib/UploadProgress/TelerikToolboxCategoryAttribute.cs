// Generated by Reflector from D:\eConsular\src\Areas\Lib\RadWebUI\Areas.Lib.UploadProgress.dll
namespace Telerik.Web
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TelerikToolboxCategoryAttribute : Attribute
    {
        public TelerikToolboxCategoryAttribute(string _categoryTitle)
        {
            this.CategoryTitle = _categoryTitle;
        }

        public string CategoryTitle { get; set; }
    }
}