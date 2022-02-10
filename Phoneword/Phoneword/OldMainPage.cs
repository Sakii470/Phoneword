using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Phoneword
{
    public class OldMainPage : ContentPage
    {

        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;

        public OldMainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15                           //ustawienie kontrolek w stos
            };

            panel.Children.Add(new Label
            {
                Text = "Enter a Phoneword:",                                        //dodanie Label-etykiety 
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "1-855-XAMARIN",                                         //dodanie pola do danych wejsciowych-wprowadzenie tekstu  
            });

            panel.Children.Add(translateButton = new Button
            {
                Text = "Translate"                                             //dodanie przycisku
            });



            panel.Children.Add(callButton = new Button
            {
                Text = "Call",
                IsEnabled = false,                                            //dodanie przycisku (nieaktywny)
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;                                    //wywołanie 2 funkcji zdefiniowanych niżej
            this.Content = panel;                                            //przypisanie stosu stacklayout do Content-głownej strony.



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



