using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using Newtonsoft.Json;

namespace ustawiacz.net {
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public class Dane {
        public string cid;
        public string tytul;
        public string opis;
        public Obrazek maly;
        public Obrazek duzy;
    }
    public class Obrazek {
        public string Kod;
        public string Tekst;
    }

    public class Presencja {
        public DiscordRpcClient client;

        public Presencja(string cid) {
            client = new DiscordRpcClient(cid);


            client.Initialize();
        }
        public void Ustaw(
            string details,
            bool start,
            string state = null,
            string largeKey = null,
            string largeText = null,
            string smallKey = null,
            string smallText = null
            ) {
            RichPresence xiaomi = new RichPresence() {
                Details = details,
                State = state,
                Assets = new Assets() {
                    LargeImageKey = largeKey,
                    LargeImageText = largeText,
                    SmallImageKey = smallKey,
                    SmallImageText = smallText
                }
            };
            if (start) {
                xiaomi.Timestamps = new Timestamps() {
                    Start = DateTime.UtcNow
                };
            }
            client.SetPresence(xiaomi);
        }
    }
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            
            WczytajDane();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            string cidC = cid.Text;
            string tytulC = tytul.Text;
            string opisC = opis.Text;
            if (String.IsNullOrEmpty(cidC)) {
                komunikat.Text = "Client ID jest wymagane!";
                gopher.IsOpen = true;
            } else if (String.IsNullOrEmpty(tytulC)) {
                komunikat.Text = "Tytuł jest wymagany!";
                gopher.IsOpen = true;
            }
            else {
                Presencja present = new Presencja(cidC);
                Dane cida = new Dane() {
                    cid = cidC,
                    tytul = tytulC,
                    opis = opisC,
                    duzy = new Obrazek { Kod = duzyK.Text, Tekst = duzyT.Text },
                    maly = new Obrazek { Kod = malyK.Text, Tekst = malyT.Text }
                };
                Zapisz<Dane>(cida);
                present.Ustaw(tytulC, (bool)(uplynelo.IsChecked), opisC, duzyK.Text, duzyT.Text, malyK.Text, malyT.Text);
            }
        }

        private void WczytajDane() {
            if(File.Exists("data.json")) {
                Dane wczytane = Czytaj<Dane>();
                    cid.Text = wczytane.cid;
                    tytul.Text = wczytane.tytul;
                    opis.Text = wczytane.opis;
                    duzyK.Text = wczytane.duzy.Kod;
                    duzyT.Text = wczytane.duzy.Tekst;
                    malyK.Text = wczytane.maly.Kod;
                    malyT.Text = wczytane.maly.Tekst;
                
            }
            
        }
        public static void Zapisz<T>(T objectToWrite) where T : new() {
            TextWriter writer = null;
            try {

                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter("data.json", false);
                writer.Write(contentsToWriteToFile);
            }
            finally {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T Czytaj<T>() where T : new() {
            TextReader reader = null;
            try {
                    reader = new StreamReader("data.json");
                    var fileContents = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(fileContents);
                
            }
            finally {
                if (reader != null)
                    reader.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("https://gist.github.com/pizza61/ba7d0178426a42207b40c88b813793d1");
        }
    }
}
