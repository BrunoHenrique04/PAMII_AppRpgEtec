using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{

    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService _uService;
        public ICommand AutenticarCommand { get; set; }

        public UsuarioViewModel()
        {
            _uService = new UsuarioService();
            InicializarCommands();
        }

        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => AutenticarUsuario());
        }

        #region AtributosPropiedades  
        private string login = string.Empty;

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertychanged();

            }
        }

        public string Password
        {
            get => password;

            set
            {

                password = value;
                OnPropertychanged();
            }

        }
        private string password = string.Empty;

        #endregion

        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Password;
                Usuario uAutenticado = await _uService.AutenticarUsuarioAsync(u);
                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem Vindo {u.Username}";
                    Preferences.Set("UsuarioToken", uAutenticado.Token);
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);


                    Application.Current.MainPage = new MainPage();

                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorretos", "Ok");

                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }

    }
}