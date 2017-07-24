using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Tabs.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomVision : ContentPage
    {
        Uri uri;

        public CustomVision()
        {
            InitializeComponent();
        }
        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });
            await MakePredictionRequest(file);
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "c03a2b1920814aa6b76f909335d6caac");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/6f01caf1-69d3-47f6-bebf-7fc26900a96e/image?iterationId=464a5036-2f1e-4d20-b92e-0631e294656f";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                string tag = "";

                if (response.IsSuccessStatusCode)
                {
                   var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    double max = responseModel.Predictions.Max(m => m.Probability);

                    List<Prediction> predictions = responseModel.Predictions;

                    foreach (Prediction p in predictions)
                    {
                        if (p.Probability == max)
                        {
                            tag = p.Tag;
                        }
                    }
                }
                await getDetails(tag);
                file.Dispose();
            }
        }
        async Task getDetails(string tag)
        {
            List<module2kmai806table> instrumentRows = await AzureManager.AzureManagerInstance.getRows();

            uri = instrumentRows[0].url;
            name.Text = instrumentRows[0].Name;
            url.Text = instrumentRows[0].url.ToString();
        }
    }
}
