// Generated by Reflector from D:\eConsular\src\Areas\Lib\RadWebUI\Areas.Lib.UploadProgress.dll
namespace Areas.Lib.UploadProgress
{
    using System;
    using System.Runtime.CompilerServices;

    public class ChunkMetaData
    {
        public ChunkMetaData()
        {
        }

        public ChunkMetaData(int chunkIndex, int totalChunks, string guid)
        {
            this.UploadID = guid;
            this.ChunkIndex = chunkIndex;
            this.TotalChunks = totalChunks;
            this.IsSingleChunkUpload = this.TotalChunks == 1;
        }

        public ChunkMetaData(int chunkIndex, int totalChunks, int totalFileSize, string guid)
            : this(chunkIndex, totalChunks, guid)
        {
            this.TotalFileSize = totalFileSize;
        }

        public int ChunkIndex { get; set; }

        public bool IsSingleChunkUpload { get; set; }

        public int TotalChunks { get; set; }

        public int TotalFileSize { get; set; }

        public string UploadID { get; set; }
    }
}