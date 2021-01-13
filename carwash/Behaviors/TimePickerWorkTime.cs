using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

//namespace carwash.Behaviors
//{
//    public class EntryNumberValidator : Behavior<Entry>
//    {
//        protected override void OnAttachedTo(Entry entry)
//        {
//            entry.TextChanged += OnEntryTextChanged;
//            base.OnAttachedTo(entry);
//        }

//        protected override void OnDetachingFrom(Entry entry)
//        {
//            entry.TextChanged -= OnEntryTextChanged;
//            base.OnDetachingFrom(entry);
//        }

//        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
//        {
//            double result;
//            bool isValid = double.TryParse(args.NewTextValue, out result);
//            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Crimson;
//            ((Entry)sender).BackgroundColor = isValid ? Color.Default : Color.Default;

//        }
//    }
//}
namespace carwash.Behaviors
{
    class TimePickerWorkTime : Behavior<TimePicker>
    {
        protected override void OnAttachedTo(TimePicker timePicker)
        {
            base.OnAttachedTo(timePicker);
        }
        protected override void OnDetachingFrom(TimePicker timePicker)
        {
            base.OnDetachingFrom(timePicker);
        }
    }
}
