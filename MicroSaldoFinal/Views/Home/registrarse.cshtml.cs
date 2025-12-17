// Archivo: Pages/Home/registrarse.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations; // Necesario para las Data Annotations

namespace MicroSaldo.Pages.Home
{
    public class RegistrarseModel : PageModel
    {
        // Esta clase (InputModel) define los campos que el usuario debe llenar.
        public class InputModel
        {
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [Display(Name = "Apellido")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [Display(Name = "N�mero de Documento")]
            [DataType(DataType.Text)] // Puedes usar [RegularExpression] si tienes un formato espec�fico
            public string Documento { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [Display(Name = "Tel�fono")]
            [DataType(DataType.PhoneNumber)]
            public string Telefono { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [EmailAddress(ErrorMessage = "Ingrese una direcci�n de correo v�lida.")]
            [Display(Name = "Correo Electr�nico")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contrase�a")]
            public string Contrasena { get; set; }

            // Opcional: Para confirmar la contrase�a
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contrase�a")]
            [Compare("Contrasena", ErrorMessage = "La contrase�a y la confirmaci�n no coinciden.")]
            public string ConfirmarContrasena { get; set; }
        }

        // Esta propiedad es la que enlaza los datos del formulario con tu modelo.
        [BindProperty]
        public required InputModel Input { get; set; }

        // M�todo que se ejecuta al cargar la p�gina por primera vez (GET request)
        public void OnGet()
        {
            // Aqu� puedes inicializar valores si fuera necesario
        }

        // M�todo que se ejecuta al enviar el formulario (POST request)
        public IActionResult OnPost()
        {
            // 1. **Validaci�n**: Verifica si las Data Annotations (Required, EmailAddress, etc.) se cumplen.
            if (!ModelState.IsValid)
            {
                // Si la validaci�n falla, regresa la misma p�gina para mostrar los errores.
                return Page();
            }

            // 2. **L�gica de Registro**: Si es v�lido, aqu� ir�a tu c�digo para:
            //    - Hashear la contrase�a (�NUNCA la guardes en texto plano!)
            //    - Guardar el nuevo usuario en tu base de datos (DB).

            // Ejemplo de c�mo acceder a los datos:
            // string nombreUsuario = Input.Nombre;
            // string correoUsuario = Input.Correo;
            // string contrasenaSinHash = Input.Contrasena; // �Recuerda hashear!

            // Por ahora, solo simularemos un �xito y redirigiremos.

            // 3. **Redirecci�n**: Si el registro es exitoso, lo rediriges a la p�gina de inicio de sesi�n.
            return RedirectToPage("inicio de sesion");
            // Esto redirige a /Home/inicio de sesion
        }
    }
}