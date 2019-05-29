using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ECNU_Electricity_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.split1.IsPaneOpen = !this.split1.IsPaneOpen;
        }
        private async System.Threading.Tasks.Task refreshAsync() {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.

            headers.Accept.TryParseAdd("application/json, text/javascript, */*; q=0.01");
            headers.AcceptEncoding.TryParseAdd("gzip, deflate");
            headers.Add("X-Requested-With", "XMLHttpRequest");
            Uri requestUri = new Uri("https://wx.ecnu.edu.cn/CorpWeChat/card/dogetEle.html?area=zb&point=5208");

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                Windows.Web.Http.HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                httpResponse = httpResponseMessage;
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                text2.Text = httpResponseBody;

            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            if(campus.SelectedIndex==-1)
            {
                await new MessageDialog("没有选择校区","错误").ShowAsync();
                return;
            }
            split1.IsPaneOpen = false;
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            localSettings.Values["campus"] = campus.SelectedIndex;
            localSettings.Values["room"] = room.Text;
            refreshAsync();
        }
    }
}
