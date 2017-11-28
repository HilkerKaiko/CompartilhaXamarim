using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CompartilhaXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            btnTake.Clicked += BtnTake_Clicked;
            btnUpload.Clicked += BtnUpLoad_Clicked;
            btnSemImg.Clicked += BtnSemImg_Clicked;
        }
            byte[] img = null;

            public byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }

            private async void BtnUpLoad_Clicked(object sender, EventArgs e)
            {
                try
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Indisponível, Recurso indisponível", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,

                    });

                    if (file == null)
                    {
                        return;
                    }

                    img = ReadFully(file.GetStream());
                    imgFoto.Source = ImageSource.FromStream(() => file.GetStream());

                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Dificuldade", "Dificuldade em tirar Foto", "ok");
                }

            }

        private Task DisplayAlert(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        private async void BtnTake_Clicked(object sender, EventArgs e)
            {
                try
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)

                    {
                        await DisplayAlert("Indisponível, Recurso indisponível", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        SaveToAlbum = true,
                        Name = "foto.jpg",
                        DefaultCamera = CameraDevice.Front,
                    });

                    

                    img = ReadFully(file.GetStream());
                    imgFoto.Source = ImageSource.FromStream(() => file.GetStream());
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Dificuldade", "Dificuldade em executar a ação", "ok");
                }
            }

            private void BtnSemImg_Clicked(object sender, EventArgs e)
            {
                img = null;
                imgFoto.Source = null;
            }




            private void BtnMsg_Clicked(object sender, EventArgs e)
            {
                ShareMessage msg = new ShareMessage();
                msg.Text = "Mensagem";
                msg.Title = "Aviso";
                msg.Url = "https://www.youtube.com/watch?v=8iiHmXiDliQ";

                var opt = new ShareOptions();

                CrossShare.Current.Share(msg, opt);
            }

            private void btnNav_Clicked(object sender, EventArgs e)
            {
                BrowserOptions opt = new BrowserOptions();

                CrossShare.Current.OpenBrowser("https://www.youtube.com/watch?v=8iiHmXiDliQ");
            }

            private void btnTransf_Clicked(object sender, EventArgs e)
            {
                CrossShare.Current.SetClipboardText("Texto");
            }
        }
    }

