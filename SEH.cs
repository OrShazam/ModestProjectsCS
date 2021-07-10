using System;
using System.ComponentModel;
// lightweight class implementation of structured error handling 
namespace SEH
{
    class SEH
    {
        private SEH nextHandler;
        private string errCode;
        private Action<string> handle;
        public void Handle(string errCode)
        {
            SEH errorHandler = new SEH(this.errCode, this.handle);
            errorHandler.nextHandler = this.nextHandler;
            while (errorHandler != null)
            {
                if (errorHandler.errCode == errCode)
                {
                    handle(errCode);
                    return;
                }
                errorHandler = errorHandler.GetNextHandler();
            }
            Console.WriteLine("No error handler initialized for error {0}", errCode);

        }
        public SEH GetNextHandler()
        {
            return nextHandler;
        }
        public SEH (string errCode, Action<string> handle)
        {
            this.nextHandler = null;
            this.errCode = errCode;
            this.handle = handle;
        }
        public string GetErrCode()
        {
            return errCode;
        }
        public bool alreadyHandlesErr(string errCode)
        {
            SEH errorHandler = new SEH(this.errCode, this.handle);
            errorHandler.nextHandler = this.nextHandler;
            while (errorHandler != null)
            {
                if (errorHandler.errCode == errCode)
                    return true;
                errorHandler = errorHandler.GetNextHandler();
            }
            return false;
        }
        public void setNextHandler(string errCode, Action<string> handle)
        {
            if (alreadyHandlesErr(errCode))
            {
                Console.WriteLine("ERROR: Trying to overwrite handler for error code {0}", errCode);
                return;
            }
            this.nextHandler = new SEH(errCode, handle); // need to actually append I'm dumb

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> handle = errCode => { Console.WriteLine("It is what it is"); };
            string errCode = "TOOBIGTOFAIL";
            SEH errorHandler = new SEH(errCode, handle);
            int[] arr = new int[5];
            try
            {
                Console.Write(arr[10]);
            }
            // need to actually fill the error handler with error handlers for it to do anything useful
            catch (Exception e)
            {
                string errorCode = Convert.ToString((e.InnerException as Win32Exception).ErrorCode);
                errorHandler.Handle(errorCode);
            }
        }
    }
}
