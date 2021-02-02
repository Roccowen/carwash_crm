using carwash.Services;
using Xamarin.Forms;

namespace carwash.Behaviors
{
    public class EntryPasswordValidator : Behavior<Entry>
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
            bool isValid = ValidService.passwordCheck.IsMatch(args.NewTextValue);
            ((Entry)sender).TextColor = !isValid ? Color.Crimson : Color.Default;
            ((Entry)sender).BackgroundColor = !isValid ? Color.Default : Color.Default;
        }
    }
}
