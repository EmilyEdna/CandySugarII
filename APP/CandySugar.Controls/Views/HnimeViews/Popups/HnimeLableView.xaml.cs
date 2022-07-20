using CandySugar.Library.TempControl;
using Mopups.Pages;
using Sdk.Component.Hnime.sdk.ViewModel.Request;

namespace CandySugar.Controls.Views.HnimeViews.Popups;

public partial class HnimeLableView : PopupPage
{

    HnimeSearch Query;
    public HnimeLableView()
    {
        InitializeComponent();
        Query = new HnimeSearch
        {
            Tags = new List<string>(),
            Brands = new List<string>()
        };
    }

    private void TagEvent(object sender, CheckedChangedEventArgs e)
    {
        var Check = (CandyCheckBox)sender;
        if (Check.IsChecked)
        {
            if (!Query.Tags.Contains(Check.Text))
                Query.Tags.Add(Check.Text);
        }
        else
        {
            if (Query.Tags.Contains(Check.Text))
                Query.Tags.Remove(Check.Text);
        }
    }

    private void BrandEvent(object sender, CheckedChangedEventArgs e)
    {
        var Check = (CandyCheckBox)sender;
        if (Check.IsChecked)
        {
            if (!Query.Brands.Contains(Check.Text))
                Query.Brands.Add(Check.Text);
        }
        else
        {
            if (Query.Brands.Contains(Check.Text))
                Query.Brands.Remove(Check.Text);
        }
    }

    private void TypeEvent(object sender, CheckedChangedEventArgs e)
    {
        Query.HnimeType = ((RadioButton)sender).Value.ToString();
    }

    private void SaveEvent(object sender, EventArgs e)
    {
        MessagingCenter.Send(this, "Query", Query);
    }
}