/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.IO;

namespace WDToolbox.Data.Text
{
    /// <summary>
    /// Lets you use events to deal with stream events (OnRead, OnWrite).
    /// Optionally this proxies another stream.
    /// </summary>
    public class StreamEventWrapper : Stream
    {
        /// <summary>
        /// The underlying stream (if used).
        /// </summary>
        public Stream Proxy {get; protected set;}
        
        /// <summary>
        /// True is an underlying stream is used.
        /// </summary>
        public bool HasProxy { get { return Proxy != null; } }

        /// <summary>
        /// Event for on read.
        /// </summary>
        public Action<byte[], int , int > OnRead {get; set;}
        
        /// <summary>
        /// Event for on write.
        /// </summary>
        public Action<byte[], int, int> OnWrite { get; set; }


        /// <summary>
        /// Creates a new StreamEventWrapper, using only event handling.
        /// </summary>
        public StreamEventWrapper() : this (null)
        {
        }

        /// <summary>
        /// Creates a new StreamEventWrapper, with a proxy stream.
        /// </summary>
        public StreamEventWrapper(Stream proxy)
        {
            Proxy = proxy;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            OnRead.SafeCall(buffer, offset, count);
            return HasProxy ? Proxy.Read(buffer, offset, count) : count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            OnWrite.SafeCall(buffer, offset, count);
            if (HasProxy)
            {
                Proxy.Write(buffer, offset, count);
            }
        }

        public override bool CanRead { get { return HasProxy ? Proxy.CanRead : (OnRead != null); } }

        public override bool CanSeek { get { return HasProxy ? Proxy.CanSeek : false; } }

        public override bool CanWrite { get { return HasProxy ? Proxy.CanWrite: (OnWrite != null); } }

        public override void Flush() { if (HasProxy) { Proxy.Flush(); } }

        public override long Length { get { return HasProxy ? Proxy.Length : 0; } }

        public override long Position 
        {
            get
            {
                return HasProxy ? Proxy.Position : 0;
            }
            set
            {
                if (HasProxy)
                {
                    Proxy.Position = value;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return HasProxy ? Proxy.Seek(offset, origin) : 0;
        }

        public override void SetLength(long value)
        {
            if (HasProxy)
            {
                Proxy.SetLength(value);
            }
        }

        public override void Close()
        {
            if (HasProxy)
            {
                Proxy.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (HasProxy)
            {
                Proxy.Dispose();
            }
        }

    }
}
