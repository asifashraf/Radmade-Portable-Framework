using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections;


public class WebClientX : WebClient
    {
		#region Fields (1) 

        private bool BreakTheLoop = false;

		#endregion Fields 

		#region Methods (3) 

		// Public Methods (1) 

        public DownloadState ValidateRemoteFileStatus(
            string url,
            string localPath,
            params string[] allowedExtensions)
        {
            this.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WebClientX_DownloadProgressChanged);
            Uri uri = new Uri(url);
            DownloadState state = new DownloadState(uri);
            state.AllowedExtensions = allowedExtensions.ToList<string>();

            System.Timers.Timer timer = new System.Timers.Timer(15000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            
            this.DownloadFileAsync(uri, localPath, state);
            int countH = 0;
            
            while (state.ValidationStatus == -1)
            {
                if (this.BreakTheLoop)
                {
                    state.ValidationStatus = 0;
                    return state;
                }

                if (null != this.ResponseHeaders)
                {
                    countH = this.ResponseHeaders.Count;

                    //--
                    WebHeaderCollection headers = this.ResponseHeaders;
                    IEnumerator keys = headers.Keys.GetEnumerator();                    
                    state.ValidationStatus = 0;
                    string urlLower = state.Uri.OriginalString.ToLower();
                    string key = string.Empty;
                    string value = string.Empty;
                    bool found = false;
                    while (keys.MoveNext())
                    {
                        string current = keys.Current.ToString();
                        if (headers[current].ToLower().Contains("filename"))
                        {
                            key = current;
                            value = headers[current].ToString();
                            found = true;
                            break;
                        }
                    }
                    string realFileName = string.Empty;
                    if (found)
                    {
                        state.FileName = "not imp";
                        state.FileFormat = "not imp";
                        state.FileSize = "not imp";
                        state.ValidationStatus = 1;                        
                    }
                    else
                    {
                        if (urlLower.Contains("."))
                        {
                            string ext = urlLower.Substring(urlLower.LastIndexOf('.'));
                            if (allowedExtensions.Contains<string>(ext))
                            {
                                state.FileName = urlLower.Substring(urlLower.LastIndexOf('/') + 1);
                                state.FileFormat = ext;
                                state.ValidationStatus = 1;
                                found = true;
                            }
                        }
                    }

                    if (!found)
                    {
                        state.ValidationStatus = 0;                        
                    }
                    timer.Stop();
                    timer.Dispose();
                   return state;
                }
                
                
            }

            return state;
        }
		// Private Methods (2) 

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.BreakTheLoop = true;
            System.Timers.Timer t = (System.Timers.Timer)sender;
            t.Stop();
            t.Dispose();
        }

        void WebClientX_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this.ResponseHeaders != null)
                this.CancelAsync();
            return;
        }

		#endregion Methods 
    }

    public class DownloadState
    {
		#region Constructors (2) 

        public DownloadState(Uri uri)
        {
            this.Uri = uri;
            this.ValidationStatus = -1;
            this.Message = string.Empty;
            this.AllowedExtensions = new List<string>();
            this.FileName = string.Empty;
            this.FileSize = string.Empty;
            this.FileFormat = string.Empty;
        }

        public DownloadState() { }

		#endregion Constructors 

		#region Properties (7) 

        public List<string> AllowedExtensions { get; set; }

        public string FileFormat { get; set; }

        public string FileName { get; set; }

        public string FileSize { get; set; }

        public string Message { get; set; }

        public Uri Uri { get; set; }

        public int ValidationStatus { get; set; }

		#endregion Properties 

		#region Methods (2) 

		// Public Methods (2) 

        public void GetState(out int validationStatus, out string msg,
            out string fileName, out string fileSize, out string format)
        {
            validationStatus = this.ValidationStatus;
            msg = this.Message;
            fileName = this.FileName;
            fileSize = this.FileSize;
            format = this.FileFormat;
        }

        public void SetState(int validationStatus, string msg,
            string fileName, string fileSize, string format)
        {
            this.ValidationStatus = validationStatus;
            this.Message = msg;
            this.FileName = FileName;
            this.FileSize = FileSize;
            this.FileFormat = format;
            this.ValidationStatus = validationStatus;
        }

		#endregion Methods 
    }

