﻿
namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class RequestParser
    {
        private byte[] _boundary;
        private byte[] _bufferedBytes = new byte[0];
        private int _bufferedBytesLength = -1;
        private byte[] _chunkBoundary;
        private byte[] _crLf;
        private int _currentFieldStartIndexInBuffer = -1;
        private int _currentStartingBoundaryLength;
        private System.Text.Encoding _encoding;
        private byte[] _firstBoundary;
        private bool _firstBoundaryFound;
        private byte[] _lastBoundary;
        private bool _lastBoundaryFound;
        private byte[] _lastBoundarySuffix;
        private RequestStateStore _requestStateStore;
        private byte[] _searchedContentBoundary;

        public RequestParser(byte[] boundary, System.Text.Encoding encoding, RequestStateStore requestStateStore)
        {
            this._boundary = boundary;
            this._encoding = encoding;
            this._requestStateStore = requestStateStore;
            this._searchedContentBoundary = this.FirstBoundary;
        }

        private byte[] GetFieldBytes(byte[] chunk, int fieldStartIndex, int fieldBytesCount)
        {
            byte[] destinationArray = (byte[])Array.CreateInstance(typeof(byte), fieldBytesCount);
            if (fieldStartIndex >= this._bufferedBytes.Length)
            {
                Array.Copy(chunk, fieldStartIndex - this._bufferedBytes.Length, destinationArray, 0, fieldBytesCount);
                return destinationArray;
            }
            if (this._bufferedBytes.Length >= (fieldStartIndex + fieldBytesCount))
            {
                Array.Copy(this._bufferedBytes, fieldStartIndex, destinationArray, 0, fieldBytesCount);
                return destinationArray;
            }
            int length = this._bufferedBytes.Length - fieldStartIndex;
            int num2 = fieldBytesCount - length;
            Array.Copy(this._bufferedBytes, fieldStartIndex, destinationArray, 0, length);
            Array.Copy(chunk, 0, destinationArray, length, num2);
            return destinationArray;
        }

        private int GetFieldLength(byte[] chunk, int nextBoundaryStartIndex, int maxCountOfBytes, int fieldStartIndex)
        {
            if (nextBoundaryStartIndex < 0)
            {
                return (maxCountOfBytes - fieldStartIndex);
            }
            return (nextBoundaryStartIndex - fieldStartIndex);
        }

        private int GetFieldStartIndex(int currentBoundaryStartIndex)
        {
            if (currentBoundaryStartIndex < 0)
            {
                return 0;
            }
            return (currentBoundaryStartIndex + this._currentStartingBoundaryLength);
        }

        private int GetNextBoundaryIndex(byte[] chunk, int searchStart, int countOfBytesToSearch, out bool lastBoundary)
        {
            lastBoundary = false;
            int num = ByteComparer.IndexOf(this.LastBoundary, this._bufferedBytes, chunk, searchStart);
            int num2 = ByteComparer.IndexOf(this._searchedContentBoundary, this._bufferedBytes, chunk, searchStart);
            if ((num2 >= 0) && (num2 < countOfBytesToSearch))
            {
                return num2;
            }
            if (num >= (countOfBytesToSearch + this._bufferedBytes.Length))
            {
                return -1;
            }
            if (num >= 0)
            {
                lastBoundary = true;
            }
            return num;
        }

        private byte[] MergeArrays(byte[] array1, byte[] array2)
        {
            int length = array1.Length + array2.Length;
            byte[] destinationArray = (byte[])Array.CreateInstance(typeof(byte), length);
            Array.Copy(array1, destinationArray, array1.Length);
            Array.Copy(array2, 0, destinationArray, array1.Length, array2.Length);
            return destinationArray;
        }

        public void Parse(byte[] chunk, int validChunkBytes)
        {
            if (!this._lastBoundaryFound)
            {
                this.RequestStateStore.UpdateCurrentRequestBytesCount(validChunkBytes);
                bool flag = false;
                int nextBoundaryStartIndex = -1;
                int currentBoundaryStartIndex = -1;
                do
                {
                    int countOfBytesToSearch = (validChunkBytes - this.BufferedBytesLength) + this._bufferedBytes.Length;
                    nextBoundaryStartIndex = this.GetNextBoundaryIndex(chunk, currentBoundaryStartIndex + 1, countOfBytesToSearch, out this._lastBoundaryFound);
                    flag = nextBoundaryStartIndex >= 0;
                    if (!this._firstBoundaryFound)
                    {
                        if (!flag)
                        {
                            this.UpdateBufferedBytes(chunk);
                            return;
                        }
                        this._firstBoundaryFound = true;
                        this._currentStartingBoundaryLength = this._searchedContentBoundary.Length;
                        this._searchedContentBoundary = this.ChunkBoundary;
                        currentBoundaryStartIndex = nextBoundaryStartIndex;
                    }
                    else
                    {
                        int fieldStartIndex = this.GetFieldStartIndex(currentBoundaryStartIndex);
                        int fieldBytesCount = this.GetFieldLength(chunk, nextBoundaryStartIndex, countOfBytesToSearch, fieldStartIndex);
                        if (fieldBytesCount < 0)
                        {
                            this._currentFieldStartIndexInBuffer = fieldStartIndex - chunk.Length;
                        }
                        else
                        {
                            if (this._currentFieldStartIndexInBuffer >= 0)
                            {
                                fieldStartIndex = this._currentFieldStartIndexInBuffer;
                                fieldBytesCount -= this._currentFieldStartIndexInBuffer;
                            }
                            this._currentFieldStartIndexInBuffer = -1;
                            this.UpdateStateStore(chunk, fieldStartIndex, fieldBytesCount, nextBoundaryStartIndex >= 0);
                        }
                        this._currentStartingBoundaryLength = this._searchedContentBoundary.Length;
                        currentBoundaryStartIndex = nextBoundaryStartIndex;
                    }
                }
                while (flag && !this._lastBoundaryFound);
                if (!this._lastBoundaryFound)
                {
                    this.UpdateBufferedBytes(chunk);
                }
            }
        }

        private void ShiftBufferBytes(int currentMeaningBytes, int freeSpace)
        {
            int num = (currentMeaningBytes - this._bufferedBytes.Length) + freeSpace;
            for (int i = num; i < currentMeaningBytes; i++)
            {
                this._bufferedBytes[i - num] = this._bufferedBytes[i];
            }
        }

        private void UpdateBufferedBytes(byte[] chunk)
        {
            int bufferedBytesLength = this.BufferedBytesLength;
            int length = this._bufferedBytes.Length;
            if ((this._bufferedBytes.Length + chunk.Length) < this.BufferedBytesLength)
            {
                bufferedBytesLength = this._bufferedBytes.Length + chunk.Length;
            }
            if (this._bufferedBytes.Length != bufferedBytesLength)
            {
                Array.Resize<byte>(ref this._bufferedBytes, bufferedBytesLength);
            }
            if ((chunk.Length < bufferedBytesLength) && ((length + chunk.Length) > this.BufferedBytesLength))
            {
                this.ShiftBufferBytes(length, chunk.Length);
            }
            if (chunk.Length >= this._bufferedBytes.Length)
            {
                Array.Copy(chunk, chunk.Length - this._bufferedBytes.Length, this._bufferedBytes, 0, this._bufferedBytes.Length);
            }
            else
            {
                Array.Copy(chunk, 0, this._bufferedBytes, this._bufferedBytes.Length - chunk.Length, chunk.Length);
            }
        }

        private void UpdateStateStore(byte[] chunk, int fieldStartIndex, int fieldBytesCount, bool isFinal)
        {
            byte[] fieldContent = this.GetFieldBytes(chunk, fieldStartIndex, fieldBytesCount);
            this.RequestStateStore.Record(fieldContent, isFinal);
        }

        private byte[] Boundary
        {
            get
            {
                return this._boundary;
            }
        }

        private int BufferedBytesLength
        {
            get
            {
                if (this._bufferedBytesLength < 0)
                {
                    this._bufferedBytesLength = this.Boundary.Length + (2 * this.CrLf.Length);
                }
                return this._bufferedBytesLength;
            }
        }

        private byte[] ChunkBoundary
        {
            get
            {
                if (this._chunkBoundary == null)
                {
                    this._chunkBoundary = this.MergeArrays(this.MergeArrays(this.CrLf, this.Boundary), this.CrLf);
                }
                return this._chunkBoundary;
            }
        }

        private byte[] CrLf
        {
            get
            {
                if (this._crLf == null)
                {
                    this._crLf = new byte[] { 13, 10 };
                }
                return this._crLf;
            }
        }

        private System.Text.Encoding Encoding
        {
            get
            {
                return this._encoding;
            }
        }

        private byte[] FirstBoundary
        {
            get
            {
                if (this._firstBoundary == null)
                {
                    this._firstBoundary = this.MergeArrays(this.Boundary, this.CrLf);
                }
                return this._firstBoundary;
            }
        }

        private byte[] LastBoundary
        {
            get
            {
                if (this._lastBoundary == null)
                {
                    this._lastBoundary = this.MergeArrays(this.MergeArrays(this.CrLf, this.Boundary), this.LastBoundarySuffix);
                }
                return this._lastBoundary;
            }
        }

        private byte[] LastBoundarySuffix
        {
            get
            {
                if (this._lastBoundarySuffix == null)
                {
                    this._lastBoundarySuffix = this.Encoding.GetBytes("--");
                }
                return this._lastBoundarySuffix;
            }
        }

        private RequestStateStore RequestStateStore
        {
            get
            {
                return this._requestStateStore;
            }
        }
    }
}