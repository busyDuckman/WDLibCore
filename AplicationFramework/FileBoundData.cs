/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.IO;

namespace WDToolbox.AplicationFramework
{
    /// <summary>
    /// This works with IReloadFile to provide a system for keeping a data object up to date with the file
    /// that it represents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileBoundData<T>
        where T : class, IReloadFile
    {
        //-------------------------------------------------------------------------------------------
        // Instance Data
        //-------------------------------------------------------------------------------------------
        bool _boundToFile { get; set; }
        bool BoundToFile {
            get { return _boundToFile; } 
            set
            {
                lock (FileBindingLock)
                {
                    _boundToFile = value;
                }
                init();
            } 
        }

        string _sourceFile;
        public string SourceFile
        {
            get { return _sourceFile; }
            set 
            {
                lock (FileBindingLock)
                {
                    _sourceFile = value;
                }
                init();
            }
        }

        bool SourceFileExists
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SourceFile))
                {
                    if (File.Exists(SourceFile))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        WeakReference<T> Data;

        //-------------------------------------------------------------------------------------------
        // Not persisted
        //-------------------------------------------------------------------------------------------
        [NonSerialized]
        protected FileSystemWatcher SourceFileWatcher;
        [NonSerialized]
        private object _fileBindingLock = new object();
        public object FileBindingLock
        {
            get 
            {
                if (_fileBindingLock == null)
                {
                    _fileBindingLock = new object();
                }
                return _fileBindingLock; 
            }
        }

        //-------------------------------------------------------------------------------------------
        // Constructors and setup
        //-------------------------------------------------------------------------------------------
        public FileBoundData(T data, string path) : this(data, path, true)
        {
        }

        public FileBoundData(T data, string path, bool bound)
        {
            Data = new WeakReference<T>(data);
            SourceFile = path;
            BoundToFile = bound;
        }

        public void init()
        {
            lock (FileBindingLock)
            {
                if (SourceFileWatcher != null)
                {
                    SourceFileWatcher.Dispose();
                    SourceFileWatcher = null;
                }

                if (SourceFileExists && BoundToFile)
                {
                    SourceFileWatcher = new FileSystemWatcher(Path.GetDirectoryName(SourceFile), Path.GetFileName(SourceFile));
                    SourceFileWatcher.Changed += OnChanged;
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        // Members
        //-------------------------------------------------------------------------------------------
        // Define the event handlers. 
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            T item;
            if (Data.TryGetTarget(out item))
            {
                item.ReloadFile(e.FullPath);
            }
            else
            {
                //items was disposed, remove the watcher.
                if (SourceFileWatcher != null)
                {
                    SourceFileWatcher.Dispose();
                    SourceFileWatcher = null;
                }
            }
        }
    }
}
