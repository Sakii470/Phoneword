using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Phoneword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        string translatedNumber;
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnTranslate(object sender, EventArgs e)                 //funkcja tłumaczaca 
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);  //wywołanie funkcji z innej klasy i przypisanie wrtości do zmiennej

            if (!string.IsNullOrEmpty(translatedNumber))                               //jesli tłumaczenie pomyslne-> aktywuj button call i zmien text na nim
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;                                        //w przeciwnym przypadku zostaw przycisk nieaktywny.
                callButton.Text = "Call";
            }
        }

        async void OnCall(object sender, System.EventArgs e)                       //funkcja odpowida za "dzwonienie"
        {
            if (await this.DisplayAlert(
                "Dial a Number",
                "Would you like to call " + translatedNumber + "?",
                "Yes",
                "No"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);                           //Proba wybrania numeru telefonu. funkcja Xamarin.Essential
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");       //wyświetlanie alletrów o nieprawidłowym numerze jezeli nie ma go w danym urzadzeniu.                        
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }
    }
}