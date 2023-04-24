using System;
using System.Windows;
using RestSharp;
using Newtonsoft.Json;

namespace testandoInterface
{
    public partial class MainWindow : Window
    {
        private readonly RestClient client = new RestClient("http://localhost:5246");

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
{
    string usuario = txtUsername.Text;
    string senha = txtPassword.Password;
    WindowStartupLocation = WindowStartupLocation.CenterScreen;

    try
    {
        // Faz a requisição HTTP para o backend
        RestClient client = new RestClient("http://localhost:5246");
        RestRequest request = new RestRequest("autenticacao", Method.Post);
        request.AddJsonBody(new AutenticacaoRequest { Usuario = usuario, Senha = senha });

        RestResponse response = await client.ExecuteAsync(request);

        string responseBody = response.Content ?? "";

        // Analisa a resposta do backend
        AutenticacaoResponse? authResponse = null;
        try
        {
            authResponse = JsonConvert.DeserializeObject<AutenticacaoResponse>(responseBody);
        }
        catch (Exception ex)
        {
            lblStatus.Content = "An error occurred while deserializing the response: " + ex.Message;
            return;
        }

        bool isAuthenticated = authResponse?.Autenticado ?? false;

        if (isAuthenticated)
{
            lblError.Visibility = Visibility.Hidden; // Esconde a mensagem de erro
            var segundaPagina = new SegundaPagina();
            segundaPagina.Show();
            this.Close();
}
        else
        {
            lblError.Visibility = Visibility.Visible; // Mostra a mensagem de erro
            lblError.Text = "Invalid username or password.";
        }
    }
    catch (Exception ex)
    {
        // Exibe uma mensagem de erro caso ocorra alguma exceção
        lblStatus.Content = "An error occurred: " + ex.Message;
    }
}
    }

    public class AutenticacaoRequest
{
    public string Usuario { get; set; } = "";
    public string Senha { get; set; } = "";
}

    public class AutenticacaoResponse
    {
        public bool Autenticado { get; set; }
    }
}
