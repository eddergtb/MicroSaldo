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
            [Display(Name = "Número de Documento")]
            [DataType(DataType.Text)] // Puedes usar [RegularExpression] si tienes un formato específico
            public string Documento { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [Display(Name = "Teléfono")]
            [DataType(DataType.PhoneNumber)]
            public string Telefono { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [EmailAddress(ErrorMessage = "Ingrese una dirección de correo válida.")]
            [Display(Name = "Correo Electrónico")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Contrasena { get; set; }

            // Opcional: Para confirmar la contraseña
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña")]
            [Compare("Contrasena", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
            public string ConfirmarContrasena { get; set; }
        }

        // Esta propiedad es la que enlaza los datos del formulario con tu modelo.
        [BindProperty]
        public InputModel Input { get; set; }

        // Método que se ejecuta al cargar la página por primera vez (GET request)
        public void OnGet()
        {
            // Aquí puedes inicializar valores si fuera necesario
        }

        // Método que se ejecuta al enviar el formulario (POST request)
        public IActionResult OnPost()
        {
            // 1. **Validación**: Verifica si las Data Annotations (Required, EmailAddress, etc.) se cumplen.
            if (!ModelState.IsValid)
            {
                // Si la validación falla, regresa la misma página para mostrar los errores.
                return Page();
            }

            // 2. **Lógica de Registro**: Si es válido, aquí iría tu código para:
            //    - Hashear la contraseña (¡NUNCA la guardes en texto plano!)
            //    - Guardar el nuevo usuario en tu base de datos (DB).

            // Ejemplo de cómo acceder a los datos:
            // string nombreUsuario = Input.Nombre;
            // string correoUsuario = Input.Correo;
            // string contrasenaSinHash = Input.Contrasena; // ¡Recuerda hashear!

            // Por ahora, solo simularemos un éxito y redirigiremos.

            // 3. **Redirección**: Si el registro es exitoso, lo rediriges a la página de inicio de sesión.
            return RedirectToPage("inicio de sesion");
            // Esto redirige a /Home/inicio de sesion
        }
    }
}