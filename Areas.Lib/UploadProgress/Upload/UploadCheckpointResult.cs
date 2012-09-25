namespace Areas.Lib.UploadProgress.Upload
{
    using System;
    using Areas.Lib.UploadProgress.Upload.AsyncUploadModels;

    public class UploadCheckpointResult
    {
        public bool InProgress { get; set; }

        public bool ProgressCounters { get; set; }

        public string CurrentOperationText { get; set; } //file name field  value  '00349.MTS'

        public string PrimaryTotal { get; set; } //'36.00MB'
        
        public string PrimaryValue { get; set; } //'33.00MB'

        public string Speed { get; set; } //'1.00MB/s'

        public long TimeElapsed { get; set; } //'31373'

        public long TimeEstimated { get; set; } //'2592'

        public bool OperationComplete { get; set; }

        public decimal? Total
        {
            get
            {
                if(PrimaryTotal.IsNullOrEmpty())
                {
                    return 0;
                }

                var val = PrimaryTotal.Substring(0, PrimaryTotal.ToLower().IndexOf("mb"));
                return Convert.ToDecimal(val);
            }
        }

        public decimal? Done
        {
            get
            {
                if (PrimaryValue.IsNullOrEmpty())
                {
                    return 0;
                }

                var logger = new UploadTrackingsService();
                try 
                {
                    var val = PrimaryValue.Substring(0, PrimaryValue.ToLower().IndexOf("mb"));
                    return Convert.ToDecimal(val);
                }
                catch(Exception errorInDone) 
                {
                    try
                    {
                        var val = PrimaryValue.Substring(0, PrimaryValue.ToLower().IndexOf("kb"));

                        decimal decimalValue = 0;

                        if (decimal.TryParse(val, out decimalValue))
                        {
                            return (decimalValue / 1024);
                        }                        
                    }
                    catch
                    {
                        try 
                        {
                            logger.Log("unknown", "UploadCheckpointResult.Done", "error occured", PrimaryValue);
                        }
                        catch 
                        {

                        }                        
                    }
                    
                    return 0;
                }
                
            }
        }
    }
}