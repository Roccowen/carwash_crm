using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace carwash.Models
{
    public class ErrorController
    {
        private Xamarin.Forms.Label Result;
        private LinkedList<string> errorList;
        public ErrorController(Xamarin.Forms.Label result)
        {
            Result = result;
            errorList = new LinkedList<string>();
        }
        public void AddError(string error)
        {
            if (error != "" && !errorList.Contains(error))
            {
                errorList.AddLast(error);
                Result.Text = errorList.First();
            }
        }
        public void DelError(string error)
        {
            errorList.Remove(error);
            try
            {
                Result.Text = errorList.First();
            }
            catch
            {
                Result.Text = "";
            }
        }
    }
}
