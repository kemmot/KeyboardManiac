using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Xml;

using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Plugins.FileSystemSearch
{
    public class FileSystemSearchPluginBase : SearchPluginBase
    {
        public const int FolderDepthDisabled = -1;

        private readonly static ILog Logger = LogManager.GetLogger(typeof(FileSystemSearchPluginBase));
        private readonly List<FileSystemSearchThread> m_SearchThreads = new List<FileSystemSearchThread>();
        private readonly object m_SearchThreadsSyncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSearchPlugin"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        protected FileSystemSearchPluginBase(IPluginHost host)
            : base(host)
        {
        }

        public string Folder { get; set; }
        private string[] Folders { get; set; }
        public int MaxFolderDepth { get; set; }
        public string IncludeFileMask { get; set; }
        private string[] IncludeFileMasks { get; set; }
        public bool IncludeFolders { get; set; }
        public bool IncludeHiddenItems { get; set; }
        public bool LogUnauthorizedAccessException { get; set; }
        public int SearchThreadCount { get; set; }

        override protected void DoSearch(CommandRequest parameters)
        {
            if (parameters.AliasCleansedCommandText.Length > 0)
            {
                FileSystemSearchThreadContext context = new FileSystemSearchThreadContext();
                context.AddFolders(Folders, 0);
                context.IncludeFileMasks.AddRange(IncludeFileMasks);
                context.Plugin = this;
                context.UpperSearchText = parameters.AliasCleansedCommandText.ToUpper();

                List<FileSystemSearchThread> localThreadCopy = new List<FileSystemSearchThread>();
                lock (m_SearchThreadsSyncRoot)
                {
                    for (int index = 0; index < SearchThreadCount; index++)
                    {
                        FileSystemSearchThread searchThread = new FileSystemSearchThread(context);
                        searchThread.ResultsFound += searchThread_ResultsFound;
                        searchThread.IsBackground = true;
                        searchThread.Name = string.Format("{0}-{1}", Thread.CurrentThread.Name, index + 1);
                        searchThread.Start();
                        m_SearchThreads.Add(searchThread);
                        localThreadCopy.Add(searchThread);
                    }
                }

                Thread.Sleep(100);

                foreach (FileSystemSearchThread searchThread in localThreadCopy)
                {
                    searchThread.Join();
                    searchThread.ResultsFound -= searchThread_ResultsFound;
                }
            }
        }
        void searchThread_ResultsFound(object sender, ItemEventArgs<List<SearchResultItem>> e)
        {
            OnResultsFound(e);
        }        
        /// <summary>
        /// Allows a plugin to run post initialisation checks.
        /// </summary>
        /// <remarks>
        /// Any exception thrown from this method will prevent the plugin from being used.
        /// </remarks>
        protected override void PostInitialiseCheck()
        {
            base.PostInitialiseCheck();

            Folders = Folder.Split(';');
            foreach (string expandedFolder in Folders)
            {
                if (!Directory.Exists(expandedFolder))
                {
                    throw new DirectoryNotFoundException(expandedFolder);
                }
            }

            IncludeFileMasks = IncludeFileMask.Split(';');

            if (MaxFolderDepth < FolderDepthDisabled)
            {
                string message = string.Format(
                    "Folder depth is invalid: {0}, must be {1}=no limit, 0=no sub folder traversing, positive integer for a limit on that number of subfolders to traverse",
                    MaxFolderDepth,
                    FolderDepthDisabled);
                throw new Exception(message);
            }
        }
        //public void RaiseResultsFoundEvent(ItemEventArgs<List<SearchResultItem>> e)
        //{
        //    OnResultsFound(e);
        //}
        protected override void DoStop()
        {
            base.DoStop();

            lock (m_SearchThreadsSyncRoot)
            {
                foreach (FileSystemSearchThread searchThread in m_SearchThreads)
                {
                    searchThread.ResultsFound -= searchThread_ResultsFound;
                    searchThread.Join(50, true, true);
                }
                m_SearchThreads.Clear();
            }
        }
    }

    public class FileSystemSearchThreadContext
    {
        private readonly List<string> m_IncludeFileMasks = new List<string>();
        private readonly Queue<SearchFolder> m_Folders = new Queue<SearchFolder>();
        private readonly object m_SyncRoot = new object();

        public List<string> IncludeFileMasks { get { return m_IncludeFileMasks; } }
        //public bool IncludeFolders { get; set; }
        //public bool IncludeHiddenItems { get; set; }
        //public bool LogUnauthorizedAccessException { get; set; }
        //public int MaxFolderDepth { get; set; }
        public FileSystemSearchPluginBase Plugin { get; set; }
        public string UpperSearchText { get; set; }
        //public bool UseShortNames { get; set; }

        public void AddFolders(IEnumerable<string> folders, int depth)
        {
            lock (m_SyncRoot)
            {
                foreach (string folder in folders)
                {
                    AddFolder(folder, depth);
                }
            }
        }
        public void AddFolder(string folder, int depth)
        {
            AddFolder(new SearchFolder(folder, depth));
        }
        private void AddFolder(SearchFolder folder)
        {
            lock (m_SyncRoot)
            {
                m_Folders.Enqueue(folder);
            }
        }
        public bool TryGetFolder(out SearchFolder folder)
        {
            bool folderAvailable;
            lock (m_SyncRoot)
            {
                folderAvailable = m_Folders.Count > 0;
                if (folderAvailable)
                {
                    folder = m_Folders.Dequeue();
                }
                else
                {
                    folder = null;
                }
            }
            return folderAvailable;
        }
    }

    public class SearchFolder
    {
        private readonly int m_Depth;
        private readonly string m_Folder;

        public SearchFolder(string folder, int depth)
        {
            m_Folder = folder;
            m_Depth = depth;
        }

        public int Depth { get { return m_Depth; } }
        public string Folder { get { return m_Folder; } }
    }

    public class FileSystemSearchThread : ThreadBase
    {
        private const int FolderDepthDisabled = FileSystemSearchPluginBase.FolderDepthDisabled;
        private const string ResultType_Directory = "Directory";
        private const string ResultType_File = "File";

        /// <summary>
        /// Fired when search results are found.
        /// </summary>
        public event EventHandler<ItemEventArgs<List<SearchResultItem>>> ResultsFound;

        private readonly static ILog Logger = LogManager.GetLogger(typeof(FileSystemSearchThread));
        private readonly FileSystemSearchThreadContext m_Context;

        public FileSystemSearchThread(FileSystemSearchThreadContext context)
        {
            m_Context = context;
        }

        protected override void InnerStart()
        {
            SearchFolder folder;
            do
            {
                if (!m_Context.TryGetFolder(out folder))
                {
                    Thread.Sleep(100);
                    m_Context.TryGetFolder(out folder);
                }

                if (folder != null)
                {
                    Search(folder.Folder, folder.Depth);
                }
            }
            while (folder != null);
        }
        private void Search(string folder, int depth)
        {
            List<SearchResultItem> results = new List<SearchResultItem>();
            AddFileResults(results, folder);
            AddFolderResults(results, folder, depth);
            
            if (!IsStopping && results.Count > 0)
            {
                OnResultsFound(new ItemEventArgs<List<SearchResultItem>>(results));
            }
        }
        private void AddFileResults(List<SearchResultItem> results, string folder)
        {
            List<string> files = GetFiles(folder);
            int matchCount = 0;
            foreach (string file in files)
            {
                if (IsStopping) break;

                if ((m_Context.Plugin.IncludeHiddenItems || (File.GetAttributes(file) & FileAttributes.Hidden) != FileAttributes.Hidden)
                    && IsMatch(Path.GetFileName(file)))
                {
                    string name = m_Context.Plugin.UseShortNames ? Path.GetFileName(file) : file;
                    results.Add(new SearchResultItem(name, ResultType_File, file));
                    matchCount++;
                }
            }

            Logger.DebugFormat(
                "Searched folder: {0}, {1}/{2} matches",
                folder,
                matchCount,
                files.Count);
        }
        private List<string> GetFiles(string folder)
        {
            List<string> files = new List<string>();
            if (!IsStopping)
            {
                try
                {
                    if (Directory.Exists(folder))
                    {
                        foreach (string fileMask in m_Context.IncludeFileMasks)
                        {
                            files.AddRange(Directory.GetFiles(folder, fileMask));
                        }
                    }
                    else
                    {
                        Logger.WarnFormat("Search folder does not exist: {0}", folder);
                    }
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    if (m_Context.Plugin.LogUnauthorizedAccessException)
                    {
                        Logger.ErrorFormat("Failed searching files in folder: {0}, {1}", folder, uaEx);
                    }
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Failed searching files in folder: {0}, {1}", folder, ex);
                }
            }
            return files;
        }
        private void AddFolderResults(List<SearchResultItem> results, string folder, int folderDepth)
        {
            List<string> subFolders = GetSubFolders(folder);

            int newFolderDepth = folderDepth + 1;
            bool canSearchSubFolders = m_Context.Plugin.MaxFolderDepth == FolderDepthDisabled || folderDepth < m_Context.Plugin.MaxFolderDepth;
            foreach (string subFolder in subFolders)
            {
                if (IsStopping) break;

                if (CanIncludeHiddenItem(subFolder))
                {
                    if (m_Context.Plugin.IncludeFolders)
                    {
                        if (IsMatch(Path.GetFileName(subFolder)))
                        {
                            string name = m_Context.Plugin.UseShortNames ? Path.GetFileName(subFolder) : subFolder;
                            results.Add(new SearchResultItem(name, ResultType_Directory, subFolder));
                        }
                    }

                    if (canSearchSubFolders)
                    {
                        m_Context.AddFolder(subFolder, newFolderDepth);
                    }
                }
            }
        }
        private bool CanIncludeHiddenItem(string path)
        {
            return m_Context.Plugin.IncludeHiddenItems || !IsHidden(path);
        }
        private bool IsHidden(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden;
        }
        private List<string> GetSubFolders(string folder)
        {
            List<string> subfolders = new List<string>();
            if (!IsStopping)
            {
                try
                {
                    if (Directory.Exists(folder))
                    {
                        subfolders.AddRange(Directory.GetDirectories(folder));
                    }
                    else
                    {
                        Logger.WarnFormat("Search folder does not exist: {0}", folder);
                    }
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    if (m_Context.Plugin.LogUnauthorizedAccessException)
                    {
                        Logger.ErrorFormat("Failed retrieving subfolders in folder: {0}, {1}", folder, uaEx);
                    }
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Failed retrieving subfolders in folder: {0}, {1}", folder, ex);
                }
            }
            return subfolders;
        }
        private bool IsMatch(string path)
        {
            return path.ToUpper().Contains(m_Context.UpperSearchText);
        }

        /// <summary>
        /// Fires the <see cref="ResultsFound"/> event.
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnResultsFound(ItemEventArgs<List<SearchResultItem>> e)
        {
            if (ResultsFound != null) ResultsFound(this, e);
        }
    }
}
