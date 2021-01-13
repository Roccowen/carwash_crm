using System.Collections.Generic;
using System.Linq;

namespace carwash.Models
{
    public class ErrorService
    {
        private Xamarin.Forms.Label Result;
        private LinkedList<string> errorList;
        public ErrorService(Xamarin.Forms.Label result)
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
        public void ClearErrors()
        {
            errorList.Clear();
            Result.Text = "";
        }
        public bool HasError() => errorList.Count > 0;
    }
}
