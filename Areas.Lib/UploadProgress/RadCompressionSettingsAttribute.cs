// Generated by Reflector from D:\eConsular\src\Areas\Lib\RadWebUI\Areas.Lib.UploadProgress.dll
namespace Areas.Lib.UploadProgress
{
    using System;

    public class RadCompressionSettingsAttribute : Attribute
    {
        private bool _commpressRegularPostbacks;
        private CompressionType _httpCompression = CompressionType.GZip;
        private CompressionType _stateCompression = CompressionType.GZip;

        public bool EnablePostbackCompression
        {
            get
            {
                return this._commpressRegularPostbacks;
            }
            set
            {
                this._commpressRegularPostbacks = value;
            }
        }

        public CompressionType HttpCompression
        {
            get
            {
                return this._httpCompression;
            }
            set
            {
                this._httpCompression = value;
            }
        }

        public CompressionType StateCompression
        {
            get
            {
                return this._stateCompression;
            }
            set
            {
                this._stateCompression = value;
            }
        }
    }
}
