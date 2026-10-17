using System;

namespace Clock;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
	}

    private void CityClicked(object sender, EventArgs e)
    {
        var city = ((Button)sender).Text;
        switch (city)
        {
            case "Сургут":
                worldClock.Offset = TimeSpan.FromHours(5);
                break;
            case "Москва":
                worldClock.Offset = TimeSpan.FromHours(3);
                break;
            case "Токио":
                worldClock.Offset = TimeSpan.FromHours(9);
                break;
            case "Нью-Йорк":
                worldClock.Offset = TimeSpan.FromHours(-4);
                break;

        }
        
    }


}


