using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace carwash.Behaviors
{
    public class EntryTextValidator : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            Regex reg = new Regex("[0-9]");
            bool isValid = reg.IsMatch(args.NewTextValue);
            ((Entry)sender).TextColor = isValid ? Color.Crimson : Color.Default;
            ((Entry)sender).BackgroundColor = isValid ? Color.Default : Color.Default;
        }
    }
}
